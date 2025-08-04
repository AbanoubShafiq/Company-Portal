
using DAL.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Repository.Implementation;
using Repository.Interfaces;
using Services.Company;
using Services.EmailService;
using Services.General;
using Services.General.Interface;
using Services.Images;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            // connection string 
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            var angularDevOrigin = builder.Configuration["AllowedOrigins:AngularDev"];

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAngularDevClient", policy =>
                {
                    policy.WithOrigins(angularDevOrigin)
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowCredentials();
                });
            });



            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddSingleton<IImageService, ImageKitService>();
            builder.Services.AddScoped<IEmailSender, SmtpEmailSender>();
            builder.Services.AddSingleton<IJWTService, JWTService>();

            builder.Services.AddScoped<ICompanyService, CompanyService>();

            builder.Services.AddAutoMapper(typeof(CompanyProfile));

            var app = builder.Build();

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseCors("AllowAngularDevClient");


            app.MapControllers();

            app.Run();
        }
    }
}
