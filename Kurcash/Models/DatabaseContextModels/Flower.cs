using System;
using System.Collections.Generic;

namespace Kurcash.Models.DatabaseContextModels
{
    public partial class Flower
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? Price { get; set; }
        public int TypeId { get; set; }
        public int? ColorId { get; set; }
        public byte[]? Image { get; set; }
        public int? BucketId { get; set; }
        public int? OrderId { get; set; }

        public virtual Bucket? Bucket { get; set; }
        public virtual Color? Color { get; set; }
        public virtual Order? Order { get; set; }
        public virtual Type Type { get; set; } = null!;
    }
}
