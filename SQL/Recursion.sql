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

------------------------------------------------------------------

create procedure GetDescendantLocationIds
(
	@LocationId int
)
as
begin
	-- v1.0

 ;with CTE
    (
        Id
    )
    as 
    (
        select
            ABS(l2.Id)
        from Location l1
		join Location l2
			on l2.ParentLocationId = l1.Id
		where l1.Id = @LocationId

        union all
        select
           ABS(l.Id)
        from Location l
        join CTE c
			on c.Id = l.ParentLocationId
    )
    
    select distinct * 
    from CTE

end
