using api.Models;
using api.Models.DTOs.CustomerDTOs;
using AutoMapper;

namespace api.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Customer, CustomerDTO>().ReverseMap();
            CreateMap<CustomerCreateRequest, Customer>().ReverseMap();
            //CreateMap<CustomerUpdateRequest, Customer>()
            //    .ForAllMembers(opts => opts.PreCondition((src, dest, srcMember) => srcMember != null));
            CreateMap<CustomerUpdateRequest, Customer>()
                .ForMember(dest => dest.Name, opt => opt.Condition(src => src.Name != null))
                .ForMember(dest => dest.Birthdate, opt => opt.Condition(src => src.Birthdate.HasValue))
                .ForMember(dest => dest.Incomes, opt => opt.Condition(src => src.Incomes.HasValue));

        }
    }
}
