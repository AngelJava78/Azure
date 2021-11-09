using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureKeyVaultWebApp.Models.Data
{
    public interface IKeyVaultRepository<T>
    {
        List<T> GetAll();
        T GetByName(string name);
        bool Add(T item);
        bool Update(T item);
        bool Delete(string name);
    }
}
