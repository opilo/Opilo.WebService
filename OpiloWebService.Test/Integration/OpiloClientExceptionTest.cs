using NUnit.Framework;
using OpiloWebService.Configs;
using OpiloWebService.Request;
using OpiloWebService.Response;
using OpiloWebService.V2;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace OpiloWebService.Test.Integration
{
    [TestFixture]
    class OpiloClientExceptionTest
    {
        private string OPILO_URL = ConfigurationSettings.AppSettings["OPILO_URL"];
        private string OPILO_USERNAME = ConfigurationSettings.AppSettings["OPILO_USERNAME"];
        private string OPILO_PASSWORD = ConfigurationSettings.AppSettings["OPILO_PASSWORD"];
        private string PANEL_LINE = ConfigurationSettings.AppSettings["PANEL_LINE"];
        private string DESTINATION = ConfigurationSettings.AppSettings["DESTINATION"];
        private string OPILO_USERNAME_WS_DISABLED = ConfigurationSettings.AppSettings["OPILO_USERNAME_WS_DISABLED"];
        private string OPILO_PASSWORD_WS_DISABLED = ConfigurationSettings.AppSettings["OPILO_PASSWORD_WS_DISABLED"];

        [Test]
        public void test401()
        {
            ConnectionConfig config = new ConnectionConfig(OPILO_URL);
            Account account = new Account(OPILO_USERNAME, "wrong_password");
            OpiloClient client = new OpiloClient(config, account);
            try
            {
                client.getCredit();
            }
            catch (CommunicationException ex)
            {
                Assert.AreEqual("Authentication Failed", ex.Message);
                Assert.AreEqual(CommunicationException.AUTH_ERROR, ex.Code);
            }
        }

        [Test]
        public void test403()
        {
            ConnectionConfig config = new ConnectionConfig(OPILO_URL);
            Account account = new Account(OPILO_USERNAME_WS_DISABLED, OPILO_PASSWORD_WS_DISABLED);
            OpiloClient client = new OpiloClient(config, account);
            try
            {
                client.getCredit();
            }
            catch (CommunicationException ex)
            {
                Assert.AreEqual("Forbidden [Web-service is disabled]", ex.Message);
                Assert.AreEqual(CommunicationException.FORBIDDEN, ex.Code);
            }
        }

        [Test]
        public void test422()
        {
            ConnectionConfig config = new ConnectionConfig(OPILO_URL);
            Account account = new Account(OPILO_USERNAME, OPILO_PASSWORD);
            OpiloClient client = new OpiloClient(config, account);
            bool failed = false;
            try
            {
                client.checkStatus(new List<int>() { -1 });
            }
            catch (CommunicationException ex)
            {
                failed = true;
                Assert.AreEqual("Input Validation Failed", ex.Message);
                Assert.AreEqual(CommunicationException.INVALID_INPUT, ex.Code);
            }
            Assert.IsTrue(failed);
        }

        [Test]
        public void test422Errors()
        {
            ConnectionConfig config = new ConnectionConfig(OPILO_URL);
            Account account = new Account(OPILO_USERNAME, OPILO_PASSWORD);
            OpiloClient client = new OpiloClient(config, account);
            bool failed = false;
            try
            {
                client.checkStatus(new List<int>() { 0 });
            }
            catch (ValidationException ex)
            {
                failed = true;
                List<Dictionary<string, string>> errors = ex.Errors;
                Assert.AreEqual(1, errors.Count);
                Assert.AreEqual("1", errors[0]["Min"]);
            }
            Assert.IsTrue(failed);
        }

        [Test]
        public void testSendInvalidSMS()
        {
            ConnectionConfig config = new ConnectionConfig(OPILO_URL);
            Account account = new Account(OPILO_USERNAME, OPILO_PASSWORD);
            OpiloClient client = new OpiloClient(config, account);
            List<OutgoingSMS> messages = new List<OutgoingSMS>();
            messages.Add(new OutgoingSMS("abcd", DESTINATION, "invalid from"));
            messages.Add(new OutgoingSMS(PANEL_LINE, "abcd", "invalid to"));
            messages.Add(new OutgoingSMS("3000", DESTINATION, "unauthorized from"));
            messages.Add(new OutgoingSMS(PANEL_LINE, DESTINATION, "authorized from"));

            List<SendSMSResponse> response = client.sendSMS(messages);
            Assert.AreEqual(4, response.Count);
            Assert.IsInstanceOf<SendError>(response[0]);
            Assert.IsInstanceOf<SendError>(response[1]);
            Assert.IsInstanceOf<SendError>(response[2]);
            Assert.IsInstanceOf<SMSId>(response[3]);
            Assert.AreEqual(SendError.ERROR_RESOURCE_NOT_FOUND, ((SendError)response[0]).Error);
            Assert.AreEqual(SendError.ERROR_INVALID_DESTINATION, ((SendError)response[1]).Error);
            Assert.AreEqual(SendError.ERROR_RESOURCE_NOT_FOUND, ((SendError)response[2]).Error);
        }
    }
}
