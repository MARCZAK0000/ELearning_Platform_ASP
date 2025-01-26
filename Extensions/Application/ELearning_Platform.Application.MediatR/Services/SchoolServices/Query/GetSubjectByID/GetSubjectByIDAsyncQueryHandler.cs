using ELearning_Platform.Domain.Core.Repository;
using ELearning_Platform.Domain.Database.Enitities;
using ELearning_Platform.Domain.Exceptions.Exceptions;
using MediatR;

namespace ELearning_Platform.Application.MediatR.Services.SchoolServices.Query.GetSubjectByID
{
    public class GetSubjectByIDAsyncQueryHandler(ISchoolRepository schoolRepository) : IRequestHandler<GetSubjectByIDAsyncQuery, Subject>
    {
        private readonly ISchoolRepository _schoolRepository = schoolRepository;


        public async Task<Subject> Handle(GetSubjectByIDAsyncQuery request, CancellationToken cancellationToken)
        {
            if (request == null || string.IsNullOrEmpty(request.SubjectID))
            {
                throw new BadRequestException("Validation Problems");
            }
            var subject = await _schoolRepository.FindSubjectByIDAsync(request.SubjectID, cancellationToken)
                ?? throw new NotFoundException("Not found subject with this id");
            return subject;
        }
    }
}
