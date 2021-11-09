using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AzureKeyVaultWebApp.Models
{
    public class SecretDto
    {
        public string Id { get; set; }
        [Display(Name = "Secret Name"), Required, StringLength(60, MinimumLength = 5)]
        public string Name { get; set; }
        [Display(Name = "Secret Value"), Required, StringLength(60, MinimumLength = 5)]
        public string Value { get; set; }
    }
}
