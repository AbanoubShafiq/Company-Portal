using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Services.Company.Dtos
{
    public class CompanyRegisterDto
    {
        [Required(ErrorMessage = "Company Arabic Name is required.")]
        public string ArabicName { get; set; }

        [Required(ErrorMessage = "Company English Name is required.")]
        public string EnglishName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Phone(ErrorMessage = "Invalid phone number format.")]
        public string? PhoneNumber { get; set; }

        [Url(ErrorMessage = "Invalid website URL format.")]
        public string? WebsiteUrl { get; set; }
        public IFormFile? Logo { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&#]).{7,}$",
        ErrorMessage = "Password must be more than 6 characters and contain at least one uppercase letter, one number, and one special character.")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirm password is required")]
        [Compare("NewPassword", ErrorMessage = "Password and confirmation password do not match")]
        public string ConfirmPassword { get; set; }

    }
}
