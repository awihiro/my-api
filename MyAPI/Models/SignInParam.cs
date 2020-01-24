using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyAPI.Models
{
    public class SignInParam
    {
        public string username { get; set; }
        public string password { get; set; }
        public string fullname { get; set; }
    }
}
