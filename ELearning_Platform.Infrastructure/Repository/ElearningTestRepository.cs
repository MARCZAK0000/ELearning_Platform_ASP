using ELearning_Platform.Domain.Enitities;
using ELearning_Platform.Domain.Exceptions;
using ELearning_Platform.Domain.Models.ELearningTestModel;
using ELearning_Platform.Domain.Repository;
using ELearning_Platform.Infrastructure.Database;

namespace ELearning_Platform.Infrastructure.Repository
{
    public class ELearningTestRepository
        (PlatformDb platformDb): IElearningTestRepository
    {
        private readonly PlatformDb _platformDb = platformDb;

        public async Task<Test> CreateTestAsync(string teacherID, CreateTestModel createTestModel, CancellationToken token)
        {
            Test test = new();
            using var transaction = await _platformDb.Database.BeginTransactionAsync(token);
            try
            {
                if (!Guid.TryParse(createTestModel.SubjectID, out var subjectID))
                {
                    throw new BadRequestException("wrong subject ID");
                }

                test.SubjectID = subjectID;
                test.TestName = createTestModel.TestName;
                test.TestLevel = createTestModel.TestLevel;
                test.StartTime = createTestModel.StartTime;
                test.EndTime = createTestModel.EndTime;
                test.Questions = [];
                
                await _platformDb.Tests.AddAsync(test, token);

                foreach (var item in createTestModel.Questions)
                {
                    var que = new Questions
                    {
                        TestId = test.TestID,
                        QuestionText = item.QuestionText,
                        CorrectAnswerIndex = item.CorrectAnswerIndex,
                        Answers = []
                    };

                    await _platformDb.Questions.AddAsync(que, token);

                    var answers = item.Answers?.Select(answer => new Answers
                    {
                        AnswerId = Guid.NewGuid(),
                        AnswerText = answer.AnswerText,
                        QuestionId = que.QuestionId
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

        public Task<Test> FindTestByIdAsync(string testId, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}