sp_dropserver 'BINARIT-WKS92\MSSQLSERVER17'
sp_addserver 'BINARIT-WKS05\MSSQLSERVER17', local

--restart SQL Server 

select @@servername

--POST Update Steps if needed:
--Add the remote logins using the sp_addremotelogin command.
		--sp_addremotelogin [ @remoteserver = ] 'remoteserver' 
		--	[ , [ @loginame = ] 'login' ] 
		--	[ , [ @remotename = ] 'remote_name' ]
--Reconfigure Replication if this was setup.
--Reconfigure Database Mirroring if this was setup.
--Reconfigure Reporting Services if this was setup and connect to the new server name.

--source:
--https://www.mssqltips.com/sqlservertip/2525/steps-to-change-the-server-name-for-a-sql-server-machine/
--https://docs.microsoft.com/en-us/sql/database-engine/install-windows/rename-a-computer-that-hosts-a-stand-alone-instance-of-sql-server?view=sql-server-ver15
