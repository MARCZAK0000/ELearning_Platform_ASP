using ELearning_Platform.Domain.Enitities;
using ELearning_Platform.Domain.Models.SchoolModel;
using ELearning_Platform.Domain.Response.SchoolResponse;
using MediatR;

namespace ELearning_Platform.Application.Services.SchoolServices.Query.GetClassByID
{
    public class GetClassByIDAsyncQuery : GetClassByIDDto, IRequest<ELearingClassDto> 
    {

    }
}
