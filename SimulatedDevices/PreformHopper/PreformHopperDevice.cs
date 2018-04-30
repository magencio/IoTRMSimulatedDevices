using Microsoft.Azure.Devices.Client;
using Simulation.Core;
using System;
using System.Threading.Tasks;
using static Simulation.Core.Helpers.RandomHelper;

namespace PreformHopper
{
    public class PreformHopperDevice : Device
    {
        public PreformHopperTelemetry Telemetry { get; private set; }

        public PreformHopperDevice(string id, string primaryKey, string schema, string jsonSchemaFile, string iotHub)
            : base (id, primaryKey, schema, jsonSchemaFile, iotHub)
        {
            Telemetry = new PreformHopperTelemetry()
            {
                online = true,
                running = true,
                producing = true
            };
        }

        public override async Task ConnectAsync()
        {
            await base.ConnectAsync();
            await SetMethodHandlerAsync("Turn On", TurnOnCommandHandler, null);
            await SetMethodHandlerAsync("Turn Off", TurnOffCommandHandler, null);
        }

        public async Task UpdateAsync()
        {
            if (!Telemetry.producing) return;

            Telemetry.counterout = (int)Vary(24, 5, 22, 25);
            Telemetry.rightquality = (int)Vary(24, 15, 0, Telemetry.counterout);

            await SendMessageAsync(Telemetry);
        }

        private async Task<MethodResponse> TurnOnCommandHandler(MethodRequest methodRequest, object userContext)
        {
            try
            {
                Console.WriteLine($"{Id}.{nameof(TurnOnCommandHandler)}...");

                Telemetry = new PreformHopperTelemetry()
                {
                    online = true,
                    running = true,
                    producing = true,
                };

                await SendMessageAsync(Telemetry);

                return new MethodResponse(0);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Exception on {Id}.{nameof(TurnOnCommandHandler)}: {ex.Message}");
                return new MethodResponse(-1);
            }
        }

        private async Task<MethodResponse> TurnOffCommandHandler(MethodRequest methodRequest, object userContext)
        {
            try
            {
                Console.WriteLine($"{Id}.{nameof(TurnOffCommandHandler)}...");

                Telemetry = new PreformHopperTelemetry();

                await SendMessageAsync(Telemetry);

                return new MethodResponse(0);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception on {Id}.{nameof(TurnOffCommandHandler)}: {ex.Message}");
                return new MethodResponse(-1);
            }
        }
    }
}
