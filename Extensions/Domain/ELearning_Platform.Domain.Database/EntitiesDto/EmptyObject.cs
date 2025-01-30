namespace ELearning_Platform.Domain.Database.EntitiesDto
{
    public static class EmptyObject<T> where T : class, new()
    {
        public static T Empty()
        {
            return new T();
        }
    }
}
