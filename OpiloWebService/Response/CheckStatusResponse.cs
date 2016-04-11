using System;
using System.Collections.Generic;
using System.Text;

namespace OpiloWebService.Response
{
    public class CheckStatusResponse
    {
        private List<Status> statusArray;

        public CheckStatusResponse(List<Status> statusArray)
        {
            this.statusArray = statusArray;
        }

        public List<Status> StatusArray
        {
            get
            {
                return this.statusArray;
            }
        }
    }
}
