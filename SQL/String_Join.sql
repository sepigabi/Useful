-- from sql server 2017 STRING_AGG
--Source:

SELECT CAST(TextColumn + ';' AS VARCHAR(MAX)) 
FROM SomeTable
WHERE SomeOtherColumn = 'Y'
FOR XML PATH ('')

--If you don't like the trailing ; you can remove the character from the result.
