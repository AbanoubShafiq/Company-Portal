using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Company;
using Services.Company.Dtos;
using Services.General;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;

        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }


        [HttpPost("Send-otp")]
        public async Task<ActionResult<ResponseDto<string>>> SendOtp([FromBody] SendOtpDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseDto<string>
                {
                    Data = null,
                    IsSuccess = false,
                    Message = "Validation failed.",
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }

            var otp = await _companyService.SendOtpAsync(request);

            return Ok(new ResponseDto<string>
            {
                Data = otp,
                IsSuccess = true,
                Message = $"this OTP: {otp} sent to your email.",
                StatusCode = StatusCodes.Status200OK
            });
        }


        [HttpPost("Register")]
        public async Task<ActionResult<ResponseDto<string>>> Register([FromForm] CompanyRegisterDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseDto<string>
                {
                    Data = null,
                    IsSuccess = false,
                    Message = "Validation failed.",
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }

            await _companyService.RegisterCompanyAsync(request);

            return Ok(new ResponseDto<string>
            {
                Data = null,
                IsSuccess = true,
                Message = "Company registered successfully.",
                StatusCode = StatusCodes.Status200OK
            });
        }


        [HttpPost("verify-otp")]
        public async Task<ActionResult<ResponseDto<string>>> VerifyOtp([FromBody] VerifyOtpDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseDto<string>
                {
                    Data = null,
                    IsSuccess = false,
                    Message = "Validation failed.",
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }

            await _companyService.VerifyOtpAsync(request);

            return Ok(new ResponseDto<string>
            {
                Data = null,
                IsSuccess = true,
                Message = "OTP verified successfully. You can now set your password.",
                StatusCode = StatusCodes.Status200OK
            });
        }


        [HttpPost("login")]
        public async Task<ActionResult<ResponseDto<string>>> Login([FromBody] LoginDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseDto<string>
                {
                    Data = null,
                    IsSuccess = false,
                    Message = "Validation failed.",
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }

            var result = await _companyService.LoginAsync(request);

            return Ok(new ResponseDto<string>
            {
                Data = result,
                IsSuccess = true,
                Message = "Login successful",
                StatusCode = StatusCodes.Status200OK
            });
        }

    }
}
