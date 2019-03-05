using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;
using Microsoft.Azure.Management.SignalR;
using Microsoft.Azure.Management.SignalR.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureSignalRMgmt
{
    class Program
    {
        static void Main(string[] args)
        {
            SignalRManagementClient client = new SignalRManagementClient(CredentialHelper.GetServiceClientCredentials());
            client.SubscriptionId = ConfigurationManager.AppSettings["SubscriptionId"];
            var resourceGroup = SdkContext.RandomResourceName("rgTest", 20);
            var region = Region.USWest;

            // create ResourceGroup
            var azure = CredentialHelper.GetAzure();
            azure.ResourceGroups.Define(resourceGroup)
                .WithRegion(region)
                .Create();

            try
            {

                // create SignalR
                var signalRName = SdkContext.RandomResourceName("srTest", 20);
                var parameter = new SignalRCreateParameters
                {
                    Location = "westus",
                    Sku = new ResourceSku
                    {
                        Name = "Standard_S1",
                        Tier = "Standard",
                        Capacity = 1,
                    },
                    Properties = new SignalRCreateOrUpdateProperties { },
                };

                var sr = client.SignalR.CreateOrUpdate(resourceGroup, signalRName, parameter);
                Console.WriteLine(sr.HostName);
                Console.WriteLine(sr.ExternalIP);

                // Scale
                Console.Read();
                var scale = new SignalRUpdateParameters
                {
                    Sku = new ResourceSku
                    {
                        Name = "Standard_S1",
                        Capacity = 2,
                    }
                };
                client.SignalR.Update(resourceGroup, signalRName, scale);
                Console.WriteLine("updated");

                // Delete
                Console.Read();
                client.SignalR.Delete(resourceGroup, signalRName);
                Console.WriteLine("deleted");
            }
            finally
            {
                azure.ResourceGroups.DeleteByName(resourceGroup);
                Console.WriteLine("Group Deleted");
            }

            Console.Read();
        }
    }
}
