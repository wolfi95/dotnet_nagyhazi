using System.Collections.Generic;

namespace Flatbuilder.DTO
{
    public class Costumer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int OrderId { get; set; }
        public ICollection<Order> Orders { get; set; }
        public Costumer()
        {
            Orders = new List<Order>();
        }
    }
}