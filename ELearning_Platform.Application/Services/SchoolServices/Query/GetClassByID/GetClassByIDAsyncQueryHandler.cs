using ELearning_Platform.Domain.Enitities;
using ELearning_Platform.Domain.Exceptions;
using ELearning_Platform.Domain.Repository;
using ELearning_Platform.Domain.Response.SchoolResponse;
using MediatR;

namespace ELearning_Platform.Application.Services.SchoolServices.Query.GetClassByID
{
    public class GetClassByIDAsyncQueryHandler(ISchoolRepository schoolRepository) : IRequestHandler<GetClassByIDAsyncQuery, ELearingClassDto>
    {
        private readonly ISchoolRepository _schoolRepository = schoolRepository;

        public async Task<ELearingClassDto> Handle(GetClassByIDAsyncQuery request, CancellationToken cancellationToken)
        {
            if(request == null || string.IsNullOrEmpty(request.ClassID))
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
