using ELearning_Platform.Domain.Enitities;
using ELearning_Platform.Domain.Models.SchoolModel;
using MediatR;

namespace ELearning_Platform.Application.Services.SchoolServices.Query.GetSubjectByID
{
    public class GetSubjectByIDAsyncQuery : GetSubjectByIDDto, IRequest<Subject>
    {

    }
}
