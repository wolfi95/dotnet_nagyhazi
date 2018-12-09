using System;
using System.Collections.Generic;

namespace Flatbuilder.DTO
{
    //Already made orders
    public class Order
    {
        public int Id { get; set; }
        public int CostumerId { get; set; }
        public Costumer Costumer { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Location { get; set; }
        public List<Room> Rooms { get; set; }
        public double Price { get; set; }
    }
}
