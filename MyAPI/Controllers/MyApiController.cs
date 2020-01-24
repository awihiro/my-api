using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyAPI.DataAccess;
using MyAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyAPI.Controllers
{
    [Route("api")]
    public class MyApiController : Controller
    {
        private readonly MyAPIContext _myAPIContext;

        public MyApiController(MyAPIContext myAPIContext)
        {
            _myAPIContext = myAPIContext;
        }

        public IActionResult Index()
        {
            return Ok("MyAPI v1.0.0");
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody]LoginParam param)
        {
            if(string.IsNullOrEmpty(param.username) || string.IsNullOrEmpty(param.password))
            {
                return Forbid();
            }

            var md5hash = GetMd5Hash(param.password);
            var result = _myAPIContext.User.Where(
                x => x.username == param.username 
                    && x.password == md5hash)
                    .FirstOrDefault();
            if(result == null)
            {
                return NoContent();
            }

            return Ok(result);
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            return Ok();
        }

        [HttpPost("signup")]
        public IActionResult SignUp([FromBody]SignInParam param)
        {
            if (string.IsNullOrEmpty(param.username) || string.IsNullOrEmpty(param.password))
            {
                return StatusCode(403);
            }

            User user = _myAPIContext.User.Find(param.username);
            if (user != null)
            {
                return Conflict();
            }

            user = new User();
            user.username = param.username;
            user.password = GetMd5Hash(param.password);
            user.fullname = param.fullname;

            var result = _myAPIContext.User.Add(user);
            _myAPIContext.SaveChanges();

            return Ok(user);
        }

        private static string GetMd5Hash(string input, bool useUpperCase = false)
        {
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
            byte[] data = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(input));
            md5Hasher.Clear();
            StringBuilder sb = new StringBuilder();
            string format;
            format = (useUpperCase ? "X2" : "x2");
            for (int i = 0; i < data.Length; i++)
            {
                sb.Append(data[i].ToString(format));
            }

            return sb.ToString();
        }
    }
}
