using ELearning_Platform.Domain.Core.Repository;
using ELearning_Platform.Domain.Exceptions.Exceptions;
using ELearning_Platform.Domain.Models.Response.SchoolResponse;
using MediatR;

namespace ELearning_Platform.Application.MediatR.Services.SchoolServices.Query.GetClassByID
{
    public class GetClassByIDAsyncQueryHandler(ISchoolRepository schoolRepository) : IRequestHandler<GetClassByIDAsyncQuery, ELearingClassDto>
    {
        private readonly ISchoolRepository _schoolRepository = schoolRepository;

        public async Task<ELearingClassDto> Handle(GetClassByIDAsyncQuery request, CancellationToken cancellationToken)
        {
            if (request == null || string.IsNullOrEmpty(request.ClassID))
            {
                throw new BadRequestException("Validaton Problems");
            }

            return await _schoolRepository
                .FindInformationsAboutClassByClassIDAsync
                (request.ClassID, request.WithStudents, request.WithSubjects, request.WithTeachers, cancellationToken)
                ?? throw new NotFoundException("Invalid ClassID");
        }
    }
}
