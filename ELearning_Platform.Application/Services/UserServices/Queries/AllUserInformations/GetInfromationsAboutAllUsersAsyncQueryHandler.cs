using AutoMapper;
using ELearning_Platform.Domain.Repository;
using ELearning_Platform.Domain.Response.UserReponse;
using MediatR;

namespace ELearning_Platform.Infrastructure.Services.UserServices.Queries.AllUserInformations
{
    public class GetInfromationsAboutAllUsersAsyncQueryHandler(IMapper mapper, IUserRepository userRepository) : IRequestHandler<GetInfromationsAboutAllUsersAsyncQuery, List<GetUserInformationsDto>>
    {
        private readonly IMapper _mapper = mapper;

        private readonly IUserRepository _userRepository = userRepository;

        public async Task<List<GetUserInformationsDto>> Handle(GetInfromationsAboutAllUsersAsyncQuery request, CancellationToken cancellationToken)
            => _mapper.Map<List<GetUserInformationsDto>>(await _userRepository.GetAllUsersAsync(token: cancellationToken));
    }
}
