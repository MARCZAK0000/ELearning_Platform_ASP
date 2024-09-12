using AutoMapper;
using ELearning_Platform.Domain.Enitities;
using ELearning_Platform.Domain.Repository;
using ELearning_Platform.Domain.Response.UserReponse;
using MediatR;

namespace ELearning_Platform.Application.Services.UserServices.Queries.Informations
{
    public class GetUserInformationsAsyncQueryHandler
        (IMapper mapper, IUserRepository userRepository) : IRequestHandler<GetUserInformationsAsyncQuery, GetUserInformationsDto>
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<GetUserInformationsDto> Handle(GetUserInformationsAsyncQuery request, CancellationToken cancellationToken)
            => _mapper.Map<GetUserInformationsDto>(source: await _userRepository.GetUserInformationsAsync(token: cancellationToken));
    }
}
