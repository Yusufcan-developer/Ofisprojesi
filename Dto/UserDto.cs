using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ofisprojesi
{
         public class UserDto{
        public int? Id { get; set; }
        public string Username { get; set; }
        public bool IsLocked { get; set; }
        public int? RoleId { get; set; }
        public string Token { get; set; }
        public RoleDto Role { get; set; }
    }

    public class UserUpdateDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsLocked { get; set; }
        public int? RoleId { get; set; }
        public string Email { get; set; }
    }

    public class RoleDto
    {
        public int? Id { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }
    }
    }
