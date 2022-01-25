using System;
using System.Collections.Generic;

#nullable disable

namespace ofisprojesi
{
    public partial class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int? RoleId { get; set; }
        public bool? IsLocked { get; set; }
        public string PasswordKey { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? CreateUserId { get; set; }
        public string CreateIpAdress { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public int? LastUpdateUserId { get; set; }
        public string LastUpdateIpAdress { get; set; }

        public virtual Role Role { get; set; }
    }
}
