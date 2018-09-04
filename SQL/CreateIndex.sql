set xact_abort on;
go
begin tran;

if not exists(select 1 from sysobjects o join sysindexes i on i.id = o.id where o.name = 'Lead' and i.name = 'IX_DMCode_ScreenTargetId')
begin
	CREATE NONCLUSTERED INDEX [IX_DMCode_ScreenTargetId]
	ON [dbo].[Lead] ([DM_Code],[ScreenTargetId])
	INCLUDE ([ID],[ReqType],[CurrentStatus_Id],[BrmId])
end
else
begin
	print 'index is already exists!'
end

commit tran;
