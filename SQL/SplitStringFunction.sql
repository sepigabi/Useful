SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER function [dbo].[Split]
(
    @string     nvarchar (4000)
    ,@delimiter nvarchar (10)
)
returns @ValueTable table ([Value] nvarchar(4000))
begin
    declare @nextString nvarchar(4000)
    declare @pos        int
    declare @nextPos    int
    declare @commaCheck nvarchar(1)

    --Initialize
    select
        @nextString = ''
    select 
        @commaCheck = right(@string,1)

    --Check for trailing Comma, if not exists, INSERT
    if (@commaCheck <> @delimiter )
        select 
            @string = @string + @delimiter

    --Get position of first Comma
    select 
        @pos = charindex(@delimiter,@string)
    select 
        @nextPos = 1

    --Loop while there is still a comma in the String of levels
    while (@pos <> 0)
    begin
        select 
            @nextString = substring(@string,1,@pos - 1)
        insert into @ValueTable 
        ( 
            [Value]
        ) 
        Values 
        (
            @nextString
        )
        select 
            @string = substring(@string,@pos +1,len(@string))
        select 
            @nextPos = @pos
        select 
            @pos = charindex(@delimiter,@string)
    end

    return
end

--usage
--select value
--from dbo.Split(@Categories,’,’)
