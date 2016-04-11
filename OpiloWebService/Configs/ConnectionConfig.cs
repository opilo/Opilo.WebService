using System;
using System.Collections.Generic;
using System.Text;

namespace OpiloWebService.Configs
{
    public class ConnectionConfig
    {
        /// <summary>
        /// Server Base URI, e.g. https://bpanel.opilo.com
        /// </summary>
        private string serverBaseUrl;

        public ConnectionConfig(string serverBaseUrl)
        {
            this.serverBaseUrl = serverBaseUrl;
        }

        public HttpClient getHttpClient()
        {
            return new HttpClient(this.serverBaseUrl + "/ws/api/v2/");
        }
    }
}
