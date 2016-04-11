using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace OpiloWebService
{
    public class HttpClient
    {
        private string baseUri;

        public HttpClient(string baseUri)
        {
            this.baseUri = baseUri;
        }

        public HttpWebRequest createRequest(string method, string path, string bodyContent = "", string queryString = "", string contentType = "application/json")
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(this.baseUri + path + queryString);
            request.Method = method;
            request.ContentType = contentType;
            if (bodyContent.Length > 0)
            {
                byte[] buffer = Encoding.UTF8.GetBytes(bodyContent);
                request.ContentLength = buffer.Length;
                Stream PostData = request.GetRequestStream();
                PostData.Write(buffer, 0, buffer.Length);
                PostData.Close();
            }

            return request;
        }

        public HttpWebResponse send(HttpWebRequest request)
        {
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            return response;
        }
    }
}
