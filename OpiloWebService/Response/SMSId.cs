using System;
using System.Collections.Generic;
using System.Text;

namespace OpiloWebService.Response
{
    public class SMSId : SendSMSResponse
    {
        private string id;

        private bool duplicate;

        public SMSId(string id, bool duplicate = false)
        {
            this.id = id;
            this.duplicate = duplicate;
        }

        public string Id
        {
            get
            {
                return this.id;
            }
        }

        public bool IsDuplicated 
        {
            get
            {
                return this.duplicate;
            }
        }
    }
}
