using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Entities
{
    public class Course
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Topic { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public string Duration { get; set; }
        public int Rate { get; set; }
        [Required]
        public string Description { get; set; }
        public string Feedback { get; set; }
    }
}
