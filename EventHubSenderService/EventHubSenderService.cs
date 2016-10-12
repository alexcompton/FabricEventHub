using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Microsoft.ServiceBus.Messaging;
using System.Text;
using System.Diagnostics;

namespace EventHubSenderService
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    internal sealed class EventHubSenderService : StatelessService
    {
        static string eventHubName = "{eventhubname}";
        static string connectionString = "{connectionstring}";

        public EventHubSenderService(StatelessServiceContext context)
            : base(context)
        { }

        /// <summary>
        /// Optional override to create listeners (e.g., TCP, HTTP) for this service replica to handle client or user requests.
        /// </summary>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return new ServiceInstanceListener[0];
        }

        /// <summary>
        /// This is the main entry point for your service instance.
        /// </summary>
        /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service instance.</param>
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            var eventHubClient = EventHubClient.CreateFromConnectionString(connectionString, eventHubName);
            int iterations = 0;

            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();
                List<EventData> dataList = new List<EventData>();

                for (int i = 0; i < 2000; i++)
                {
                    iterations++;
                    var message = Guid.NewGuid().ToString();
                    EventData eventData = new EventData(Encoding.UTF8.GetBytes(iterations + " || " + message));
                    dataList.Add(eventData);
                }

                try
                {
                    Trace.TraceInformation("{0} > Sending message batch: {1}", DateTime.Now, iterations);
                    await eventHubClient.SendBatchAsync(dataList);
                }
                catch (Exception exception)
                {
                    Trace.TraceError("{0} > Message Exception Batch Id: {1}\n\tException: {2}", DateTime.Now, iterations, exception.Message);
                }

                await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            }
        }
    }
}
