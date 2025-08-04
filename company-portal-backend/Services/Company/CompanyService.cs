using AutoMapper;
using DAL.Models;
using Repository.Helper;
using Repository.Interfaces;
using Services.Company.Dtos;
using Services.EmailService;
using Services.General;
using Services.General.Interface;
using Services.Images;

namespace Services.Company
{
    public class CompanyService : ICompanyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;
        private readonly IJWTService _jWTService;

        public CompanyService(
            IUnitOfWork unitOfWork,
            IImageService imageService,
            IMapper mapper,
            IEmailSender emailSender,
            IJWTService jWTService)
        {
            _unitOfWork = unitOfWork;
            _imageService = imageService;
            _mapper = mapper;
            _emailSender = emailSender;
            _jWTService = jWTService;
        }


        public async Task<string> SendOtpAsync(SendOtpDto model)
        {

            var existingCompanyEmail = await _unitOfWork.GenericRepository<DAL.Models.Company>().FindAsync(company => company.Email == model.Email);
            if (existingCompanyEmail != null)
            {
                throw new ArgumentException("This Email Already Exists");
            }

            if (model.PhoneNumber != null)
            {
                var existingCompanyPhone = await _unitOfWork.GenericRepository<DAL.Models.Company>().FindAsync(company => company.PhoneNumber == model.PhoneNumber);
                if (existingCompanyPhone != null)
                {
                    throw new ArgumentException("This PhoneNumber Already Exists");
                }
            }


            var newOtp = new DAL.Models.OtpVerification()
            {
                Email = model.Email,
            };

            await _unitOfWork.GenericRepository<DAL.Models.OtpVerification>().AddAsync(newOtp);
            await _unitOfWork.CompleteAsync();

            await _emailSender.SendEmailAsync(
                toEmail: model.Email,
                Subject: "Your Company Registration OTP Code - Sent",
                htmlBody: $"<p>Hello <strong>{model.Email}</strong>,</p>" +
                          $"<p>Use the following OTP to complete your registration:</p>" +
                          $"<h2>{newOtp.OtpCode}</h2>" +
                          $"<p>This code will expire at {newOtp.ExpirationTime.ToLocalTime():hh:mm tt}.</p>" +
                          $"<br/><p>Thank you for registering with us!</p>"
            );

            return newOtp.OtpCode;

        }

        public async Task VerifyOtpAsync(VerifyOtpDto request)
        {

            var otpVerification = await _unitOfWork.GenericRepository<DAL.Models.OtpVerification>()
                .FindAsync(otp => otp.Email == request.Email,
                          orderBy: otp => otp.CreatedAt,
                          orderByDirection: OrderBy.Descending);

            if (otpVerification == null)
                throw new ArgumentException("No valid OTP found. Please request a new OTP");

            if (DateTime.UtcNow > otpVerification.ExpirationTime)
                throw new ArgumentException("OTP has expired. Please request a new OTP");

            if (otpVerification.OtpCode != request.Otp)
                throw new ArgumentException("Invalid OTP code");

            otpVerification.IsVerified = true;
            _unitOfWork.GenericRepository<DAL.Models.OtpVerification>().Update(otpVerification);
            await _unitOfWork.CompleteAsync();

        }


        public async Task RegisterCompanyAsync(CompanyRegisterDto request)
        {
            var VerifiedOtp = await _unitOfWork.GenericRepository<OtpVerification>().FindAsync(otp => otp.Email == request.Email && otp.IsVerified);

            if (VerifiedOtp == null)
            {
                throw new ArgumentException("OTP verification required. Please verify your email before registering the company.");
            }

            var existCompany = await _unitOfWork.GenericRepository<DAL.Models.Company>().FindAsync(company => company.Email == request.Email);

            if (existCompany != null)
                throw new ArgumentException("Company with this email Already Exists ");

            var model = _mapper.Map<DAL.Models.Company>(request);

            var (passwordHash, passwordSalt) = PasswordHelper.HashPassword(request.NewPassword);

            model.PasswordHash = passwordHash;
            model.PasswordSalt = passwordSalt;

            if (request.Logo != null)
            {
                var (fileId, url) = await _imageService.UploadImageAsync(request.Logo);
                model.LogoPath = url;
                model.LogoId = fileId;
            }

            await _unitOfWork.GenericRepository<DAL.Models.Company>().AddAsync(model);
            await _unitOfWork.CompleteAsync();
        }


        public async Task<string> LoginAsync(LoginDto request)
        {
            var company = await _unitOfWork.GenericRepository<DAL.Models.Company>()
                .FindAsync(c => c.Email == request.Email);

            if (company == null)
                throw new ArgumentException("Invalid email or password");

            bool isPasswordValid = PasswordHelper.VerifyPassword(request.Password, company.PasswordHash, company.PasswordSalt);

            if (!isPasswordValid)
                throw new ArgumentException("Invalid email or password");

            string token = _jWTService.GenerateToken(company);

            return token;
        }

    }
}
