using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Services.Company.Dtos
{
    public class SendOtpDto
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Please provide a valid email address")]
        public string Email { get; set; }
        [Phone(ErrorMessage = "Invalid phone number format.")]
        public string? PhoneNumber { get; set; }
    }
}
