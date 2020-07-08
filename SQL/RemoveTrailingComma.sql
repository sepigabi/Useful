--Source: https://stackoverflow.com/questions/31420597/sql-remove-last-comma-in-string

SELECT @string = REPLACE(@string + '<END>', ',<END>', '')
--if you can't be sure if last comma appear in string, use this:
SET @string = REPLACE(REPLACE(@string + '<END>', ',<END>', ''), '<END>', '')
