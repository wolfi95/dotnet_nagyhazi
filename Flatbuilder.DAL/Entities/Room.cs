using System;
using System.Collections.Generic;
using System.Text;

namespace Flatbuilder.DAL.Entities
{
    //generalization of all the rooms
    public class Room
    {
        public int Id { get; set; }       
        public double Price { get; set; }
        public string Type { get; set; }
        
        public ICollection<OrderRoom> OrderRooms { get; set; }

        public Room()
        {
            OrderRooms = new List<OrderRoom>();
        }
    }
}
