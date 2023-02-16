using Kurcash.Models.DatabaseContextModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kurcash.Models.StaticModels
{
    public static class CurrentUser
    {
        public static User Current { get; set; }

    }
}
