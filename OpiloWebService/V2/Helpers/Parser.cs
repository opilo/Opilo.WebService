using Newtonsoft.Json.Linq;
using OpiloWebService.Request;
using OpiloWebService.Response;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace OpiloWebService.V2.Helpers
{
    class Parser
    {
        public static string getRawResponseBody(HttpWebResponse response)
        {
            HttpStatusCode statusCode = response.StatusCode;
            Stream sResponse = response.GetResponseStream();
            StreamReader srResponse = new StreamReader(sResponse);
            string rawResponse = srResponse.ReadToEnd();

            if (statusCode != HttpStatusCode.OK)
            {
                throw CommunicationException.createFromHTTPResponse(statusCode, rawResponse);
            }

            return rawResponse;
        }

        public static Credit prepareCredit(HttpWebResponse response)
        {
            string rawResponse = getRawResponseBody(response);
            try
            {
                JObject jResponse = JObject.Parse(rawResponse);
                if (jResponse["sms_page_count"] == null)
                    throw new CommunicationException(string.Format("Unprocessable Response: {0}", rawResponse), CommunicationException.UNPROCESSABLE_RESPONSE);
                return new Credit(int.Parse(jResponse["sms_page_count"].ToString()));
            }
            catch
            {
                throw new CommunicationException(string.Format("Unprocessable Response: {0}", rawResponse), CommunicationException.UNPROCESSABLE_RESPONSE);
            }
        }

        public static Inbox prepareIncomingSMS(HttpWebResponse response)
        {
            string rawResponse = getRawResponseBody(response);
            try
            {
                JObject jResponse = JObject.Parse(rawResponse);
                JArray jMsgs = (JArray)jResponse["messages"];
                List<IncomingSMS> prepared = new List<IncomingSMS>();
                for (int i = 0; i < jMsgs.Count; i++)
                {
                    JObject jMsg = (JObject)jMsgs[i];
                    IncomingSMS sms = new IncomingSMS(jMsg["from"].ToString(), jMsg["to"].ToString(), jMsg["text"].ToString(),
                        int.Parse(jMsg["id"].ToString()), DateTime.Parse(jMsg["received_at"].ToString()));
                    prepared.Add(sms);
                }
                return new Inbox(prepared);
            }
            catch
            {
                throw new CommunicationException(string.Format("Unprocessable Response: {0}", rawResponse), CommunicationException.UNPROCESSABLE_RESPONSE);
            }
        }

        protected static CheckStatusResponse makeStatusArray(string rawResponse)
        {
            try
            {
                JObject jResponse = JObject.Parse(rawResponse);
                JArray jStatusArray = (JArray)jResponse["status_array"];
                List<Status> prepared = new List<Status>();

                for (int i = 0; i < jStatusArray.Count; i++)
                {
                    Status status = new Status(int.Parse(jStatusArray[i].ToString()));
                    prepared.Add(status);
                }
                return new CheckStatusResponse(prepared);
            }
            catch
            {
                throw new CommunicationException(string.Format("Unprocessable Response: {0}", rawResponse), CommunicationException.UNPROCESSABLE_RESPONSE);
            }
        }

        public static CheckStatusResponse prepareStatusArray(HttpWebResponse response)
        {
            string rawResponse = getRawResponseBody(response);
            return makeStatusArray(rawResponse);
        }

        public static List<SendSMSResponse> prepareSendResponse(HttpWebResponse response)
        {
            string rawResponse = getRawResponseBody(response);
            return makeSendResponseArray(rawResponse);
        }

        protected static List<SendSMSResponse> makeSendResponseArray(string rawResponse)
        {
            try
            {
                JObject jResponse = JObject.Parse(rawResponse);
                JArray jArray = (JArray)jResponse["messages"];
                List<SendSMSResponse> prepared = new List<SendSMSResponse>();

                for (int i = 0; i < jArray.Count; i++)
                {
                    JObject jItem = (JObject)jArray[i];
                    if (jItem["error"] != null)
                        prepared.Add(new SendError(int.Parse(jItem["error"].ToString())));
                    else if (jItem["id"] != null)
                    {
                        bool isDuplicated = false;
                        if(jItem["duplicate"] != null)
                            isDuplicated = (bool) jItem["duplicate"];
                        
                        prepared.Add(new SMSId(jItem["id"].ToString(), isDuplicated));
                    }
                    else
                        throw new CommunicationException(string.Format("Unprocessable Response: {0}", rawResponse), CommunicationException.UNPROCESSABLE_RESPONSE);
                }
                return prepared;
            }
            catch
            {
                throw new CommunicationException(string.Format("Unprocessable Response: {0}", rawResponse), CommunicationException.UNPROCESSABLE_RESPONSE);
            }
        }
    }
}
