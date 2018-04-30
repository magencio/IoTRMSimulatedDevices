using System;
using System.Threading.Tasks;

namespace Filler
{
    class Program
    {
        static async Task Main(string[] args)
        {
            FillerDevice device = null;

            try
            {
                device = new FillerDevice(
                    Environment.GetEnvironmentVariable("DeviceId"),
                    Environment.GetEnvironmentVariable("DevicePrimaryKey"),
                    Environment.GetEnvironmentVariable("Schema"),
                    Environment.GetEnvironmentVariable("JsonSchemaFile"),
                    Environment.GetEnvironmentVariable("IoTHubAddress")
                );

                await device.ConnectAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception while connecting {device?.Id} to IoT Hub: {ex.Message}");
                return;
            }

            while (true)
            {
                try
                {
                    await Task.Delay(5000);
                    await device.UpdateAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception on {device?.Id}: {ex.Message}");
                }
            }
        }
    }
}
