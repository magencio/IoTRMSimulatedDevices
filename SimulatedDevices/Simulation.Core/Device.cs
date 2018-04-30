using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Shared;
using Newtonsoft.Json;
using Simulation.Core.Helpers;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Simulation.Core
{
    public abstract class Device
    {
        public string Id { get; }

        public string PrimaryKey { get; }

        public string JsonSchemaFile { get; }

        public string Schema { get; }

        public string IoTHub { get; }

        private DeviceClient Client { get; set; }

        public Device(string id, string primaryKey, string schema, string jsonSchemaFile, string iotHub)
        {
            Id = id;
            PrimaryKey = primaryKey;
            Schema = schema;
            JsonSchemaFile = jsonSchemaFile;
            IoTHub = iotHub;
        }

        public virtual async Task ConnectAsync()
        {
            Console.WriteLine($"Connecting to {Id} with schema {Schema}...");

            var authMethod = new DeviceAuthenticationWithRegistrySymmetricKey(Id, PrimaryKey);
            var deviceConnectionString = IotHubConnectionStringBuilder.Create(IoTHub, authMethod).ToString();
            Client = DeviceClient.CreateFromConnectionString(deviceConnectionString, TransportType.Amqp);
            var schema = FileHelper.ReadFile(JsonSchemaFile);
            var reportedProperties = new TwinCollection(schema);
            await Client.UpdateReportedPropertiesAsync(reportedProperties);

            await SendMessageAsync("", "");
        }

        protected Task SetMethodHandlerAsync(string methodName, MethodCallback methodCallback, object userContext)
            => Client.SetMethodHandlerAsync(methodName, methodCallback, userContext);

        public Task SendMessageAsync<T>(T telemetry)
             => SendMessageAsync(JsonConvert.SerializeObject(telemetry), Schema);

        private async Task SendMessageAsync(string messageJson, string schema)
        {
            await AzureRetryHelper.OperationWithBasicRetryAsync(async () =>
            {
                var message = new Message(Encoding.UTF8.GetBytes(messageJson));
                message.Properties.Add("$$CreationTimeUtc", DateTime.UtcNow.ToString());
                message.Properties.Add("$$MessageSchema", schema);
                message.Properties.Add("$$ContentType", "JSON");
                await Client.SendEventAsync(message);

                Console.WriteLine($"{Id}.{nameof(SendMessageAsync)}: {messageJson}");
            });
        }
    }
}
