using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Entities
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Category Name Is Required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Category Description Is Required")]
        public string Description { get; set; }

        [InverseProperty("Category")]
        public List<Product> Product { get; set; }
    }
}
