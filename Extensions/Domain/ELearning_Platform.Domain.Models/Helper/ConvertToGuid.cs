using ELearning_Platform.Domain.Exceptions.Exceptions;

namespace ELearning_Platform.Domain.Models.Helper
{
    public static class ConvertToGuid
    {
        public static Guid ToGuid(string str)
        {
            if (!Guid.TryParse(str, out var result))
            {
                throw new BadRequestException("Invalid ID");
            }
            return result;
        }
    }
}
