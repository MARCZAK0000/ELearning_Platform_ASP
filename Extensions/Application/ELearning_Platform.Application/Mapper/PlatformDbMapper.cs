using AutoMapper;
using ELearning_Platform.Domain.Database.Enitities;
using ELearning_Platform.Domain.Models.Models.UserAddress;
using ELearning_Platform.Domain.Models.Response.UserReponse;

namespace ELearning_Platform.Infrastructure.Mapper
{
    public class PlatformDbMapper : Profile
    {
        public PlatformDbMapper()
        {
            CreateMap<UserAddress, UserAddressDto>();

            CreateMap<UserInformations, GetUserInformationsDto>()
                .ForMember(pr => pr.Address, opt => opt.MapFrom(src => src.Address));
        }
    }
}
