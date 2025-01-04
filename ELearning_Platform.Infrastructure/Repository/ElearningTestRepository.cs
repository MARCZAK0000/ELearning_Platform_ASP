using ELearning_Platform.Domain.Enitities;
using ELearning_Platform.Domain.Models.ELearningTestModel;
using ELearning_Platform.Domain.Models.Pagination;
using ELearning_Platform.Domain.Repository;
using ELearning_Platform.Domain.Response.ElearningTest;
using ELearning_Platform.Domain.Response.Pagination;
using ELearning_Platform.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace ELearning_Platform.Infrastructure.Repository
{
    public class ELearningTestRepository
        (PlatformDb platformDb) : IElearningTestRepository
    {
        private readonly PlatformDb _platformDb = platformDb;

        public async Task<Test> CreateTestAsync(string teacherID, CreateTestModel createTestModel, CancellationToken token)
        {
            Test test = new();
            using var transaction = await _platformDb.Database.BeginTransactionAsync(token);
            try
            {

                test.SubjectID = createTestModel.SubjectID;
                test.TestName = createTestModel.TestName;
                test.TestLevel = createTestModel.TestLevel;
                test.StartTime = createTestModel.StartTime;
                test.EndTime = createTestModel.EndTime;
                test.Questions = [];
                test.TeacherID = teacherID;

                await _platformDb.Tests.AddAsync(test, token);

                foreach (var item in createTestModel.Questions)
                {
                    var que = new Questions
                    {
                        TestId = test.TestID,
                        QuestionText = item.QuestionText,
                        Answers = []
                    };

                    await _platformDb.Questions.AddAsync(que, token);

                    var answers = item.Answers?.Select(answer => new Answers
                    {
                        AnswerText = answer.AnswerText,
                        QuestionId = que.QuestionId,
                        IsCorrect = answer.IsCorrect,
                    }) ?? [];

                    await _platformDb.Answers.AddRangeAsync(answers, token);
                }

                await transaction.CommitAsync(token);
            }
            catch (Exception err)
            {
                await transaction.RollbackAsync(token);
                throw new Exception(err.Message);
            }
            return test;

        }

        public async Task<Test?> FindTestByIdAsync(string testId, CancellationToken token)
        {

            return await _platformDb.Tests
                .Where(pr => pr.TestID == testId)
                .FirstOrDefaultAsync(token);
        }

        public async Task<Pagination<Test>> FindTestsByTeacherIDAsync(string teacherID,
            bool isComplited, PaginationModelDto paginationModelDto,
            CancellationToken token)
        {
            var pagination = new PaginationBuilder<Test>();
            var findTestBase = _platformDb
                .Tests
                .Where(pr => pr.IsComplited == isComplited && pr.TeacherID.ToString() == teacherID);

            var findCount = await findTestBase.CountAsync(token);

            var result = await findTestBase
                .Skip((paginationModelDto.PageSize * paginationModelDto.PageIndex) + 1)
                .Take(paginationModelDto.PageSize)
                .ToListAsync(token);

            return pagination
                .SetPageSize(paginationModelDto.PageSize)
                .SetPageIndex(paginationModelDto.PageIndex)
                .SetLastIndex(paginationModelDto.PageSize, paginationModelDto.PageIndex)
                .SetItems(result)
                .SetTotalCount(findCount)
                .Build();

        }

        public async Task<Pagination<Test>> FindTestsBySubjectIDAsync(string subjectID, PaginationModelDto paginationModelDto, CancellationToken token)
        {
            var pagination = new PaginationBuilder<Test>();


            var findTestBase = await _platformDb
                .Tests
                .Include(pr => pr.Questions!)
                .ThenInclude(pr => pr.Answers)
                .AsSplitQuery()
                .Where(pr => pr.SubjectID == subjectID)
                .Skip((paginationModelDto.PageSize * paginationModelDto.PageIndex) + 1)
                .Take(paginationModelDto.PageSize)
                .OrderBy(pr => pr.StartTime)
                .ToListAsync(token);

            return pagination
                .SetItems(findTestBase)
                .SetPageSize(paginationModelDto.PageSize)
                .SetPageIndex(paginationModelDto.PageIndex)
                .SetFirstIndex(paginationModelDto.PageSize, paginationModelDto.PageIndex)
                .SetLastIndex(paginationModelDto.PageSize, paginationModelDto.PageIndex)
                .SetTotalCount(findTestBase.Count)
                .Build();

        }

        public async Task<List<UserAnswers>> CommitTestAsync(string userID, string testID, DoTestModelDto testModelDto, CancellationToken token)
        {
            var userAnswers = new List<UserAnswers>();
            foreach (var item in testModelDto.TestQuestions)
            {
                userAnswers.Add(new UserAnswers
                {
                    UserID = userID,
                    TestID = testID,
                    QuestionID = item.QuestionID,
                    AnswerID = item.AnswerID,
                });
            }

            await _platformDb.UserAnswers.AddRangeAsync(userAnswers, token);
            return userAnswers;
        }

        public async Task<int> CheckTestAnswersAsync(string userID, string testID, List<UserAnswers> userAnswers, CancellationToken token)
        {
            var answers = await _platformDb
                .Questions
                .Where(pr => pr.TestId == testID)
                .OrderBy(pr => pr.QuestionId)
                .ToListAsync(token);



            throw new NotImplementedException();
        }
        public async Task<IDictionary<Questions, Answers>> GetTestAsnwersAsync(string userID, string TestID, CancellationToken token)
        {
            var dictionary = new Dictionary<Questions, Answers>();

            var getTest = await _platformDb
                .Tests
                .Where(pr => pr.TestID == TestID)
                .Include(pr => pr.Questions)
                .ToListAsync(token);

            throw new NotImplementedException();

        }
        public Task<TestScoreResponse> CalculateTestScoreAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> CommitTestAsync(string userID, Test test, DoTestModelDto testModelDto, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task<int> CheckTestAnswersAsync()
        {
            throw new NotImplementedException();
        }
    }
}