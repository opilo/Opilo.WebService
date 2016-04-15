using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpiloWebService.Response
{
    public class ValidationException : CommunicationException
    {
        private string httpResponseBody = "";

        private Dictionary<string, Dictionary<string, string>> errors = new Dictionary<string, Dictionary<string, string>>();

        public ValidationException(int code, string httpResponseBody)
            : base(code)
        {
            this.httpResponseBody = httpResponseBody;
            extractErrors();
        }

        public ValidationException(string message, int code, string httpResponseBody)
            : base(message, code)
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
                JObject jResponse = JObject.Parse(this.httpResponseBody);
                if (jResponse["errors"] != null)
                {
                    JObject jErrors = (JObject)jResponse["errors"];
                    foreach (JProperty prop in jErrors.Properties())
                    {
                        JObject jError = (JObject)jErrors[prop.Name];
                        Dictionary<string, string> dic = new Dictionary<string, string>();
                        if (jError["Integer"] != null)
                            dic.Add("Integer", "");
                        if (jError["Min"] != null)
                            dic.Add("Min", ((JArray)jError["Min"])[0].ToString());
                        this.errors.Add(prop.Name, dic);
                    }
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

        public Dictionary<string, Dictionary<string, string>> Errors
        {
            get
            {
                return this.errors;
            }
        }
    }
}
