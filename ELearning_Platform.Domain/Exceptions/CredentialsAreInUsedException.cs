namespace ELearning_Platform.Domain.Exceptions
{
    public class CredentialsAreInUsedException(string? message) : Exception(message)
    {
    }
}
