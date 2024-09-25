namespace ELearning_Platform.Infrastructure.Authorization
{
    public interface IUserContext
    {
        CurrentUser GetCurrentUser();
    }
}
