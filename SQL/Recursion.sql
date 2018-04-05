begin try
       ;with CTE (dat)
       as
       (
       select convert(datetime,'2000.01.01') as dat
       union all
       select dat+1 from CTE where dat < '2020.01.01'
       )
       select dat from CTE
end try
begin catch
       select ERROR_NUMBER(), ERROR_MESSAGE()
end catch
