use ADATBÁZISNEVE

-- hinyázó indexek, lehet, hogy ugyan arra a táblára többet is ad, ezek közt lehet olyan amiket össze lehet vonni egy indexbe, lehet olyan amivel le akarja duplikálni a teljes táblát, 
-- mert az include-ba berak minden mezőt, na ezt csak kis táblákra szabad és csak akkor akkor ha nem változnak rajta gyorsan az adatok :)
-- ez csak a nagyobb falatokat kéri le
select 'MI'
       , Getdate() as SavedAt
       , @@ServerName
       , s.user_seeks * s.avg_total_user_cost * ( s.avg_user_impact * 0.01 ) as Effectivity
       , 'Create index ix_??? on '+i.statement+' ('+isnull(equality_columns,'')+case when equality_columns is not null then ',' else '' end
             + isnull(inequality_columns,'')+')'+isnull(' include ('+included_columns+')','') CreateIndex
       , s.user_seeks + s.user_scans as R
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
where (((user_scans+user_seeks) > 50 and (avg_total_user_cost * (avg_user_impact/100.0))>50)
       or ((user_scans+user_seeks) > 1000))
       and i.database_id = db_id()
order by CreateIndex
