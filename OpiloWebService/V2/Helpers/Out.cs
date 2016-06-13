using Newtonsoft.Json.Linq;
using OpiloWebService.Configs;
using OpiloWebService.Request;
using OpiloWebService.Response;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace OpiloWebService.V2.Helpers
{
    class Out
    {
        public static HttpWebResponse send(HttpClient client, HttpWebRequest request)
        {
            try
            {
                return client.send(request);
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    HttpWebResponse response = (HttpWebResponse)ex.Response;
                    HttpStatusCode statusCode = response.StatusCode;
                    Stream sResponse = response.GetResponseStream();
                    StreamReader srResponse = new StreamReader(sResponse);
                    string rawResponse = srResponse.ReadToEnd();

                    throw CommunicationException.createFromHTTPResponse(statusCode, rawResponse);
                }
                throw new CommunicationException("RequestException", ex, CommunicationException.GENERAL_HTTP_ERROR);
            }
        }

        public static void attachAuth(Account account, JObject content)
        {
            content.Add("username", account.UserName);
            content.Add("password", account.Password);
        }

        public static string attachAuth(Account account, string query)
        {
            if (query != "")
            {
                query += string.Format("&username={0}", account.UserName);
                query += string.Format("&password={0}", account.Password);
            }
            else
            {
                query += string.Format("username={0}", account.UserName);
                query += string.Format("&password={0}", account.Password);
            }
            return query;
        }

        public static JObject SMSArrayToSendRequestBody(List<OutgoingSMS> messages)
        {
            JObject result = new JObject();
            JArray msgs = new JArray();

            bool first = true;
            foreach (OutgoingSMS message in messages)
            {
                if (first)
                {
                    first = false;
                    JObject defaults = new JObject();
                    defaults.Add("from", message.From);
                    defaults.Add("text", message.Text);


                    if (message.SendAt != null)
                    {
                        defaults.Add("send_at", message.formatSendAt);
                    }
                    result.Add("defaults", defaults);
                }

                JObject msg = new JObject();
                msg.Add("to", message.To);

                if (message.UserDefinedId != null)
                    msg.Add("uid", message.UserDefinedId);

                if (result["defaults"]["from"].ToString() != message.From)
                    msg.Add("from", message.From);

                if (result["defaults"]["text"].ToString() != message.Text)
                    msg.Add("text", message.Text);

                if (message.SendAt != null &&
                    (result["defaults"]["send_at"] == null || result["defaults"]["send_at"].ToString() != message.formatSendAt))
                {
                    msg.Add("send_at", message.formatSendAt);
                }

                msgs.Add(msg);
            }
            result.Add("messages", msgs);
            return result;
        }
    }
}
