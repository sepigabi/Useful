public string GetEntireErrorMessageFromException(Exception e)
        {
            Exception exception = e;
            StringBuilder errorMessage = new StringBuilder();
            errorMessage.Append($"Message: {e.Message} ");
            while (exception.InnerException != null)
            {
                errorMessage.Append($"\nInnerException message: {exception.InnerException.Message} ");
                exception = exception.InnerException;
            }
            errorMessage.Append($"\nStacktrace: {e.StackTrace}");
            return errorMessage.ToString();
        }
