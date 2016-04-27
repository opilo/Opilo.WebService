# Opilo .NET Web service  client

In order to send and receive SMS via opilo.com panel, you should first create an instance object of class OpiloWebService.V2.OpiloClient.
For that, first you need to configure your webservice in [the configuration page](http://bpanel.opilo.com/api).
## Create a Client Object
```c#
using OpiloWebService.Request;
using OpiloWebService.Response;
using OpiloWebService.V2;
...
OpiloClient client = new OpiloClient("YOUR_WEBSERVICE_USERNAME", "YOUR_WEBSERVICE_PASSWORD");
```
## Sending SMS
### Sending a Single SMS
```c#
OutgoingSMS message = new OutgoingSMS("3000****", "0912*******", "Hello World!");
SendSMSResponse response = client.sendSMS(message);
```
### Sending a Batch of SMS at Once

```c#
List<OutgoingSMS> messages = new List<OutgoingSMS>();
messages.Add(new OutgoingSMS("3000****", "0912*******", "Hello World!"));
messages.Add(new OutgoingSMS("3000****", "0912*******", "Hello World!"));
List<SendSMSResponse> responses = client.sendSMS(messages);
```
### User defined ids
In case of network errors, you may resend your SMS to be sure it is delivered to the Opilo server, but you don't want it to be sent to the target more than once.
To prevent duplicate SMSes you can set unique strings as uid fields of the `OutgoingSMS` objects.
In case of receiving a SMS with a duplicate uid, the Opilo server drops that SMS and return an SMSId object with a boolean `duplicate` flag.
The duplication of a `uid` is checked only against the messages sent during the last 24 hours.

```c#
string someUniqueIdentifierForThisSms = ...;
message = new OutgoingSMS("3000****", "0912*******", "Dont send this twice!", someUniqueIdentifierForThisSms);
```
### Parsing The Return Value of sendSMS()
```c#
for (int i = 0; i < responses.Count; i++)
{
	if (responses[i] is SMSId)
	{
		//store ((SMSId)responses[i]).Id as the id of messages[i] in your database and schedule for checking status if needed
	}
	else
	{
		//It could be that you run out of credit, the line number is invalid, or the receiver number is invalid.
        //To find out more examine ((SendError)responses[i]).Error and compare it against constants in SendError class
	}
}
```

## Check the Inbox by Pagination
```c#
int minId = 0;
Inbox inbox;
while (true)
{
    inbox = client.checkInbox(minId);
	List<IncomingSMS> messages = inbox.Messages;
    if (messages.Count > 0)
	{
        foreach (var message in messages)
		{
            //Process message.OpiloId, message.From, message.To, message.Text, and message.ReceivedAt and store them in your database
            minId = Math.Max(minId, message.OpiloId + 1);
        }
    }
	else
	{
        //no new SMS
        //Store minId in your database for later use of this while loop! You don't need to start from 0 tomorrow!
        break;
    }
}
```

## Checking the Delivery Status of Sent Messages
```c#
//one sms:
int opiloId = yourOpiloIdOfMessageSentViaSendSMSFunction;
Status status = client.checkStatus(opiloId);

//multiple sms:
List<int> opiloIds = yourDatabaseRepository.getArrayOfOpiloIdsOfMessagesSentViaSendSMSFunction();
CheckStatusResponse response = client.checkStatus(opiloIds);
List<Status> statusArray = CheckStatusResponse.StatusArray;
for (int i = 0; i < statusArray.Count; i++)
{
    //process and store the status code statusArray[i].Code for the SMS with Id opiloIds[i]
    //Take a look at constants in OpiloWebService.Response.Status class and their meanings
}
```

## Getting Your SMS Credit
```c#
int numberOfSMSYouCanSendBeforeNeedToCharge = client.getCredit().SmsPageCount;
```

## Exception Handling
All the functions in OpiloClient may throw CommunicationException if the credentials or configurations are invalid, or if there is a network or server error.
Prepare to catch the exceptions appropriately.

```c#
using OpiloWebService.Response;
...
try
{
    ...
    client.sendSMS(...);
    ...
}
catch (CommunicationException e)
{
    //process the exception by comparing e.Code against constants defined in CommunicationException class.
}
```