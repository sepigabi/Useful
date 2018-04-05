create procedure [SavePartners]
(
    @objects xml
)
as
	begin

		create table #Partner
		(
			Id int,
			Comment varchar(max),
			Name varchar(50),
			Email varchar(100),
			PhoneNumber varchar(20)
		)

		insert into #Partner
		select
			Partners.[Partner].value('Id[not(@xsi:nil = "true")][1]', 'int')							      as Id,
			Partners.[Partner].value('Comment[not(@xsi:nil = "true")][1]', 'varchar(max)')			as Comment,
			Partners.[Partner].value('Name[not(@xsi:nil = "true")][1]', 'varchar(50)')					as Name,
			Partners.[Partner].value('Email	[not(@xsi:nil = "true")][1]', 'varchar(100)')				as Email,
			Partners.[Partner].value('PhoneNumber[not(@xsi:nil = "true")][1]', 'varchar(20)')		as PhoneNumber
		from @objects.nodes('/ArrayOfPartner/Partner') as Partners([Partner])
  end
