using System.ComponentModel.DataAnnotations;

namespace Demo.PL.Models.UserRegister
{
    public class ClientRegisterViewModel
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
        [RegularExpression("^[0-9]{1,3}-[a-zA-Z]{5,10}-[a-zA-Z]{4,10}-[a-zA-Z]{5,10}$",
            ErrorMessage = "Address must be Like 123-Street-City-Country")]
        public string Address { get; set; }

    }
}
