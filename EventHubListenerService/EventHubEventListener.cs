using Microsoft.ServiceFabric.Services.Communication.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using Microsoft.ServiceBus.Messaging;

namespace EventHubListenerService
{
    class EventHubEventListener : ICommunicationListener
    {
        Stopwatch checkpointStopWatch;
        private EventProcessorHost eventProcessorHost;
        private string eventProcessorHostName;
        private string eventHubName;
        private string eventHubConnectionString;
        private string storageConnectionString;

        public EventHubEventListener(string eventProcessorHostName, string eventHubName, string eventHubConnectionString, string storageConnectionString)
        {
            this.eventProcessorHostName = eventProcessorHostName;
            this.eventHubName = eventHubName;
            this.eventHubConnectionString = eventHubConnectionString;
            this.storageConnectionString = storageConnectionString;
        }

        public void Abort()
        {
            eventProcessorHost.UnregisterEventProcessorAsync().Wait();
        }

        public async Task CloseAsync(CancellationToken cancellationToken)
        {
            await eventProcessorHost.UnregisterEventProcessorAsync();
        }

        public async Task<string> OpenAsync(CancellationToken cancellationToken)
        {
            eventProcessorHost = new EventProcessorHost(eventProcessorHostName, eventHubName, EventHubConsumerGroup.DefaultGroupName, eventHubConnectionString, storageConnectionString);
            var options = new EventProcessorOptions();
            options.ExceptionReceived += (sender, e) => { Trace.TraceError(e.Exception.ToString()); };
            await eventProcessorHost.RegisterEventProcessorAsync<EventProcessor>(options);

            return "Recieving Events From Azure...";
        }
    }
}
