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
  
        [DataType(DataType.Currency)]
        [Required(ErrorMessage = "Price is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive number")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Course Description Is Required")]
        public string Description { get; set; }

        public string InstructorId { get; set; }
        public ApplicationUser Instructor { get; set; }

        public ICollection<ApplicationUser> Purchasers { get; set; }

        [NotMapped]
        public IFormFile Image { get; set; }
        public string ImageName { get; set; }

        public string Requirements {  get; set; }

        [Display(Name = "Enrollment Start Date")]
        public DateTime EnrollmentStartDate { get; set; }
        [Display(Name = "Category")]
        public CourseCategory Category { get; set; }

        [Display(Name = "Status")]
        public CourseStatus CourseStatus { get; set; }

        public string Status {  get; set; }
        ////

        [Display(Name = "Total Hours")]
        public int TotalHours { get; set; }

        [Display(Name = "Video Content")]
        public string VideoContentUrl { get; set; }

    }
    public enum CourseCategory
    {
        FreelancingPhotography,
        WeddingPhotography,
        FashionPhotography,
        FilmSetPhotography,
        ProductPhotography
    }
    public enum CourseStatus
    {
        Draft,
        Published,
        Archived
    }
}
