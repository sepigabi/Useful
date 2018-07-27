string bodyBase64 = OperationContext.Current.RequestContext.RequestMessage.ToString().Replace("<Binary>", "").Replace("</Binary>", "");
byte[] rawBody = Convert.FromBase64String(bodyBase64);
string bodyString = Encoding.ASCII.GetString(rawBody); //or UTF8?
