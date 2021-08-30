       
       declare 
             @ugyfelId int,
             @kerdoivId int,
             @statusCode nvarchar(1),
             @modifierUserId bigint,
             @SWP_Ret_Value int

       select @modifierUserId = 0

       create table #InsertedFoos (CID int, KID int, StatusCode nvarchar(2) )

       insert into #InsertedFoos
       select 
             l.CID,
             l.KID,
             ll.NewClosedStateCode
       from Foo l
       join #FoomenFoos ll
             on ll.FooId = l.ID
       where ll.NewClosedStateCode is not null

       declare fooCursor cursor forward_only
       for
       select 
             CID,
             KID,
             StatusCode
       from #InsertedFoos
       open fooCursor

       fetch next from fooCursor into @ugyfelId, @kerdoivId, @statusCode

       while @@fetch_status = 0
       begin
             exec sp_set_foo_status @al_ugyfel_id = @ugyfelId, @al_kerdoiv_id = @kerdoivId, @as_status_code = @statusCode, @al_ModifierUserID = @modifierUserId, @SWP_Ret_Value = @SWP_Ret_Value output
             fetch next from leadCursor into @ugyfelId, @kerdoivId, @statusCode
       end

       close fooCursor
       deallocate fooCursor
       
       ------------------------------------------------------------------------------------------
       -MÁSIK MÓDSZER - csak egy helyen kell a fetch!!!
       ------------------------------------------------------------------------------------------
       declare fooCursor cursor forward_only     --Ha nem akarom a rekordokat módosítani, akkor declare fooCursor cursor fast_forward
       for
       select 
             CID,
             KID,
             StatusCode
       from #InsertedFoos
       open fooCursor
       
       while 1=1
		begin
			fetch next from fooCursor into @ugyfelId, @kerdoivId, @statusCode
			if @@fetch_status != 0
				break		
			print 'exec akarmilyen SP'
			exec sp_set_foo_status @al_ugyfel_id = @ugyfelId, @al_kerdoiv_id = @kerdoivId, @as_status_code = @statusCode, @al_ModifierUserID = @modifierUserId, @SWP_Ret_Value = @SWP_Ret_Value output
		end

       close fooCursor
       deallocate fooCursor
       
