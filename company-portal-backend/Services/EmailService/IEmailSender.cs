using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.EmailService
{
    public interface IEmailSender
    {
        public Task SendEmailAsync(string toEmail, string Subject, string htmlBody);

    }
}
