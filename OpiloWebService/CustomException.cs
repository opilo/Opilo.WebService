using System;
using System.Collections.Generic;
using System.Text;

namespace OpiloWebService
{
    public class CustomException : Exception
    {
        protected int code = 0;

        public CustomException(int code)
            : base()
        {
            this.code = code;
        }

        public CustomException(string message, int code)
            : base(message)
        {
            this.code = code;
        }

        public CustomException(string message, Exception innerException, int code)
            : base(message, innerException)
        {
            this.code = code;
        }

        public int Code
        {
            get
            {
                return this.code;
            }
        }
    }
}
