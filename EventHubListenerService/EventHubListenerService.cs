using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;

namespace EventHubListenerService
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    internal sealed class EventHubListenerService : StatelessService
    {
        private static string eventHubConnectionString = "{eventhubconnectionstring}";
        private static string eventHubName = "{eventhubname}";
        private static string storageAccountName = "{storageAccountNameForState}";
        private static string storageAccountKey = "{storageAccountKeyForState}";
        private static string storageConnectionString = string.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}", storageAccountName, storageAccountKey);
        private static string eventProcessorHostName = Guid.NewGuid().ToString();
        EventHubEventListener eventListener;

        public EventHubListenerService(StatelessServiceContext context)
            : base(context)
        { }

        private ICommunicationListener CreateEventHubListener()
        {
            return new EventHubEventListener(eventProcessorHostName, eventHubName, eventHubConnectionString, storageConnectionString);
        }

        /// <summary>
        /// Optional override to create listeners (e.g., TCP, HTTP) for this service replica to handle client or user requests.
        /// </summary>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return new ServiceInstanceListener[]
            {
                new ServiceInstanceListener(context => CreateEventHubListener())
            };
        }

        /// <summary>
        /// This is the main entry point for your service instance.
        /// </summary>
        /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service instance.</param>
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            await Task.Delay(0);
        }
    }
}
