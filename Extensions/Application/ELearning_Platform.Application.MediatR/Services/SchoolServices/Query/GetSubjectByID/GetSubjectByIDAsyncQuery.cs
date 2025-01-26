using ELearning_Platform.Domain.Database.Enitities;
using ELearning_Platform.Domain.Models.Models.SchoolModel;
using MediatR;

namespace ELearning_Platform.Application.MediatR.Services.SchoolServices.Query.GetSubjectByID
{
    public class GetSubjectByIDAsyncQuery : GetSubjectByIDDto, IRequest<Subject>
    {

    }
}
