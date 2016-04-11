using System;
using System.Collections.Generic;
using System.Text;

namespace OpiloWebService.Response
{
    public class Credit
    {
        private int smsPageCount;

        public Credit(int smsPageCount)
        {
            this.smsPageCount = smsPageCount;
        }

        public int SmsPageCount
        {
            get
            {
                return this.smsPageCount;
            }
        }
    }
}
