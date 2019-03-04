using PhidgetIoT.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhidgetIoT
{
    class Program
    {
        static void Main(string[] args)
        {
            var azureIoTHubService = new AzureIoTHubService();

            //Setup Phidgets
            var deviceManager = new PhidgetDeviceManagerService(azureIoTHubService);

            Console.ReadLine();
        }
    }
}
