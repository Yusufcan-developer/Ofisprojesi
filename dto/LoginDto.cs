using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ofisprojesi
{
    public class LoginDto
    {
        public string username{get;set;}
        public string Password{get;set;}
    }
    public class changePasswordDto{
        public string username{get;set;}
        public string oldPassword{get;set;}
        public string newPassword{get;set;}
    }
}