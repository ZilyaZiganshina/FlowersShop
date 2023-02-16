using System;
using System.Collections.Generic;

namespace Kurcash.Models.DatabaseContextModels
{
    public partial class Bucket
    {
        public Bucket()
        {
            Flowers = new HashSet<Flower>();
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public DateTime CreationDate { get; set; }

        public virtual User? User { get; set; }
        public virtual ICollection<Flower> Flowers { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
