using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Models
{
    public class Student
    {

        public int Id { get; set; }
        [Required(ErrorMessage = "Please Entre Name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please Entre Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please Entre Mobile")]
        public string Mobile { get; set; }
        [Required(ErrorMessage = "Please Entre Gender")]
        public string Gender { get; set; }
        [Required(ErrorMessage = "Please Entre StudentFee")]
        public string StudentFee { get; set; }
        [Required(ErrorMessage = "Please Entre Discription")]
        public string Discription { get; set; }

        public string Subject { get; set; }
        public string Path { get; set; }
        [NotMapped]
        [Display(Name = "Choose Image")]
        public IFormFile ImagePath { get; set; }
        public DateTime Dob { get; set; }


    }
}
