using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Kurcash.Models.TemporaryModels
{
    public class MyItem
    {
        public int Id { get; set; } 
        public string? Name { get; set; }
        public int? Price { get; set; }
        public int? BucketId { get; set; }
        public BitmapImage ImagePreview { get; set; }
        public string ItemType { get; set; }
        public string colorName { get; set; }

    }
}
