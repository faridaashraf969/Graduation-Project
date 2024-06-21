using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Entities
{
    public class PhotographerImages
    {
        public int Id { get; set; }


        public string ImagePath { get; set; }

     
        public string PhotographerId { get; set; }

        // Navigation property to the Photographer
        public ApplicationUser Photographer { get; set; }
    }
}
