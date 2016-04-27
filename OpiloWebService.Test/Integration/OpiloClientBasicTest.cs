using NUnit.Framework;
using OpiloWebService.Configs;
using OpiloWebService.Request;
using OpiloWebService.Response;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace OpiloWebService.Test.Integration
{
    [TestFixture]
    class OpiloClientBasicTest
    {
        private string OPILO_URL = ConfigurationSettings.AppSettings["OPILO_URL"];
        private string OPILO_USERNAME = ConfigurationSettings.AppSettings["OPILO_USERNAME"];
        private string OPILO_PASSWORD = ConfigurationSettings.AppSettings["OPILO_PASSWORD"];
        private string PANEL_LINE = ConfigurationSettings.AppSettings["PANEL_LINE"];
        private string DESTINATION = ConfigurationSettings.AppSettings["DESTINATION"];

        private OpiloWebService.V2.OpiloClient client;

        [SetUp]
        public void setUp()
        {
            ConnectionConfig config = new ConnectionConfig(OPILO_URL);
            Account account = new Account(OPILO_USERNAME, OPILO_PASSWORD);
            this.client = new V2.OpiloClient(config, account);
        }

        [Test]
        public void testGetCredit()
        {
            Credit credit = this.client.getCredit();
            Assert.True(credit.SmsPageCount >= 0);
        }

        [Test]
        public void testSendSingleSMS()
        {
            int initCredit = this.client.getCredit().SmsPageCount;
            OutgoingSMS message = new OutgoingSMS(PANEL_LINE, DESTINATION, "V2::testSendSingleSMS()");
            SendSMSResponse response = client.sendSMS(message);
            Assert.IsInstanceOf<SMSId>(response);
            Status status = this.client.checkStatus(int.Parse(((SMSId)response).Id));
            Assert.IsInstanceOf<Status>(status);
            int finalCredit = this.client.getCredit().SmsPageCount;
            Assert.LessOrEqual(initCredit - finalCredit, 1);
        }

        [Test]
        public void testSendSmsWithDuplicateUidBatch()
        {
            string uid = Guid.NewGuid().ToString();
            List<OutgoingSMS> messages = new List<OutgoingSMS>();
            messages.Add(new OutgoingSMS(PANEL_LINE, DESTINATION, "V2::testSendSmsWithDuplicateUidBatch()", uid));
            messages.Add(new OutgoingSMS(PANEL_LINE, DESTINATION, "V2::testSendSmsWithDuplicateUidBatch()", uid));
            List<SendSMSResponse> result = this.client.sendSMS(messages);
            Assert.IsInstanceOf<SMSId>(result[0]);
            Assert.IsInstanceOf<SMSId>(result[1]);
            Assert.IsFalse(((SMSId)result[0]).IsDuplicated);
            Assert.IsTrue(((SMSId)result[1]).IsDuplicated);
        }

        [Test]
        public void testSendSmsWithDuplicateUidSingle()
        {
            string uid = Guid.NewGuid().ToString();
            OutgoingSMS message = new OutgoingSMS(PANEL_LINE, DESTINATION, "V2::testSendSmsWithDuplicateUidSingle()", uid);
            SendSMSResponse result = this.client.sendSMS(message);
            Assert.IsInstanceOf<SMSId>(result);
            Assert.IsFalse(((SMSId)result).IsDuplicated);

            message = new OutgoingSMS(PANEL_LINE, DESTINATION, "V2::testSendSmsWithDuplicateUidSingle()", uid);
            result = this.client.sendSMS(message);
            Assert.IsInstanceOf<SMSId>(result);
            Assert.IsTrue(((SMSId)result).IsDuplicated);
        }

        [Test]
        public void testSendMultipleSMS()
        {
            int initCredit = this.client.getCredit().SmsPageCount;
            List<OutgoingSMS> messages = new List<OutgoingSMS>();
            for (int i = 0; i < 10; i++)
                messages.Add(new OutgoingSMS(PANEL_LINE, DESTINATION, "V2::testSendMultipleSMS" + string.Format("({0})", i), i.ToString(), DateTime.Now.AddMinutes(i)));

            List<SendSMSResponse> response = client.sendSMS(messages);
            Assert.AreEqual(10, response.Count);
            List<int> ids = new List<int>();
            foreach (var id in response)
            {
                Assert.IsInstanceOf<SMSId>(id);
                ids.Add(int.Parse(((SMSId)id).Id));
            }
            CheckStatusResponse status = this.client.checkStatus(ids);
            Assert.AreEqual(10, status.StatusArray.Count);
            int finalCredit = this.client.getCredit().SmsPageCount;
            Assert.LessOrEqual(initCredit - finalCredit, 10);
        }

        [Test]
        public void testCheckInbox()
        {
            Inbox inbox = this.client.checkInbox(0);
            List<IncomingSMS> messages = inbox.Messages;
            Assert.LessOrEqual(messages.Count, Inbox.PAGE_LIMIT);
        }
    }
}
