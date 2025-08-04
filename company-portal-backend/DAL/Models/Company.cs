using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Company : BaseEntity
    {

        //public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string ArabicName { get; set; }

        [Required]
        public string EnglishName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Phone(ErrorMessage = "Invalid phone number format.")]
        public string? PhoneNumber { get; set; }

        [Url]
        public string? WebsiteUrl { get; set; }

        public string? LogoPath { get; set; } 
        public string? LogoId { get; set; }

        public string PasswordHash { get; set; } 
        public string PasswordSalt { get; set; } 

    }
}
