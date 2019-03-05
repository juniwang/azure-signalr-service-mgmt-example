using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;
using Microsoft.Rest;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureSignalRMgmt
{
    public class CredentialHelper
    {
        public static AzureCredentials GetServiceClientCredentials()
        {
            // see https://docs.microsoft.com/en-us/azure/active-directory/develop/howto-create-service-principal-portal about how to create a new SP
            var clientId = ConfigurationManager.AppSettings["AzureSPClientId"];
            var clientSecret = ConfigurationManager.AppSettings["AzureSPClientSecret"];
            var tenantId = ConfigurationManager.AppSettings["TenantId"];
            var subscriptionId = ConfigurationManager.AppSettings["SubscriptionId"];

            var credential = SdkContext.AzureCredentialsFactory.FromServicePrincipal(
               clientId,
               clientSecret,
               tenantId,
               AzureEnvironment.AzureGlobalCloud)
               .WithDefaultSubscription(subscriptionId);

            return credential;
        }

        public static IAzure GetAzure()
        {
            return Azure.Configure()
                .Authenticate(GetServiceClientCredentials())
                .WithSubscription(ConfigurationManager.AppSettings["SubscriptionId"]);
        }
    }
}
