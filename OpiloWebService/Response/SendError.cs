using System;
using System.Collections.Generic;
using System.Text;

namespace OpiloWebService.Response
{
    public class SendError : SendSMSResponse
    {
        public const int ERROR_RESOURCE_NOT_FOUND = 5;

        public const int ERROR_INVALID_DESTINATION = 6;

        public const int ERROR_OUT_OF_CREDIT = 7;

        public const int ERROR_GENERAL = 8;

        private int error;

        public SendError(int error)
        {
            this.error = error;
        }

        public int Error
        {
            get
            {
                return this.error;
            }
        }
    }
}
