using Flatbuilder.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Flatbuilder.DAL.Interfaces
{
    public interface IRoomManager
    {
        Task<List<Room>> GetRoomsAsync();
        Task<Room> GetRoomByIdAsync(int id);
        Task AddRoomAsync(Room r);
        Task<Room> UpdateRoomAsync(int id, Room r); 
        Task DeletRoomAsync(Room r);
        Task SaveChangesAsync();
    }
}
