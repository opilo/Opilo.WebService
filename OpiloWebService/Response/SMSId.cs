using System;
using System.Collections.Generic;
using System.Text;

namespace OpiloWebService.Response
{
    public class SMSId : SendSMSResponse
    {
        private string id;

        public SMSId(string id)
        {
            this.id = id;
        }

        public string Id
        {
            get
            {
                return this.id;
            }
        }
    }
}
