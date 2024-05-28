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

        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        //[ForeignKey("Product")]
        //public int ProductId { get; set; }
        [InverseProperty("Category")]
        // navigational property [many]
        public ICollection<Product> Products { get; set; } = new HashSet<Product>();
        // public List<Product> Products { get; set; } 
    }
}
