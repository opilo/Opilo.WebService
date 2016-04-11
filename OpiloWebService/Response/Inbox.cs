using OpiloWebService.Request;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpiloWebService.Response
{
    public class Inbox
    {
        public const int PAGE_LIMIT = 90;

        public const string INBOX_READ = "read";

        public const string INBOX_NOT_READ = "not_read";

        public const string INBOX_ALL = "all";

        private List<IncomingSMS> messages;

        public Inbox(List<IncomingSMS> messages)
        {
            this.messages = messages;
        }

        public List<IncomingSMS> Messages
        {
            get
            {
                return this.messages;
            }
        }
    }
}
