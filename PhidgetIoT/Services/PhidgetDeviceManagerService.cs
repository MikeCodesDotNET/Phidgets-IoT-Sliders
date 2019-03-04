using Phidget22;
using PhidgetIoT.Services;
using System;
using System.Threading;

namespace PhidgetIoT
{
    public class PhidgetDeviceManagerService
    {
        public Manager Manager;
        public AzureIoTHubService IoTHubService;

        public PhidgetDeviceManagerService(AzureIoTHubService azureIoTHubService)
        {
            IoTHubService = azureIoTHubService;

            Manager = new Manager();
            Manager.Attach += DeviceAttached;
            Manager.Detach += DeviceDetached;
            Manager.Open();
        }

        private void DeviceDetached(object sender, Phidget22.Events.ManagerDetachEventArgs e)
        {
            Manager.Close();
        }

        private void DeviceAttached(object sender, Phidget22.Events.ManagerAttachEventArgs e)
        {
            var slider = new VoltageRatioInput() { Channel = e.Channel.Channel };
            slider.Open();

            Thread.Sleep(200);

            if (slider.Attached)
            {
                slider.VoltageRatioChange += SliderValueChanged;
                Console.WriteLine($"Detected Device on Channel {slider.Channel}");
            }
        }

        private void SliderValueChanged(object sender, Phidget22.Events.VoltageRatioInputVoltageRatioChangeEventArgs e)
        {
            var input = sender as VoltageRatioInput;
            if (e.VoltageRatio > 0)
            {
                var percentage = VoltageToPercentage(e.VoltageRatio);
                IoTHubService.SendValueToIoTHub(input.Channel, percentage);

                Console.WriteLine($"Channel: {input.Channel} : {percentage}");
            }
        }

        private double VoltageToPercentage(double voltage)
        {
            return Math.Round((voltage * 100), 0);
        }
    }
}
