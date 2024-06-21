using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Entities
{
    public class ApplicationUser : IdentityUser

    {

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public string Role { get; set; }

        public string City { get; set; }
        public string State { get; set; }

        public string StreetAddress { get; set; }
        //////

        public string SSN { get; set; }
        public string PortofiloUrl { get; set; }
        public string BankAccountNumber { get; set; }
        public string AccountStatus { get; set; }
        public string Feedback { get; set; }
        public bool IsActive { get; set; }

        public string ImagePath { get; set; }

        public string PriceRange { get; set; }

        public Availiability Availiability { get; set; }
        public Location Location { get; set; }
        public Specialty Specialty { get; set; }

        ////////
        public int YearsOfExperience { get; set; }

        public ICollection<Order> Orders { get; set; }

        public ICollection<Course> Courses { get; set; }//for instructor
        public ICollection<Course> PurchasedCourses { get; set; }//for client

        public ICollection<Product> Products { get; set; }

        public ICollection<Message> SentMessages { get; set; }
        public ICollection<Message> ReceivedMessages { get; set; }
        
        public ICollection<PhotographerImages> PhotographerImages { get; set; } = new List<PhotographerImages>();
    }

    public enum Specialty
    {
        Wedding,
        Product,
        Portrait,
        Event
    }

    public enum Availiability
    {
        Weekdays,
        Weekends,
        Anytime

    }

    public enum Location
    {
        Cairo,
        Giza,
        Alex,
        Fayoum

    }
    

}
