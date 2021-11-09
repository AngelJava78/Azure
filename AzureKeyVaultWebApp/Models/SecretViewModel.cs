using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureKeyVaultWebApp.Models
{
    public class SecretViewModel
    {
        public List<SecretDto> Secrets { get; set; }
        public string SecretName { get; set; }
    }
}
