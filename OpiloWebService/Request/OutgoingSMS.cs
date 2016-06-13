using System;
using System.Collections.Generic;
using System.Text;

namespace OpiloWebService.Request
{
    public class OutgoingSMS : SMS
    {
        private string userDefinedId;

        private DateTime? sendAt;

        public OutgoingSMS(string from, string to, string text, string userDefinedId = null, DateTime? sendAt = null)
            : base(from, to, text)
        {
            this.userDefinedId = userDefinedId;
            this.sendAt = sendAt;
        }

        public string UserDefinedId
        {
            get
            {
                return this.userDefinedId;
            }
        }

        public DateTime? SendAt
        {
            get
            {
                return this.sendAt;
            }
        }

        public string formatSendAt
        {
            get
            {
                if (this.sendAt != null)
                {
                    return this.sendAt.Value.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss");
                }
                return null;
            }
        }
    }
}
