using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kurcash.Models.TemporaryModels
{
    public class MyOrderItem
    {
        public int Id { get; set; }
        public int? TotalPrice { get; set; }

        public DateTime? CreationDate { get; set; }

        public string Items { get; set; }

        

    }
}
