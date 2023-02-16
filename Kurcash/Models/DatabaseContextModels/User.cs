using System;
using System.Collections.Generic;

namespace Kurcash.Models.DatabaseContextModels
{
    public partial class User
    {
        public int Id { get; set; }
        public string Login { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? Email { get; set; }
        public int RoleId { get; set; }
        public string? Fullname { get; set; }

        public virtual Bucket IdNavigation { get; set; } = null!;
        public virtual Role Role { get; set; } = null!;
    }
}
