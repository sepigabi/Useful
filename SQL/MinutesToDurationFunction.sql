set ansi_nulls on
go
set quoted_identifier on
go

if exists (select * from sysobjects where name = 'MinutesToDuration' and type='FN')
begin
	drop function [MinutesToDuration]
end
go
CREATE FUNCTION [dbo].[MinutesToDuration]
(
    @minutes int 
)
RETURNS nvarchar(30)

AS
BEGIN
declare @hours  nvarchar(20)

SET @hours = 
	CASE WHEN @minutes >= 1440 THEN
		(SELECT CAST((@minutes / 1440) AS VARCHAR(7)) + 'd ' +  
						CASE WHEN (@minutes % 1440) > 0 THEN
							CAST(((@minutes % 1440) / 60) AS VARCHAR(2)) + 'h ' +
								CASE WHEN ((@minutes % 1440) % 60) > 0 THEN
									CAST(((@minutes % 1440) % 60) AS VARCHAR(2)) + 'm'
								ELSE ''
								END
						ELSE
							''
						END)
	ELSE
		CASE WHEN @minutes >= 60 THEN
			(SELECT CAST((@minutes / 60) AS VARCHAR(2)) + 'h ' +  
					CASE WHEN (@minutes % 60) > 0 THEN
						CAST((@minutes % 60) AS VARCHAR(2)) + 'm'
					ELSE
						''
					END)
		ELSE 
			CAST((@minutes % 60) AS VARCHAR(2)) + 'm'
		END
	END
return @hours
END
go
grant exec on [MinutesToDuration] to ApplicationUsers
