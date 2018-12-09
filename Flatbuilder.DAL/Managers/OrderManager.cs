using Flatbuilder.DAL.Context;
using Flatbuilder.DAL.Entities;
using Flatbuilder.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Flatbuilder.DAL.Managers
{
    public class OrderManager : IOrderManager
    {
        private readonly FlatbuilderContext _context;

        public OrderManager(FlatbuilderContext context)
        {
            _context = context;
        }

        public async Task<List<Order>> GetOrdersAsync()
        {
            var orders = await _context.Orders
                .Include(o => o.Costumer)
                .Include(o => o.OrderRooms)
                .ThenInclude(or => or.Room)
                .AsNoTracking()
                .ToListAsync();
            return orders;

            //var kitchens = _context.Rooms.OfType<Kitchen>().ToList();
            //var zuhayn = _context.Rooms.OfType<Shower>().ToList();
        }

        public async Task<List<Order>> GetOrdersByNameAsync(string name)
        {
            if (await _context.Costumers.FirstOrDefaultAsync(c => c.Name.Equals(name)) == null)
                return null;
            var orders = await _context.Orders
                .Include(o => o.Costumer)
                .Include(o => o.OrderRooms)
                .ThenInclude(or => or.Room)
                .AsNoTracking()
                .Where(o => o.Costumer.Name.Equals(name))
                .ToListAsync();
            return orders;
        }

        public async Task<Order> GetOrderByIdAsync(int id)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == id);

            return order;
        }

        public async Task DeleteOrderAsync(int id)
        {
            Order orderToDelete = await _context.Orders.Include(o => o.OrderRooms).FirstOrDefaultAsync(o => o.Id == id);
            //csak igy torli a hozza tartozokat is
            _context.Remove(orderToDelete);

            await SaveChangesAsync();
        }

        public async Task<List<Room>> GetFreeRoomsAsync(DateTime start, DateTime end)
        {
            if (end <= start)
                return null;
            var freerooms = await _context.Rooms
                .Include(r => r.OrderRooms)
                .Where(r => (!_context.OrderRooms.Select(or => or.RoomId).Contains(r.Id)) //meg nem foglalt szobak
                   || (!(_context.OrderRooms  //szobak amik nincsenek zavaro foglalasok szobai kozt
                            .Where(or => (_context.Orders
                                .Where(o => (o.StartDate < end && o.EndDate > start)
                                    || (o.EndDate > start && o.StartDate < end))
                                .Select(o => o.Id))
                            .Contains(or.OrderId))
                            .Select(or => or.RoomId)).Contains(r.Id)))
                .AsNoTracking()
                .ToListAsync();

            if (freerooms == null)
                return null;
            return freerooms;
        }

        public async Task<Order> AddOrderAsync(Order order, List<Room> rooms)
        {
            if(order.EndDate <= order.StartDate)
                throw new Exception("Invalid time interval");

            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var freerooms = await _context.Rooms
                .Include(r => r.OrderRooms)
                .Where(r => (!_context.OrderRooms.Select(or => or.RoomId).Contains(r.Id)) //meg nem foglalt szobak
                   || (!(_context.OrderRooms  //szobak amik nincsenek zavaro foglalasok szobai kozt
                            .Where(or => (_context.Orders
                                .Where(o => (o.StartDate < order.EndDate && o.EndDate > order.StartDate)
                                    || (o.EndDate > order.StartDate && o.StartDate < order.EndDate))
                                .Select(o => o.Id))
                            .Contains(or.OrderId))
                            .Select(or => or.RoomId)).Contains(r.Id)))
                .AsNoTracking()
                .ToListAsync();

                if (freerooms == null)
                    return null;

                List<OrderRoom> newors = new List<OrderRoom>();

                for (int i = 0; i < rooms.Count; i++)
                {
                    string type="";
                    foreach (var fr in freerooms)
                    {
                        if (fr.GetType().ToString().Equals("Flatbuilder.DAL.Entities.Kitchen"))
                            type = "Flatbuilder.DTO.Kitchen";
                        if (fr.GetType().ToString().Equals("Flatbuilder.DAL.Entities.Shower"))
                            type = "Flatbuilder.DTO.Shower";
                        if (fr.GetType().ToString().Equals("Flatbuilder.DAL.Entities.Bedroom"))
                            type = "Flatbuilder.DTO.Bedroom";
                        if (rooms[i].Type.ToString().Equals(type))
                        {
                            newors.Add(new OrderRoom { RoomId = fr.Id, Note = "megrendeles" });
                            freerooms.Remove(fr);
                            break;
                        }
                    }
                    if (newors.Count != (i + 1))
                        return null;
                }

                int costmerid = await _context.Costumers
                    .Where(c => c.Name == order.Costumer.Name)
                    .Select(c => c.Id)
                    .FirstOrDefaultAsync();

                Order newo = new Order
                {
                    CostumerId = costmerid,
                    StartDate = order.StartDate,
                    EndDate = order.EndDate,
                    OrderRooms = newors,
                    Price = order.Price
                };

                await _context.AddAsync(newo);

                await SaveChangesAsync();

                scope.Complete();

                return newo;
            }
        }

        public async Task InsertAsync(/*Order order*/)
        {
            var room = new Shower { Price = 200 };
            _context.Rooms.Add(room);
            var room2 = new Bedroom { Price = 100 };
            _context.Rooms.Add(room2);
            var room3 = new Bedroom { Price = 100 };
            _context.Rooms.Add(room3);


            _context.Add(new Order
            {
                Costumer = new Costumer { Name = "nevem" },
                StartDate = DateTime.Now.AddDays(-5),
                EndDate = DateTime.Now.AddDays(1),
                OrderRooms = new List<OrderRoom>
                {
                    new OrderRoom { Room = room, Note = "megrendeles" },
                    new OrderRoom { Room = room2, Note = "megrendeles" },
                    new OrderRoom { Room = room3, Note = "megrendeles "}
                },
                Price = (room.Price + room2.Price + room3.Price) * 6
            });

            await _context.SaveChangesAsync();
            //var foglalasi_szobak = order.Rooms.Select(r => r.Id).ToList();

            //var marFoglalva = await _context.Orders.AnyAsync(o => o.Rooms.Any(r => foglalasi_szobak.Contains(r.Id)));
            //var roomId = 7;
            //var szabad = _context.Rooms.Where(r => r.Id == roomId && r.OrderRooms.Any(or => or.Order.EndDate < DateTime.Now ))
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
