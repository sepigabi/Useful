--In SQL Server 2012+, you can use OFFSET...FETCH:

SELECT
   <column(s)>
FROM
   <table(s)>
ORDER BY
   <sort column(s)>
OFFSET 1 ROWS   -- Skip this number of rows
FETCH NEXT 1 ROWS ONLY;  -- Return this number of rows
