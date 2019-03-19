public class ErrorHandler : HttpClientHandler
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        async protected override Task<HttpResponseMessage> SendAsync( HttpRequestMessage request, CancellationToken cancellationToken )
        {
            HttpResponseMessage response = await base.SendAsync( request, cancellationToken );

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.Found)
                {
                    if (await RestClient.Instance.Authenticate())
                    {
                        var repeatedResponse = await base.SendAsync( request, cancellationToken );
                        if (!repeatedResponse.IsSuccessStatusCode)
                        {
                           await HandleError( repeatedResponse );
                        }
                        return repeatedResponse;
                    }
                }
                else
                {
                    await HandleError( response );
                }
            }
            return response;
        }

        private async Task HandleError( HttpResponseMessage response )
        {
            try
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var requestContent = await response.RequestMessage.Content.ReadAsStringAsync();
                logger.Error( $"RestClient error: Request_uri: {response.RequestMessage?.RequestUri}, Request_content: {requestContent}, Response_status: {response.StatusCode}, Response_reason: {response.ReasonPhrase}, Response_content: {responseContent}!" );
            }
            catch (System.Exception e)
            {
                logger.Error( e, "RestClient error!" );
            }
        }
    }
    
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //USAGE:
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    public class RestClient
    {
        private static readonly Object sync;
        private static volatile RestClient instance;
        private HttpClient httpClient;
        private CookieContainer cookieContainer;
        private IJsonSerializer jsonSerializer;
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        static RestClient()
            {
                sync = new Object();
            }

        public static RestClient Instance
            {
                get
                {
                    if (instance == null)
                    {
                        lock (sync)
                        {
                            if (instance == null)
                            {
                                instance = new RestClient();
                            }
                        }
                    }
                    return instance;
                }
            }

        private RestClient()
            {
                cookieContainer = new CookieContainer();
                HttpClientHandler handler = new ErrorHandler() { CookieContainer = cookieContainer, AllowAutoRedirect = false };
                httpClient = new HttpClient( handler );
                jsonSerializer = new JsonSerializerFactory().Create();
            }

        public async Task<bool> Authenticate()
            {
                var uri = new Uri( Constants.AUTHENTICATIONSERVICE_URL );
                var body = new
                {
                    userNameOrEmail = Constants.RESTSERVICE_USERNAME,
                    password = Constants.RESTSERVICE_PASSWORD
                };
                var json = jsonSerializer.SerializeObject( body );
                var content = new StringContent( json, Encoding.UTF8, "application/json" );
                var response = await httpClient.PostAsync( uri, content );
                IEnumerable<Cookie> responseCookies = cookieContainer.GetCookies( uri ).Cast<Cookie>();
                var resultJson = await response.Content.ReadAsStringAsync();
                var authenticationResult = jsonSerializer.DeserializeObject<AuthenticationWebserviceResult>( resultJson ).D;
                if (authenticationResult != AuthenticationResult.Succeeded)
                {
                    logger.Error( $"RestClient authentication failed! AuthenticationResult: {authenticationResult}" );
                }

                return authenticationResult == AuthenticationResult.Succeeded;
            }

        public async Task<string> Test()
            {
                var uri = new Uri( $"{Constants.RESTSERVICE_BASEURL}WorksheetWebService.ashx/GetWorksheetWorkflowStateAliasById" );
                var json = JsonConvert.SerializeObject( new
                {
                    worksheetId = -1
                } );
                var content = new StringContent( json, Encoding.UTF8, "application/json" );
                var response = await httpClient.PostAsync( uri, content );
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                return null;
            }

        public bool IsOnline()
            {
                try
                {
                    var baseServiceUri = new Uri( Constants.RESTSERVICE_BASEURL );
                    var hostUri = baseServiceUri.Scheme + Uri.SchemeDelimiter + baseServiceUri.Host;
                    HttpWebRequest pingRequest = (HttpWebRequest) WebRequest.Create( hostUri );
                    pingRequest.Timeout = Constants.RESTSERVICE_ISONLINECHECKING_TIMEOUT;
                    using (var pingResponse = pingRequest.GetResponse())
                    {
                        return true;
                    }
                }
                catch (WebException e)
                {
                    if (e.Status == WebExceptionStatus.ProtocolError)
                    {
                        var response = e.Response as HttpWebResponse;
                        if (response?.StatusCode == HttpStatusCode.Unauthorized || response?.StatusCode == HttpStatusCode.Found)
                        {
                            return true;
                        }
                    }
                    return false;
                }
            }

            public async Task<bool> IsOnlineAsync()
            {
                try
                {
                    var baseServiceUri = new Uri( Constants.RESTSERVICE_BASEURL );
                    var hostUri = baseServiceUri.Scheme + Uri.SchemeDelimiter + baseServiceUri.Host;
                    HttpWebRequest pingRequest = (HttpWebRequest) WebRequest.Create( hostUri );
                    pingRequest.Timeout = Constants.RESTSERVICE_ISONLINECHECKING_TIMEOUT;
                    using (var pingResponse = await pingRequest.GetResponseAsync())
                    {
                        return true;
                    }
                }
                catch (WebException e)
                {
                    if (e.Status == WebExceptionStatus.ProtocolError)
                    {
                        var response = e.Response as HttpWebResponse;
                        if (response?.StatusCode == HttpStatusCode.Unauthorized || response?.StatusCode == HttpStatusCode.Found)
                        {
                            return true;
                        }
                    }
                    return false;
                }
            }
    }
    
    
    
