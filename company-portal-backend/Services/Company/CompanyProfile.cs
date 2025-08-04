using AutoMapper;
using DAL.Models;
using Services.Company.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Company
{
    public class CompanyProfile : Profile
    {
        public CompanyProfile()
        {
            CreateMap<CompanyRegisterDto, DAL.Models.Company>();

            //CreateMap<DAL.Models.Company, CompanyResponseDto>()
            //    .ForMember(company => company.Name, option => option.MapFrom(c => c.EnglishName)).ReverseMap();


        }
    }
}
