--Hiányzó indexek lekérdezése
--itt csak azzal érdemes foglalkozni ahol az Effectivity 1000 fölött van

select top 50 @@ServerName
             , s.user_seeks * s.avg_total_user_cost * ( s.avg_user_impact * 0.01 ) as Effectivity
             , s.user_seeks + s.user_scans as Reads
             , 'Create index ix_??? on '+i.statement+' ('+isnull(equality_columns,'')+case when equality_columns is not null then ',' else '' end
                    + isnull(inequality_columns,'')+')'+isnull(' include ('+included_columns+')','') CreateIndex
             , avg_system_impact
             , avg_total_system_cost
             , avg_total_user_cost
             , avg_user_impact
             , database_id
             , equality_columns
             , group_handle
             , included_columns
             , index_group_handle
             , i.index_handle
             , inequality_columns
             , last_system_scan
             , last_system_seek
             , last_user_scan
             , last_user_seek
             , object_id
             , statement
             , system_scans
             , system_seeks
             , unique_compiles
             , user_scans
             , user_seeks
       from sys.dm_db_missing_index_details i
       join sys.dm_db_missing_index_groups g
             on g.index_handle = i.index_handle
       join sys.dm_db_missing_index_group_stats s
             on g.index_group_handle = s.group_handle 
       where i.database_id = db_id()
       order by Effectivity desc

--Valószínűleg feleslegesen terhelő indexek
--- nem túl effektív indexek: ezek olyan indexek, amiknek a karban tartására sokat költ az szerver, de utána keveset használja
-- értelemszerűen ebből is csak óvatosan lehet válogatni hogy mit akarunk törölni :)
SELECT  'MNU',
              o.name as TableName,
        i.[name] AS IndexName ,
        ddius.[user_seeks] + ddius.[user_scans] + ddius.[user_lookups] AS Reads ,
        ddius.[user_updates] AS Writes ,
              ddius.[user_updates] - (ddius.[user_seeks] + ddius.[user_scans] + ddius.[user_lookups] ) as Diff,
        SUM(SP.rows) AS TotalRows,
        'DROP index ['+i.name+'] on ['+o.name+']' as DropStm
FROM    sys.dm_db_index_usage_stats ddius
        INNER JOIN sys.indexes i ON ddius.[object_id] = i.[object_id]
                                     AND i.[index_id] = ddius.[index_id]
        INNER JOIN sys.partitions SP ON ddius.[object_id] = SP.[object_id]
                                        AND SP.[index_id] = ddius.[index_id]
        INNER JOIN sys.objects o ON ddius.[object_id] = o.[object_id]
        INNER JOIN sys.sysusers su ON o.[schema_id] = su.[UID]
WHERE   ddius.[database_id] = DB_ID() -- current database only
        AND OBJECTPROPERTY(ddius.[object_id], 'IsUserTable') = 1
        AND user_updates > ( user_seeks + user_scans + user_lookups )
        AND i.index_id > 1
GROUP BY su.[name] ,
        o.[name] ,
        i.[name] ,
        ddius.[user_seeks] + ddius.[user_scans] + ddius.[user_lookups] ,
        ddius.[user_updates]
HAVING  ddius.[user_seeks] + ddius.[user_scans] + ddius.[user_lookups] = 0
ORDER BY Diff DESC

--Töredezett indexek:
-- töredezett indexek: ezeken csak a DROP/CREATE segít, de csak akkor érdemes vele foglalkozni, ha nagy a tábla
-- azaz a page_count nagy
--- EZ SOKÁIG FUTHAT!
select o.name as TableName
       , ix.name as IndexName
       , i.avg_fragmentation_in_percent
       , i.fragment_count
       , i.page_count
       , i.index_depth 
from sys.dm_db_index_physical_stats(DB_ID(),default,default,default,default) i
join sysindexes ix
       on ix.indid = i.index_id
       and ix.id = i.object_id
join sysobjects o
       on o.id = i.object_id
where ix.name is not null -- ezek a táblák maguk
order by i.avg_fragmentation_in_percent desc

--Bírja a disk?
--- Latch lekérdezése: ez azt mutatja meg, hogy mennyit kell várni egy index page betöltésére azért mert egy másik betöltésra várunk
--- ez a diszk -> memória betöltés, ha nagy a avg_page_io_latch_wait_in_ms akkor nem bírja a diszk a terhelést (nagy ha sok van ami nagyobb mint 30ms)
Use BinaritKHR9
select AVG(avg_page_io_latch_wait_in_ms), MAX(avg_page_io_latch_wait_in_ms)
from (
SELECT  'LAW' l,
              DB_NAME(ddios.[object_id]) as DBName,
              OBJECT_SCHEMA_NAME(ddios.[object_id]) as SchemaName,
        OBJECT_NAME(ddios.[object_id])  AS TableName,
        i.[name] AS IndexName ,
        ddios.page_io_latch_wait_count as WatCount,
        ddios.page_io_latch_wait_in_ms as Wait_ms,
        1.0*( ddios.page_io_latch_wait_in_ms / ddios.page_io_latch_wait_count ) as avg_page_io_latch_wait_in_ms
FROM    sys.dm_db_index_operational_stats(DB_ID(), NULL, NULL, NULL) ddios
    INNER JOIN sys.indexes i 
              ON ddios.[object_id] = i.[object_id]
            AND i.index_id = ddios.index_id
WHERE   ddios.page_io_latch_wait_count > 0
        AND OBJECTPROPERTY(i.OBJECT_ID, 'IsUserTable') = 1
) as t
ORDER BY avg_page_io_latch_wait_in_ms DESC


--és még egy apróság 😊

--- index page lock: összeszedö, hogy hány és milyen hosszú lockok szoktak lenni, ha itt magasak a számok, akkor
-- ki kell találni, hogy lehet megkerülni a lockokat :)
SELECT  'ILC',
              OBJECT_SCHEMA_NAME(ddios.[object_id]) as SchemaName,
        OBJECT_NAME(ddios.[object_id])  AS TableName,
        i.name AS IndexName ,
        ddios.partition_number as PartitionCount,
        ddios.page_lock_wait_count as PageLockWaitCount,
        ddios.page_lock_wait_in_ms as PageLockWaitCountms,
        CASE WHEN DDMID.database_id IS NULL THEN 'N' ELSE 'Y' END AS missing_index_identified
FROM    sys.dm_db_index_operational_stats(DB_ID(), NULL, NULL, NULL) ddios
   INNER JOIN sys.indexes i 
              ON ddios.OBJECT_ID = i.OBJECT_ID
                    AND ddios.index_id = i.index_id
       LEFT OUTER JOIN ( SELECT DISTINCT
                                                      database_id ,
                                                      OBJECT_ID
                                        FROM      sys.dm_db_missing_index_details
                                  ) AS DDMID 
              ON DDMID.database_id = ddios.database_id
                    AND DDMID.OBJECT_ID = ddios.OBJECT_ID
WHERE   ddios.page_lock_wait_in_ms > 0
ORDER BY ddios.page_lock_wait_count DESC
