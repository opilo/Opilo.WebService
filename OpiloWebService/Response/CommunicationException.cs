using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace OpiloWebService.Response
{
    public class CommunicationException : CustomException
    {
        public const int GENERAL_HTTP_ERROR = 1;

        public const int AUTH_ERROR = 2;

        public const int INVALID_INPUT = 3;

        public const int FORBIDDEN = 4;

        public const int UNPROCESSABLE_RESPONSE = 10;

        public const int UNPROCESSABLE_RESPONSE_ITEM = 11;

        public const int DOWN_FOR_MAINTENANCE = 12;

        public CommunicationException(int code)
            : base(code)
        {
        }

        public CommunicationException(string message, int code)
            : base(message, code)
        {
        }

        public CommunicationException(string message, Exception innerException, int code)
            : base(message, innerException, code)
        {
        }

        public static CommunicationException createFromHTTPResponse(HttpStatusCode statusCode, string bodyContent)
        {
            switch (statusCode)
            {
                case HttpStatusCode.Unauthorized:
                    return new CommunicationException("Authentication Failed", AUTH_ERROR);
                case HttpStatusCode.Forbidden:
                    return new CommunicationException("Forbidden [Web-service is disabled]", FORBIDDEN);
                case HttpStatusCode.ServiceUnavailable:
                    return new CommunicationException("Server is down for maintenance. Retry after a few seconds", DOWN_FOR_MAINTENANCE);
                case (HttpStatusCode)422:
                    return new ValidationException("Input Validation Failed", INVALID_INPUT, bodyContent);
                default:
                    return new CommunicationException(string.Format("StatusCode: {0}, Contents: {1}", statusCode.ToString(), bodyContent), GENERAL_HTTP_ERROR);
            }
        }
    }
}
