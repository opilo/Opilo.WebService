using System;
using System.Collections.Generic;
using System.Text;

namespace OpiloWebService.Configs
{
    public class Account
    {
        private string userName;

        private string password;

        public Account(string userName, string password)
        {
            this.userName = userName;
            this.password = password;
        }

        public string UserName {
            get
            {
                return this.userName;
            }
        }

        public string Password {
            get
            {
                return this.password;
            }
        }
    }
}
