using System.ComponentModel.DataAnnotations;

namespace Demo.PL.Models.UserRegister
{
    public class InstructorViewModel
    {
        [Required(ErrorMessage = "First Name Is Required")]
        public string FName { get; set; }
        [Required(ErrorMessage = "Last Name Is Required")]
        public string LName { get; set; }
        [EmailAddress(ErrorMessage = "Invalid Email")]
        [Required(ErrorMessage = "Email Is Required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password Is Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Comfirm Password Is Required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password Doesn't Match")]
        public string ComfirmPassword { get; set; }
        [Required(ErrorMessage = "Phone Number Is Required")]
        [Phone]
        public string Phone { get; set; }
        [Required]
        public int YearsOfExperience { get; set; }
        [Required]
        public string SSN { get; set; }
    }
}
