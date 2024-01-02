using AutoMapper;
using System.Text.Json;
using Tekton_Challenge.Dto;
using Tekton_Challenge.Entity;

namespace Tekton_Challenge.Utilities
{
    public class AutoMapperProductProfile : Profile
    {
        public AutoMapperProductProfile() 
        {
            CreateMap<ProductDto, Product>()
                .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.Status == 1 ? "Active" : "Inactive"))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => (src.ProductId >= 1 && src.ProductId <= 5) ? "Premium": "Classic"))
                .ForMember(dest => dest.RegistrationDate, opt => opt.MapFrom(src =>DateTime.Now.ToString()))
                .ForMember(dest => dest.Customer, opt => opt.MapFrom(src =>"Tekton Evaluator"));
        }
    }
}
