using AutoMapper;
using ELearning_Platform.Domain.Enitities;
using ELearning_Platform.Domain.Models.UserAddress;
using ELearning_Platform.Domain.Response.UserReponse;

namespace ELearning_Platform.Application.Mapper
{
    public class PlatformDbMapper : Profile
    {
        public PlatformDbMapper()
        {
            CreateMap<UserAddress, UserAddressDto>();

            CreateMap<UserInformations, GetUserInformationsDto>()
                .ForMember(pr=>pr.Address, opt=>opt.MapFrom(src=>src.Address));
        }
    }
}
