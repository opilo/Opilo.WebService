using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpiloWebService.Response
{
    public class ValidationException : CommunicationException
    {
        private string httpResponseBody = "";

        private List<string> errors = new List<string>();

        public ValidationException(int code, string httpResponseBody)
            : base(code)
        {
            this.httpResponseBody = httpResponseBody;
            extractErrors();
        }

        public ValidationException(string message, int code, string httpResponseBody)
            : base(message,code)
        {
            this.httpResponseBody = httpResponseBody;
            extractErrors();
        }

        public ValidationException(string message, Exception innerException, int code, string httpResponseBody)
            : base(message, innerException, code)
        {
            this.httpResponseBody = httpResponseBody;
            extractErrors();
        }

        private void extractErrors()
        {
            try
            {
                JArray jArray = JArray.Parse(this.httpResponseBody);
                if( jArray["errors"] != null )
                {
                    JArray jErrors = (JArray)jArray["errors"];
                    for (int i = 0; i < jErrors.Count; i++)
                        this.errors.Add(jErrors[i].ToString());
                }
            }
            catch (Exception ex) { }
            
        }

        public string HttpResponseBody
        {
            get
            {
                return this.httpResponseBody;
            }
        }

        public List<string> Errors
        {
            get
            {
                return this.errors;
            }
        }
    }
}
