using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Models.ViewModel
{
    public class LoginSignUpViewModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; } 
        [Display(Name = "Remember Me")]
        public bool IsRemember { get; set; }
    }
}
