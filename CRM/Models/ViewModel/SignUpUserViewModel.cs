using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Models.ViewModel
{
    public class SignUpUserViewModel
    {
        public int Id { get; set; }  
        [Display(Name ="Username")]
        [Required(ErrorMessage = "Please enter Username")]
        [Remote(action: "UserNameIsExite",controller:"Account")]
        public string Username { get; set; }
        [Display(Name ="Email")]
        [Required(ErrorMessage = "Please enter Email")]
        public string Email { get; set; }
        
        public long? Mobile { get; set; }
        [Display(Name ="password")]
        [Required(ErrorMessage = "Please enter Password")]
        public string Password { get; set; } 

        [Display(Name ="Confirm Password")]
        [Required(ErrorMessage = "Please enter Confirm Password")]
        public string ConfirmPassword{ get; set; } 
        [Display(Name ="Active")]
        public bool isActive { get; set; }
      

    }

}
