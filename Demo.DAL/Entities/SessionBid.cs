using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Entities
{
    public class SessionBid
    {
        [Key]
        public int Id { get; set; }
        public DateTime SessionDate { get; set; }
        [Required]
        public string Location { get; set; }
        public string Details { get; set; }
        [Required]
        public string SeesionType { get; set; }
    }
}
