using ELearning_Platform.Domain.Enitities;
using ELearning_Platform.Domain.Models.Pagination;
using ELearning_Platform.Domain.Repository;
using ELearning_Platform.Domain.Response.Pagination;
using ELearning_Platform.Infrastructure.Authorization;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ELearning_Platform.Application.Services.SchoolServices.Query.LessonBySubjects
{
    public class GetLessonsBySubjectIDQueryHandler(IValidator<PaginationModelDto> paginationValidator, 
        IUserContext userContext, 
        ISchoolRepository schoolRepository) : IRequestHandler<GetLessonsBySubjectIDQuery, Results<Ok<Pagination<Lesson>>, ForbidHttpResult>>
    {
        private readonly IValidator<PaginationModelDto> _paginationValidator = paginationValidator;

        private readonly IUserContext _userContext = userContext;

        private readonly ISchoolRepository _schoolRepository = schoolRepository;

        public Task<Results<Ok<Pagination<Lesson>>, ForbidHttpResult>> Handle(GetLessonsBySubjectIDQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
