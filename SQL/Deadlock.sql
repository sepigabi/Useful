--csak az utolsó restart óta bekövetkezett deadlockot látja

select @@VERSION

--- 2012 felett
;WITH
         CurrentSystemHealthTraceFile AS (
             SELECT CAST(target_data AS xml).value('(/EventFileTarget/File/@name)[1]', 'varchar(255)') AS FileName
             FROM sys.dm_xe_session_targets
             WHERE
                    target_name = 'event_file'
                    AND CAST(target_data AS xml).value('(/EventFileTarget/File/@name)[1]', 'varchar(255)') LIKE '%\system[_]health%'
       )
       , AllSystemHealthFiles AS (
             SELECT 
                    REVERSE(SUBSTRING(REVERSE(FileName), CHARINDEX(N'\', REVERSE(FileName)), 255)) + N'system_health*.xel' AS FileNamePattern
             FROM CurrentSystemHealthTraceFile
             )
       , DeadLockReports AS (
             SELECT CAST(event_data AS xml) AS event_data
             FROM AllSystemHealthFiles
             CROSS APPLY sys.fn_xe_file_target_read_file ( FileNamePattern, NULL, NULL, NULL) AS xed
             WHERE xed.object_name like 'xml_deadlock_report'
       )
SELECT TOP 10 *
       , DATEADD(hour, DATEDIFF(hour, SYSUTCDATETIME(), SYSDATETIME()), event_data.value('(/event/@timestamp)[1]', 'datetime2')) AS LocalTime
       , event_data AS DeadlockReport
FROM DeadLockReports;
GO

--- 2008 R2 részletes XMLben

SELECT
      DATEADD(hour, DATEDIFF(hour, SYSUTCDATETIME(), SYSDATETIME()), xed.value('@timestamp', 'datetime2')) AS LocalTime
       , xed.query('.') AS Extend_Event
FROM
(
       SELECT CAST([target_data] AS XML) AS Target_Data
       FROM sys.dm_xe_session_targets AS xt
       INNER JOIN sys.dm_xe_sessions AS xs
       ON xs.address = xt.event_session_address
       WHERE xs.name = N'system_health'
       AND xt.target_name = N'ring_buffer'
) AS XML_Data
CROSS APPLY Target_Data.nodes('RingBufferTarget/event[@name="xml_deadlock_report"]') AS XEventData(xed)
ORDER BY LocalTime DESC;
GO

