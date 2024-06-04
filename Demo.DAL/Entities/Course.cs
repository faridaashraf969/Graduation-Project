using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Entities
{
    public class Course
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Course Topic Is Required")]
        public string Topic { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
        [Required]
        public string Duration { get; set; }
        public int Rate { get; set; }
        [Required(ErrorMessage = "Course Description Is Required")]
        public string Description { get; set; }
        public string Feedback { get; set; }

        public string InstructorId { get; set; }
        public ApplicationUser Instructor { get; set; }
        public string Status {  get; set; }
        [NotMapped]
        public IFormFile Image { get; set; }
        public string ImageName { get; set; }
        public string Requirements {  get; set; } 
        public CartItem CartItem { get; set; }
    }
}
