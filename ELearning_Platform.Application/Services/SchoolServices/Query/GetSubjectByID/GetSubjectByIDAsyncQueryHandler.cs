using ELearning_Platform.Domain.Enitities;
using ELearning_Platform.Domain.Exceptions;
using ELearning_Platform.Domain.Repository;
using ELearning_Platform.Infrastructure.Authorization;
using MediatR;

namespace ELearning_Platform.Application.Services.SchoolServices.Query.GetSubjectByID
{
    public class GetSubjectByIDAsyncQueryHandler(ISchoolRepository schoolRepository) : IRequestHandler<GetSubjectByIDAsyncQuery, Subject>
    {
        private readonly ISchoolRepository _schoolRepository = schoolRepository;
        

        public async Task<Subject> Handle(GetSubjectByIDAsyncQuery request, CancellationToken cancellationToken)
        {
            if(request == null || string.IsNullOrEmpty(request.SubjectID))
            {
                throw new BadRequestException("Validation Problems");
            }
            var subject = await _schoolRepository.FindSubjectByIDAsync(request.SubjectID, cancellationToken)
                ??throw new NotFoundException("Not found subject with this id");
            return subject;
        }
    }
}
