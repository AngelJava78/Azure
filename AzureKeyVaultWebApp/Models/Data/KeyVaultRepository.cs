using Azure;
using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureKeyVaultWebApp.Models.Data
{
    public class KeyVaultRepository : IKeyVaultRepository<SecretDto>
    {
        private readonly SecretClient client;
        //private readonly string keyVaultName;
        private readonly string keyVaultUri;
        public KeyVaultRepository()
        {
            //keyVaultName = "keyvault-ajv1978";
            keyVaultUri = "https://keyvault-ajv1978.vault.azure.net/";
            SecretClientOptions options = new SecretClientOptions()
            {
                Retry =
                {
                    Delay= TimeSpan.FromSeconds(2),
                    MaxDelay = TimeSpan.FromSeconds(16),
                    MaxRetries = 5,
                    Mode = RetryMode.Exponential
                }
            };
            client = new SecretClient(new Uri(keyVaultUri), new DefaultAzureCredential(), options);
        }
        public bool Add(SecretDto item)
        {
            var result = false;
            try
            {
                client.SetSecret(item.Name, item.Value);
                result = true;
            }
            catch
            {
                result = false;
            }
            return result;
        }

        public bool Delete(string name)
        {
            var result = false;
            try
            {
                client.StartDeleteSecret(name);
                result = true;
            }
            catch
            {
                result = false;
            }
            return result;
        }

        public List<SecretDto> GetAll()
        {
            var secrets = new List<SecretDto>();
            Pageable<SecretProperties> allSecrets = client.GetPropertiesOfSecrets();

            foreach (SecretProperties secretProperties in allSecrets)
            {
                var secret = GetByName(secretProperties.Name);
                secrets.Add(secret);
            }
            return secrets;
        }

        public SecretDto GetByName(string name)
        {
            KeyVaultSecret secret = client.GetSecret(name);
            var result = new SecretDto
            {
                Id = secret.Name,
                Name = secret.Name,
                Value = secret.Value
            };
            return result;
        }

        public bool Update(SecretDto item)
        {
            return Add(item);
            //var result = false;
            //try
            //{
            //    client.SetSecret(item.Name, item.Value);
            //    result = true;
            //}
            //catch
            //{
            //    result = false;
            //}
            //return result;
        }
    }
}
