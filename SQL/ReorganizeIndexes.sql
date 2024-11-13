use BinaritTOM2

declare @dbName varchar(max) = 'BinaritTOM2'

-- Find the average fragmentation percentage of all indexes and based on this the appropriate correction instruction
--	avg_fragmentation_in_percent value	Corrective statement
--	> 5% and < = 30%					ALTER INDEX REORGANIZE
--	> 30%								ALTER INDEX REBUILD WITH (ONLINE = ON)*

--https://www.beyondtrust.com/docs/privileged-identity/faqs/reorganize-and-rebuild-indexes-in-database.htm

SELECT 
	t.name as table_name,
	i.name as index_name,
	s.avg_fragmentation_in_percent,
	case when s.avg_fragmentation_in_percent > 30 then 'ALTER INDEX ' + i.name + ' ON ' + t.name + ' REBUILD; GO' 
	else 'ALTER INDEX ' + i.name + ' ON ' + t.name + ' REORGANIZE; GO ' end as action
FROM sys.dm_db_index_physical_stats (DB_ID(@dbName),0, NULL, NULL, NULL) AS s
JOIN sys.indexes AS i
	ON s.object_id = i.object_id 
	AND s.index_id = i.index_id
JOIN sys.tables t
	ON t.object_id = i.object_id
WHERE s.avg_fragmentation_in_percent > 5
GO
