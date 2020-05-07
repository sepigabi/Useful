public static string ToSafeFileName(this string filename )
        {
            //based on https://stackoverflow.com/questions/1976007/what-characters-are-forbidden-in-windows-and-linux-directory-names
            var originalFileName = filename;
 
            //replace invalid printable ASCII characters:
            var invalidPrintableCharacters = new[] { '<', '>', ':', '"', '/', '\\', '|', '?', '*' };
            invalidPrintableCharacters.ForEach( c => filename = filename.Replace( c, '_' ) );

            //replace Non-printable characters:
            char[] invalidNonPrintableCharacters = new char[ 32 ];
            for (int i = 0; i < 32; i++)
            {
                invalidNonPrintableCharacters[ i ] = (char) i;
            }
            invalidNonPrintableCharacters.ForEach( c => filename = filename.Replace( c, '_' ) );

            //replace reservedFileNames:
            var reservedFileNames = new[] { "CON", "PRN", "AUX", "NUL", "COM1", "COM2", "COM3", "COM4", "COM5", "COM6", "COM7", "COM8", "COM9", "LPT1", "LPT2", "LPT3", "LPT4", "LPT5", "LPT6", "LPT7", "LPT8", "LPT9" };
            var splittedFileName = filename.Split( '.' );

            while(reservedFileNames.Contains( splittedFileName[ 0 ] ))
            {
                if (splittedFileName.Length == 1)
                {
                    filename = Guid.NewGuid().ToString();
                }
                else if (splittedFileName.Length == 2)
                {
                    filename = Guid.NewGuid().ToString() + '.' + splittedFileName[ 1 ];
                }
                else
                {
                    filename = filename.Remove( 0, splittedFileName[ 0 ].Length + 1 );
                }
                splittedFileName = filename.Split( '.' );
            }

            //normalizeFileEndings:
            var invalidFileEndings = new[] { ' ', '.' };
            invalidFileEndings.ForEach( c => { if (filename[ filename.Length - 1 ] == c) filename = filename.Remove( filename.Length - 1 ); } );

            logger.Debug( $"SafeFileName from {originalFileName} to {filename}" );
            return filename;
        }
