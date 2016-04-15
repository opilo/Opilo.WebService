using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpiloWebService.Response
{
    public class ValidationException : CommunicationException
    {
        private string httpResponseBody = "";

        private List<Dictionary<string, string>> errors = new List<Dictionary<string, string>>();

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
                JObject jResponse = JObject.Parse(this.httpResponseBody);
                if (jResponse["errors"] != null)
                {
                    JObject jErrors = (JObject)jResponse["errors"];
                    for (int i = 0; i < jErrors.Count; i++) {
                        JObject jError = (JObject)jErrors["ids." + i.ToString()];
                        Dictionary<string,string> dic = new Dictionary<string,string>();
                        if (jError != null && jError["Integer"] != null)
                            dic.Add("Integer", "");
                        if (jError != null && jError["Min"] != null)
                            dic.Add("Min", ((JArray)jError["Min"])[0].ToString());
                        errors.Add(dic);
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

        public List<Dictionary<string, string>> Errors
        {
            get
            {
                return this.errors;
            }
        }
    }
}
