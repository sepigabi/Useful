 
 private static List<List<byte>> acceptableImgFileHeaders;

        static DocumentHandler()
        {
            List<byte> jpg = new List<byte> { 0xFF, 0xD8 };
            List<byte> bmp = new List<byte> { 0x42, 0x4D };
            List<byte> gif = new List<byte> { 0x47, 0x49, 0x46 };
            List<byte> png = new List<byte> { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };
            acceptableImgFileHeaders = new List<List<byte>> { jpg, bmp, gif, png };
        }


private bool IsImage( Stream stream )
        {
            stream.Seek( 0, SeekOrigin.Begin );
            List<byte> bytesIterated = new List<byte>();

            for (int i = 0; i < 8; i++)
            {
                int bit = stream.ReadByte();
                if (bit < 0)
                {
                    break;
                }
                
                bytesIterated.Add( (byte)bit );

                bool isImage = acceptableImgFileHeaders.Any( img => !img.Except( bytesIterated ).Any() );
                if (isImage)
                {
                    stream.Seek( 0, SeekOrigin.Begin );
                    return true;
                }
            }

            return false;
        }



//-----------------------------------------------------------------------------------------------------------------------------------
Lehet így is: 
//------------------------------------------------------------------------------------------------------------------------------------





private bool IsImage( Stream stream )
        {
            stream.Seek( 0, SeekOrigin.Begin );

            List<string> jpg = new List<string> { "FF", "D8" };
            List<string> bmp = new List<string> { "42", "4D" };
            List<string> gif = new List<string> { "47", "49", "46" };
            List<string> png = new List<string> { "89", "50", "4E", "47", "0D", "0A", "1A", "0A" };
            List<List<string>> imgTypes = new List<List<string>> { jpg, bmp, gif, png };

            List<string> bytesIterated = new List<string>();

            for (int i = 0; i < 8; i++)
            {
                string bit = stream.ReadByte().ToString( "X2" );
                bytesIterated.Add( bit );

                bool isImage = imgTypes.Any( img => !img.Except( bytesIterated ).Any() );
                if (isImage)
                {
                    stream.Seek( 0, SeekOrigin.Begin );
                    return true;
                }
            }

            return false;
        }
