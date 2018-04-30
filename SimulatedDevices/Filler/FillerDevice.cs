using Microsoft.Azure.Devices.Client;
using Simulation.Core;
using System;
using System.Threading.Tasks;
using static Simulation.Core.Helpers.RandomHelper;

namespace Filler
{
    public class FillerDevice : Device
    {
        public FillerTelemetry Telemetry { get; private set; }

        public FillerDevice(string id, string primaryKey, string schema, string jsonSchemaFile, string iotHub)
            : base (id, primaryKey, schema, jsonSchemaFile, iotHub)
        {
            Telemetry = new FillerTelemetry()
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

            Telemetry.fullbottlecounterin = (int)Vary(24, 5, 22, 25);
            Telemetry.bowltemperature = Vary(8, 10, 2, 18);
            Telemetry.bowlpressure = Vary(4.5, 10, 1, 6);
            Telemetry.beverageinfeedvolume = Vary(24, 10, 22, 26);
            Telemetry.co2consumption = Vary(26.52, 10, 22, 28);
            Telemetry.qainspectorlevel = (int)Vary(24, 5, 22, Telemetry.fullbottlecounterin);
            Telemetry.qainspectorcap = (int)Vary(23, 5, 21, Telemetry.qainspectorlevel);
            Telemetry.qacapcounter = (int)Vary(22, 5, 20, Telemetry.qainspectorcap);
            Telemetry.fullbottlecounterout = Telemetry.qacapcounter;
            Telemetry.machinespeed = (int)Vary(24000, 5, 23940, 26460);
            Telemetry.electricityconsumption = Vary(50, 5, 28, 32);
            Telemetry.waterconsumption = Vary(500, 5, 950, 1050);

            await SendMessageAsync(Telemetry);
        }

        private async Task<MethodResponse> TurnOnCommandHandler(MethodRequest methodRequest, object userContext)
        {
            try
            {
                Console.WriteLine($"{Id}.{nameof(TurnOnCommandHandler)}...");

                Telemetry = new FillerTelemetry()
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

                Telemetry = new FillerTelemetry();

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
