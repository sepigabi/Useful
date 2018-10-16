internal void Call()
        {
            var requestUri = BuildRequestUri();
            if (string.IsNullOrEmpty(requestUri.ToString()))
            {
                return;
            }

            HttpClientHandler handler = new HttpClientHandler();
            handler.UseDefaultCredentials = true;
            HttpClient client = new HttpClient(handler);
            client.DefaultRequestHeaders.Add("HeaderName", "HeaderValue");
            //string body = "{'key':'value'}";
            //HttpContent content = new StringContent(body, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = client.GetAsync(requestUri).Result;
                //HttpResponseMessage response = client.PostAsync(requestUri, content).Result;
                string result = response.Content.ReadAsStringAsync().Result;
                object resultObject = Newtonsoft.Json.JsonConvert.DeserializeObject(result);
            }
            catch (Exception e)
            {
                Trace.WriteLine(new Logger(e.ToEntireErrorMessage(), 3, "HttpClient"));
                throw;
            }
        }

        private Uri BuildRequestUri()
        {
            var baseAddress = ConfigurationManager.AppSettings["baseAddress"];
            if (!Int32.TryParse(ConfigurationManager.AppSettings["timeWindowDays"], out int timeWindow) || string.IsNullOrEmpty(baseAddress))
            {
                return null;
            }
            var builder = new UriBuilder(baseAddress);
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["DateFrom"] = DateTime.Now.AddDays(-timeWindow).ToString("yyyy-MM-dd");
            query["DateTo"] = DateTime.Now.ToString("yyyy-MM-dd");
            builder.Query = query.ToString();
            return builder.Uri;
        }
