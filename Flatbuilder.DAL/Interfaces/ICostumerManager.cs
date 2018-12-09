using Flatbuilder.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Flatbuilder.DAL.Interfaces
{
    public interface ICostumerManager
    {
        Task<List<Costumer>> GetCostumersAsync();
        Task<Costumer> GetCostumerByIdAsync(int id);
        Task<Costumer> GetCostumerByNameAsync(string name);
        Task AddCostumerAsync(Costumer c);
        Task<Costumer> UpdateCostumerAsync(int id, Costumer c);
        Task DeletCostumerAsync(Costumer c);
        Task SaveChangesAsync();
    }
}
