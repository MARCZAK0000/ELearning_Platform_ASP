using System.Collections;

namespace E_LearningPlatform.IntegrationTest.Data
{
    public class RegisterUserTheory : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { "test3@test.com", "password", "333444555" };
            yield return new object[] { "test4@test.com", "password", "444444555" };
            yield return new object[] { "test5@test.com", "password", "555444555" };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
