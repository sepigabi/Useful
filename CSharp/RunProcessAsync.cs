
                        var progressIndicator = new Progress<string>( ( outputItem ) =>
                        {
                            ThreadHelper.ThrowIfNotOnUIThread();
                            clientAppPane.OutputString( outputItem + "\n" );
                        } );
                        int msbuildExitCode = await StartMsBuildProcessAsync( msbuildCommandArguments, progressIndicator );
                        
 --------------------------------------------------------------------------------------------------------------------------------------------------------------------------
 
 protected async virtual Task<int> StartMsBuildProcessAsync( string msbuildCommandArguments, IProgress<string> progress )
        {
            using (System.Diagnostics.Process process = new System.Diagnostics.Process())
            {
                process.StartInfo.FileName = MsBuildFileName;
                process.StartInfo.Arguments = msbuildCommandArguments;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.CreateNoWindow = true;
                process.EnableRaisingEvents = true;
                Action<object, System.Diagnostics.DataReceivedEventArgs> outputHandler = ( s, ea ) => { progress.Report( ea.Data ); };
                Action<object, System.Diagnostics.DataReceivedEventArgs> errorHandler = ( s, ea ) => { progress.Report( ea.Data ); };
                return await RunProcessAsync( process, outputHandler, errorHandler ).ConfigureAwait( false );
            }
        }
        
 --------------------------------------------------------------------------------------------------------------------------------------------------------------------------
 
  private static Task<int> RunProcessAsync( System.Diagnostics.Process process, Action<object, System.Diagnostics.DataReceivedEventArgs> outputHandler, Action<object, System.Diagnostics.DataReceivedEventArgs> errorHandler )
        {
            var tcs = new TaskCompletionSource<int>();

            process.Exited += ( s, ea ) => tcs.SetResult( process.ExitCode );
            process.OutputDataReceived += ( s, ea ) => outputHandler( s, ea );
            process.ErrorDataReceived += ( s, ea ) => errorHandler( s, ea );

            bool started = process.Start();
            if (!started)
            {
                throw new InvalidOperationException( "Could not start process: " + process );
            }

            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            return tcs.Task;
        }
        
