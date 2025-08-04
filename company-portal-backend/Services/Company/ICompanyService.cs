using Services.Company.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Company
{
    public interface ICompanyService
    {
        public Task RegisterCompanyAsync(CompanyRegisterDto request);
        public Task VerifyOtpAsync(VerifyOtpDto request);
        public Task<string> LoginAsync(LoginDto request);
        public Task<string> SendOtpAsync(SendOtpDto model);
    }
}
