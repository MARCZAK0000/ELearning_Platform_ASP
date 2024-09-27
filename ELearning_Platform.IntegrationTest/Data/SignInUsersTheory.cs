using System.Collections;

namespace E_LearningPlatform.IntegrationTest.Data
{
    public class SignInUsersTheory : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { "test1@test.com", "password" };
            yield return new object[] { "test@test.com", "password" };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
