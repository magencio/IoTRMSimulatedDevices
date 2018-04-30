using Microsoft.Azure.Devices.Client;
using Simulation.Core;
using Simulation.Core.Helpers;
using System;
using System.Threading.Tasks;
using static Simulation.Core.Helpers.RandomHelper;

namespace Rinser
{
    public class RinserDevice : Device
    {
        public RinserTelemetry Telemetry { get; private set; }

        public RinserDevice(string id, string primaryKey, string schema, string jsonSchemaFile, string iotHub)
            : base (id, primaryKey, schema, jsonSchemaFile, iotHub)
        {
            Telemetry = new RinserTelemetry()
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

            Telemetry.emptybottlecounterin = (int)Vary(24, 5, 22, 25);
            Telemetry.emptybottlecounterout = (int)Vary(24, 5, 22, Telemetry.emptybottlecounterin);
            Telemetry.machinespeed = (int)Vary(25200, 5, 23940, 26460);
            Telemetry.electricityconsumption = Vary(30, 5, 28, 32);
            Telemetry.waterconsumption = Vary(1000, 5, 950, 1050);

            await SendMessageAsync(Telemetry);
        }

        private async Task<MethodResponse> TurnOnCommandHandler(MethodRequest methodRequest, object userContext)
        {
            try
            {
                Console.WriteLine($"{Id}.{nameof(TurnOnCommandHandler)}...");

                Telemetry = new RinserTelemetry()
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

                Telemetry = new RinserTelemetry();

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
