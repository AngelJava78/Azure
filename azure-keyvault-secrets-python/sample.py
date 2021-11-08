from azure.keyvault.secrets import SecretClient
from azure.identity import DefaultAzureCredential

keyVaultName ="keyvault-ajv1978"
KVUri = f"https://keyvault-ajv1978.vault.azure.net/"
secretName = "birthdate3"

credential = DefaultAzureCredential()
client = SecretClient(vault_url=KVUri, credential=credential)
retrieved_secret = client.get_secret(secretName)
print(f"The value of '{secretName}' in '{keyVaultName}' is '{retrieved_secret.value}'")
