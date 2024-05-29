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
        [Required(ErrorMessage = "Product Description Is Required")]
        public string Description { get; set; }
        public string Feedback { get; set; }
        [ForeignKey("Instructor")]
        public int? CategoryId { get; set; } //FK
        [InverseProperty("Cources")]
        public Instructor Instructor { get; set; }
    }
}
