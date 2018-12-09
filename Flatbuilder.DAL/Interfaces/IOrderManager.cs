using Flatbuilder.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Flatbuilder.DAL.Interfaces
{
    public interface IOrderManager
    {
        Task<List<Order>> GetOrdersAsync();
        Task<List<Order>> GetOrdersByNameAsync(string name);
        Task<Order> GetOrderByIdAsync(int id);
        Task DeleteOrderAsync(int id);
        Task<Order> AddOrderAsync(Order order,List<Room> rooms);
        Task<List<Room>> GetFreeRoomsAsync(DateTime start, DateTime end);
        Task InsertAsync();
        Task SaveChangesAsync();
    }
}
