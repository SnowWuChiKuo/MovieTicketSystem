using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ClientSide.Models.DTOs
{
    public class RegisterDto
    {
        public string Account { get; set; }
        
        public string Email { get; set; }
       
        public string Password { get; set; }

        public string PasswordHash { get; set; }

        public string Name { get; set; }

    }
}