using ELearning_Platform.Domain.Models.Models.SchoolModel;
using ELearning_Platform.Domain.Models.Response.SchoolResponse;
using MediatR;

namespace ELearning_Platform.Application.MediatR.Services.SchoolServices.Query.GetClassByID
{
    public class GetClassByIDAsyncQuery : GetClassByIDDto, IRequest<ELearingClassDto>
    {

    }
}
