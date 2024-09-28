using AutoMapper;
using ELearning_Platform.Domain.Repository;
using ELearning_Platform.Domain.Response.Pagination;
using ELearning_Platform.Domain.Response.UserReponse;
using ELearning_Platform.Infrastructure.Services.UserServices.Queries.AllUserInformations;
using MediatR;

namespace ELearning_Platform.Application.Services.UserServices.Queries.AllUserInformations
{
    public class GetInfromationsAboutAllUsersAsyncQueryHandler(IMapper mapper, IUserRepository userRepository) :
        IRequestHandler<GetInfromationsAboutAllUsersAsyncQuery, Pagination<GetUserInformationsDto>>
    {
        private readonly IMapper _mapper = mapper;

        private readonly IUserRepository _userRepository = userRepository;

        public async Task<Pagination<GetUserInformationsDto>> Handle(GetInfromationsAboutAllUsersAsyncQuery request, CancellationToken cancellationToken)
            => await _userRepository.GetAllUsersAsync(request, cancellationToken);
    }
}
