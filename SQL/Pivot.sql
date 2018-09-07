--Pivot: Gyakorlatilag sorokból oszlopokat készít. Mivel lehet, hogy több sorból kell egy oszlopot csinálnia, ezért meg
--kell mondani,hogy a több értékből hogyan képezze az egy értéket, tehát valamilyen aggregáló függvényt kell megadni.
--Lenti példa: Minden Name oszlopban szereplő értékből (jelen esetben Apr,Kamat: ezt előre meg kell adni, hogy milyen 
--értékek, vagy dinamikus sql-el kell összeállítani) egy-egy oszlopot készít. Ha egy ProductName,RIV,Name sorhoz több
--különböző Value is tartozik, akkor tudnia kell, hogy a Value értékeket hogyan aggregálja. (Pl: SUM, AVG, MIN, MAX)

--Unpivot: A Pivot ellentéte, oszlopokból csinál sorokat.




--create table #ize
--(
--	ProductName varchar(10),
--	RIV int,
--	Name varchar(10),
--	Value int
--)
--insert into #ize(ProductName,RIV,Name,Value) values ('product1',10,'Apr',11)
--go
--insert into #ize(ProductName,RIV,Name,Value) values ('product1',10,'Apr',50)
--go
--insert into #ize(ProductName,RIV,Name,Value) values ('product1',20,'Apr',12)
--go
--insert into #ize(ProductName,RIV,Name,Value) values ('product1',30,'Apr',13)
--go
--insert into #ize(ProductName,RIV,Name,Value) values ('product1',10,'Kamat',14)
--go
--insert into #ize(ProductName,RIV,Name,Value) values ('product1',20,'Kamat',15)
--go
--insert into #ize(ProductName,RIV,Name,Value) values ('product1',30,'Kamat',16)
--go
--insert into #ize(ProductName,RIV,Name,Value) values ('product2',10,'Apr',17)
--go
--insert into #ize(ProductName,RIV,Name,Value) values ('product2',20,'Apr',18)
--go
--insert into #ize(ProductName,RIV,Name,Value) values ('product2',30,'Apr',19)
--go
--insert into #ize(ProductName,RIV,Name,Value) values ('product2',10,'Kamat',20)
--go
--insert into #ize(ProductName,RIV,Name,Value) values ('product2',20,'Kamat',21)
--go
--insert into #ize(ProductName,RIV,Name,Value) values ('product2',30,'Kamat',22)
--go


select pvt.* from
(select ProductName,RIV,Name,Value from #ize) as s
pivot (sum(Value) for Name in(Apr,Kamat)) as pvt
order by ProductName

select pvt.* from
(select ProductName,RIV,Name,Value from #ize) as s
pivot (avg(Value) for Name in(Apr,Kamat)) as pvt
order by ProductName

--drop table #ize

