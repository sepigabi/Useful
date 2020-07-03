public int SetStatus(Foo foo, String newStatusCode, long? userFocusId)
        {
            try
            {
                var returnValue = new SqlParameter("@SWP_Ret_Value", System.Data.SqlDbType.Int)
                {
                    Direction = System.Data.ParameterDirection.Output
                };
                var result = this.Database.SqlQuery<object>($"exec sp_set_status @al_ugyfel_id, @al_kerdoiv_id, @as_status_code, @al_FocusUserID, @SWP_Ret_Value = @SWP_Ret_Value output",
                    new SqlParameter("@al_ugyfel_id", foo.PccsCID),
                    new SqlParameter("@al_kerdoiv_id", foo.PccsKID),
                    new SqlParameter("@as_status_code", newStatusCode),
                    new SqlParameter("@al_FocusUserID", userFocusId ?? 0),
                    returnValue).ToList();
                Trace.WriteLine(new Logger($"Status has been sent to XXX at {DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss")}", 1, "Status -> Send to XXX", foo.ID));
                return (int) returnValue.Value;
            }
            catch (Exception e)
            {
                Trace.WriteLine(new Logger(e.ToEntireErrorMessage(), 1, "Status -> Send to XXX", foo.ID));
                throw;
            }
        }
