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
    public class RoomManager : IRoomManager
    {
        private readonly FlatbuilderContext _context;

        public RoomManager(FlatbuilderContext context)
        {
            _context = context;
        }

        public async Task<Room> GetRoomByIdAsync(int id)
        {
            return await _context.Rooms.FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<List<Room>> GetRoomsAsync()
        {
            var rooms = await _context.Rooms
                .AsNoTracking()
                .ToListAsync();
            return rooms;
        }

        public async Task AddRoomAsync(Room r)
        {
            await _context.Rooms.AddAsync(r);
            await SaveChangesAsync();
        }

        public async Task DeletRoomAsync(Room r)
        {
            _context.Remove(r);
            await SaveChangesAsync();
        }

        public async Task<Room> UpdateRoomAsync(int id, Room r)
        {
            var update = await GetRoomByIdAsync(id);

            if (update == null)
                return null;

            update.Price = r.Price;

            await SaveChangesAsync();

            return update;
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
    }
}