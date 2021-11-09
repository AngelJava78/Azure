using System;
using System.Threading.Tasks;
using Azure.Identity;
using Azure.Security.KeyVault.Keys;

namespace key_vault_keys_console_app
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var keyName = string.Empty;
            Console.Write("Input the name of your key: ");
            keyName = Console.ReadLine();
            var keyVaultName = "keyvault-ajv1978";
            var kvUri = $"https://{keyVaultName}.vault.azure.net";

            var client = new KeyClient(new Uri(kvUri), new DefaultAzureCredential());
            Console.WriteLine($"Creating a key in {keyVaultName} calle: {keyName} ...");
            var createdKey = client.CreateKey(keyName, KeyType.Rsa);
            Console.WriteLine(" done.");

            Console.WriteLine($"Retrieving your key from {keyVaultName}");
            var key = client.GetKey(keyName);
            Console.WriteLine($"Your key version is: '{key.Value.Properties.Version}'");

            Console.Write($"Deleting your key: {keyName} from {keyVaultName} ...");
            var deleteOperation = await client.StartDeleteKeyAsync(keyName);
            await deleteOperation.WaitForCompletionAsync();
            if(deleteOperation.HasCompleted)
            {
                Console.WriteLine(" done");
            }
            else
            {
                Console.WriteLine(" failed");
            }

            Console.Write($"Purging your key: {keyName} from {keyVaultName} ...");
            client.PurgeDeletedKey(keyName);
            Console.WriteLine(" done.");
        }
    }
}
