using System;
using System.Collections.Generic;
using System.Text;

namespace Flatbuilder.DAL.Entities
{
    public class OrderRoom
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public Room Room { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public string Note { get; set; }
       // public double Price { get; set; }
    }
}