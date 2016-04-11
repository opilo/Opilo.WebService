using System;
using System.Collections.Generic;
using System.Text;

namespace OpiloWebService.Request
{
    public class IncomingSMS : SMS
    {
        private DateTime receivedAt;

        private int opiloId;

        public IncomingSMS(string from, string to, string text, int opiloId, DateTime receivedAt)
            : base(from, to, text)
        {
            this.receivedAt = receivedAt;
            this.opiloId = opiloId;
        }

        public int OpiloId
        {
            get
            {
                return this.opiloId;
            }
        }

        public DateTime ReceivedAt
        {
            get
            {
                return this.receivedAt;
            }
        }
    }
}
