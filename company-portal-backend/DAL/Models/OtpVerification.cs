using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public  class OtpVerification : BaseEntity
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string OtpCode { get; set; }

        public DateTime ExpirationTime { get; set; }
        public bool IsVerified { get; set; } = false;

        public OtpVerification()
        {
            ExpirationTime = DateTime.UtcNow.AddMinutes(5);
            OtpCode = new Random().Next(100000, 999999).ToString();
        }


    }
}
