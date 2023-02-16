using System;
using System.Collections.Generic;

namespace Kurcash.Models.DatabaseContextModels
{
    public partial class Type
    {
        public Type()
        {
            Flowers = new HashSet<Flower>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Flower> Flowers { get; set; }
    }
}
