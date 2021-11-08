using System;
using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

namespace keyvault_console_app
{
    class Program
    {
        static void Main(string[] args)
        {
            var secretName = string.Empty;
            //Console.WriteLine("Hello World!");
            
            
            string keyVaultName = "keyvault-ajv1978";
            string kvUri = "https://keyvault-ajv1978.vault.azure.net/";
            SecretClientOptions options = new SecretClientOptions
            {
                Retry =
                {
                    Delay = TimeSpan.FromSeconds(2),
                    MaxDelay = TimeSpan.FromSeconds(16),
                    MaxRetries = 5,
                    Mode = RetryMode.Exponential
                }
            };
            Console.Write("Input the name of your secret: ");
            secretName = Console.ReadLine();
            var client = new SecretClient(new Uri(kvUri), new DefaultAzureCredential(), options);
            Console.Write("Input the value of your secret: ");
            var secretValue = Console.ReadLine();

            Console.Write($"Creating a seret in: {keyVaultName} called: {secretName} with the value: {secretValue}... ");
            client.SetSecret(secretName, secretValue);
            Console.WriteLine("Done");

            Console.WriteLine("Forgetting your secret");
            secretValue = string.Empty;
            Console.WriteLine($"Your secret is: {secretValue}");

            Console.WriteLine($"Retrieving your secret from {keyVaultName}.");
            KeyVaultSecret secret = client.GetSecret(secretName);
            Console.WriteLine($"Your secret is: {secret.Value}");

            Console.Write($"Deleting your secret from {keyVaultName} ...");
            client.StartDeleteSecret(secretName);
            System.Threading.Thread.Sleep(5000);
            Console.WriteLine(" done.");
        }
    }
}
