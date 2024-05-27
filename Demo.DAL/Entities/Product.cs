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
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage ="Product Name Is Required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Product Description Is Required")]
        public string Description { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
        [Required]
        public int Quantity { get; set; }

        public string ImageName { get; set; }
        [ForeignKey("Category")]
        public int? CategoryId { get; set; } //FK
        [InverseProperty("Product")]
        public Category Category { get; set; }
        public string Status { get; set; }
        [NotMapped]
        public IFormFile Image { get; set; }


    }
}
