using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Company.Dtos
{ 
    public class VerifyOtpDto
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Please provide a valid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "OTP is required")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "OTP must be exactly 6 digits")]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "OTP must be exactly 6 digits")]
        public string Otp { get; set; }
    }
}
