using Microsoft.AspNetCore.Http;


namespace Demo.PL.Models
{
    public class EditProfileViewModel
    {
        public string FirstName{ get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string LinkedInUrl { get; set; }

        public IFormFile ProfileImage { get; set; }  // For uploading the new profile image
        public string ImagePath { get; set; }
    }
}
