using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PhidgetIoT.Services
{
    public class AzureIoTHubService
    {
        private string deviceName = "PhidgetSliderBoard";
        private DeviceClient deviceClient;

        private string deviceConnectionString = "";
        private TransportType transportType = TransportType.Amqp;

        public AzureIoTHubService()
        {
            deviceClient = DeviceClient.CreateFromConnectionString(deviceConnectionString, transportType);
            if (deviceClient == null)
            {
                Console.WriteLine("Failed to create DeviceClient!");
                return;
            }
        }

        public string DeviceName { get => deviceName; set => deviceName = value; }

        public async void SendValueToIoTHub(double channel, double value)
        {           
            var telemetryDataPoint = new
            {
                channel = channel,
                value = value
            };

            var messageString = JsonConvert.SerializeObject(telemetryDataPoint);
            var message = new Message(Encoding.ASCII.GetBytes(messageString));

            await deviceClient.SendEventAsync(message).ConfigureAwait(false);
        }       
    }
}
