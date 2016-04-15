using Newtonsoft.Json.Linq;
using OpiloWebService.Configs;
using OpiloWebService.Request;
using OpiloWebService.Response;
using OpiloWebService.V2.Helpers;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace OpiloWebService.V2
{
    public class OpiloClient
    {
        protected Account account;

        protected HttpClient client;

        public OpiloClient(string userName, string password, string serverBaseUrl = "http://bpanel.opilo.com")
        {
            ConnectionConfig config = new ConnectionConfig(serverBaseUrl);
            this.account = new Account(userName, password);
            this.client = config.getHttpClient();
        }

        public OpiloClient(ConnectionConfig config, Account account)
        {
            this.account = account;
            this.client = config.getHttpClient();
        }

        public List<SendSMSResponse> sendSMS(List<OutgoingSMS> messages)
        {
            JObject options = Out.SMSArrayToSendRequestBody(messages);
            Out.attachAuth(this.account, options);
            HttpWebRequest request = this.client.createRequest("POST", "sms/send", options.ToString());
            HttpWebResponse response = Out.send(this.client, request);

            return Parser.prepareSendResponse(response);
        }

        public Inbox checkInbox(int minId = 0, DateTime? minReceivedAt = null, string read = Inbox.INBOX_ALL, string lineNumber = null)
        {
            string query = "?";
            query += string.Format("min_id={0}", minId);
            if (minReceivedAt != null)
                query += string.Format("&min_received_at={0}", minReceivedAt.Value.ToString("yyyy-MM-dd HH:mm:ss"));

            if (read != Inbox.INBOX_ALL)
                query += string.Format("&read={0}", read);

            if (lineNumber != null)
                query += string.Format("&line_number={0}", lineNumber);

            query = Out.attachAuth(this.account, query);
            HttpWebRequest request = this.client.createRequest("GET", "inbox", "", query);
            HttpWebResponse response = Out.send(this.client, request);

            return Parser.prepareIncomingSMS(response);
        }

        public CheckStatusResponse checkStatus(List<int> opiloIds)
        {
            string query = "?" + Out.attachAuth(this.account, "");
            opiloIds.ForEach(id => query += string.Format("&ids[]={0}", id));
            HttpWebRequest request = this.client.createRequest("GET", "sms/status", "", query);
            HttpWebResponse response = Out.send(this.client, request);

            return Parser.prepareStatusArray(response);
        }

        public Credit getCredit()
        {
            string query = "?" + Out.attachAuth(this.account, "");
            HttpWebRequest request = this.client.createRequest("GET", "credit", "", query);
            HttpWebResponse response = Out.send(this.client, request);

            return Parser.prepareCredit(response);
        }
    }
}
