namespace ELearning_Platform.Infrastructure.Identity
{
    public class HttpOnlyCookieOptions
    {
        public bool IsHttpOnly { get; set; } = true;

        public string AccessTokenName { get; set; } = "accessToken";

        public string RefreshTokenName { get; set; } = "refreshToken";

        public DateTimeOffset AccessTokenExpireTime { get; set; } = DateTimeOffset.Now.AddMinutes(15);

        public DateTimeOffset RefreshTokenExpireTime { get; set; } = DateTimeOffset.Now.AddHours(1);
    }
}
