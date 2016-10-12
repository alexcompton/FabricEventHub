
## Simple demonstration of Event Hub in Microsoft Service Fabric

This project leverages the following:

- Microsoft Service Fabric
- Azure Event Hub
- Azure Storage Account

This is a simplified project version of this tutorial: 
https://azure.microsoft.com/en-us/documentation/articles/event-hubs-csharp-ephcs-getstarted/

This should help you get started if you don't know how to create those accounts on your own.

## Event Hub Sender

Change the following lines of code to your implementation in the EventHubSenderService/EventHubSenderService.cs

```c#
static string eventHubName = "{eventhubname}";
static string connectionString = "{connectionstring}";
```

## Event Hub Listener

Change the following lines of code to your implementation in the EventHubListenerService/EventHubListenerService.cs

```c#
private static string eventHubConnectionString = "{eventhubconnectionstring}";
private static string eventHubName = "{eventhubname}";
private static string storageAccountName = "{storageAccountNameForState}";
private static string storageAccountKey = "{storageAccountKeyForState}";
```