namespace ELearning_Platform.Domain.Exceptions.Exceptions
{
    public class CredentialsAreInUsedException(string? message) : Exception(message)
    {
    }
}
