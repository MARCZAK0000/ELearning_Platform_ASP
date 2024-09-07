using ELearning_Platform.Application.Authorization;
using ELearning_Platform.Domain.Repository;
using ELearning_Platform.Domain.Response.ClassResponse;
using MediatR;

namespace ELearning_Platform.Application.Services.SchoolServices.Command.CreateClass
{
    public class CreateClassAsyncCommandHandler(ISchoolRepository schoolRepository)
        : IRequestHandler<CreateClassAsyncCommand, CreateClassResponse>
    {
        private readonly ISchoolRepository _schoolRepository = schoolRepository;

        public async Task<CreateClassResponse> 
            Handle(CreateClassAsyncCommand request, CancellationToken cancellationToken) => 
            await _schoolRepository.CreateClassAsync(request, cancellationToken);
        
    }
}
