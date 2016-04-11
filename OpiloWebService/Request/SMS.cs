using System;
using System.Collections.Generic;
using System.Text;

namespace OpiloWebService.Request
{
    public class SMS
    {
        /// <summary>
        /// Sender Number
        /// </summary>
        private string from;

        /// <summary>
        /// Receiver Number
        /// </summary>
        private string to;

        /// <summary>
        /// The content of SMS
        /// </summary>
        private string text;

        public SMS(string from, string to, string text)
        {
            this.from = from;
            this.to = to;
            this.text = text;
        }

        public string From
        {
            get
            {
                return this.from;
            }
        }

        public string To
        {
            get
            {
                return this.to;
            }
        }

        public string Text
        {
            get
            {
                return this.text;
            }
        }
    }
}
