SELECT 
    te.name AS eventtype
    ,t.loginname
    ,t.starttime
    ,t.objectname
    ,t.databasename
    ,t.hostname
    ,t.ntusername
   FROM sys.fn_trace_gettable
(
	convert(nvarchar(256),(select value from fn_trace_getinfo(NULL) where property=2 and traceid = 1))
    , -1
) t
left JOIN sys.trace_events as te 
    ON t.eventclass = te.trace_event_id 
--where t.objectname = 'GetUIMessages'
order by t.starttime
