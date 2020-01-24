using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MyAPI.Models
{
    [Table("user")]
    public class User
    {
        public string username { get; set; }

        [JsonIgnore]
        public string password { get; set; }
        public string fullname { get; set; }
    }
}
