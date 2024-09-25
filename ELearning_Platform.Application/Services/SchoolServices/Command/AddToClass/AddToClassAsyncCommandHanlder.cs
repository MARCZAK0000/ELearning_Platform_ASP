using ELearning_Platform.Infrastructure.Authorization;
using ELearning_Platform.Domain.Repository;
using ELearning_Platform.Domain.Response.ClassResponse;
using MediatR;

namespace ELearning_Platform.Infrastructure.Services.SchoolServices.Command.AddToClass
{
    public class AddToClassAsyncCommandHanlder(ISchoolRepository schoolRepository) 
        : IRequestHandler<AddToClassAsyncCommand, AddStudentToClassResponse>
    {
        private readonly ISchoolRepository _schoolRepository = schoolRepository;

        public async Task<AddStudentToClassResponse> Handle(AddToClassAsyncCommand request, CancellationToken cancellationToken)
            => await _schoolRepository.AddStudentToClassAsync(request, cancellationToken);
    }
}
