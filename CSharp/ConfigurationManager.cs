public class ConfigurationManager
    {
        private static readonly Object sync;
        private static volatile ConfigurationManager instance;

        static ConfigurationManager()
        {
            sync = new Object();
        }
        private ConfigurationManager()
        {
        }

        internal static ConfigurationManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (sync)
                    {
                        if (instance == null)
                        {
                            instance = new ConfigurationManager();
                        }
                    }
                }
                return instance;
            }
        }

        internal T GetConfigurationValue<T>( String configurationName )
        {
            String configurationValue = GetConfigurationValue( configurationName );
            var typeConverter = TypeDescriptor.GetConverter( typeof( T ) );
            return (T) typeConverter.ConvertFrom( configurationValue );
        }

        private String GetConfigurationValue( String name )
        {
            //TODO Implement
        }
    }
