using System;
using System.Collections.Generic;

namespace Kurcash.Models.DatabaseContextModels
{
    public partial class Order
    {
        public Order()
        {
            Flowers = new HashSet<Flower>();
        }

        public int Id { get; set; }
        public int BucketId { get; set; }
        public int? TotalPrice { get; set; }
        public DateTime? CreationDate { get; set; }

        public virtual Bucket Bucket { get; set; } = null!;
        public virtual ICollection<Flower> Flowers { get; set; }
    }
}
