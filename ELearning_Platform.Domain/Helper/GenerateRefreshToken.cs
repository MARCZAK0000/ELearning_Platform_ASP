namespace ELearning_Platform.Domain.Helper
{
    public static class GenerateRefreshToken
    {
        private readonly static string _chars = "abcdefghijklmnopqrstuvwxyz0123456789@#$%";

        public static Task<string> GenerateToken()
            => Task.FromResult(new string(Enumerable.Repeat(_chars, 12).Select(s => s[Random.Shared.Next(_chars.Length)]).ToArray()));

            
    }
}
