using Flatbuilder.DAL.Context;
using Flatbuilder.DAL.Entities;
using Flatbuilder.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flatbuilder.DAL.Managers
{
    public class CostumerManager : ICostumerManager
    {
        private readonly FlatbuilderContext _context;

        public CostumerManager(FlatbuilderContext context)
        {
            _context = context;
        }

        public async Task AddCostumerAsync(Costumer c)
        {
            await _context.Costumers.AddAsync(c);
            await SaveChangesAsync();
        }

        public async Task DeletCostumerAsync(Costumer c)
        {
            _context.Remove(c);
            await SaveChangesAsync();
        }

        public async Task<Costumer> GetCostumerByIdAsync(int id)
        {
            return await _context.Costumers.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Costumer> GetCostumerByNameAsync(string name)
        {
            return await _context.Costumers.FirstOrDefaultAsync(c => c.Name.Equals(name));
        }

        public async Task<List<Costumer>> GetCostumersAsync()
        {
            var costumers = await _context.Costumers
                .AsNoTracking()
                .ToListAsync();
            return costumers;
        }

        public async Task SaveChangesAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new Exception("Concurrency error");
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<Costumer> UpdateCostumerAsync(int id, Costumer c)
        {
            var update = await GetCostumerByIdAsync(id);

            if (update == null)
                return null;

            update.Name = c.Name;

            await SaveChangesAsync();

            return update;
        }
    }
}