using ELearning_Platform.Domain.Core.Repository;
using ELearning_Platform.Domain.Database.Enitities;
using ELearning_Platform.Domain.Exceptions.Exceptions;
using ELearning_Platform.Domain.Models.CalculateGrade;
using ELearning_Platform.Domain.Models.Models.ELearningTestModel;
using ELearning_Platform.Domain.Models.Models.Pagination;
using ELearning_Platform.Domain.Models.Response.Pagination;
using ELearning_Platform.Infrastructure.Database.Database;
using Microsoft.EntityFrameworkCore;

namespace ELearning_Platform.Infrastructure.Repository
{
    public class ELearningTestRepository
        (PlatformDb platformDb, ICalculateGradeFactory calculateGradeFactory) : IElearningTestRepository
    {
        #region DI Properties

        private readonly PlatformDb _platformDb = platformDb;

        private readonly ICalculateGradeFactory _calculateGradeFactory = calculateGradeFactory;
        #endregion

        #region Commit Methods

        public async Task<Test> CreateTestAsync(string teacherID, CreateTestModel createTestModel, CancellationToken token)
        {
            Test test = new()
            {
                SubjectID = createTestModel.SubjectID,
                TestName = createTestModel.TestName,
                TestLevel = (Domain.Models.SchoolModel.TestLevel)createTestModel.TestLevel,
                StartTime = createTestModel.StartTime,
                EndTime = createTestModel.EndTime,
                Questions = [],
                TeacherID = teacherID
            };

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

            return test;

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

        public async Task<IDictionary<int, int>> CheckTestAnswersAsync(string testID, List<UserAnswers> userAnswers, CancellationToken token)
        {
            //Correct answer for each question
            IDictionary<string, string> keyValuePairs = new Dictionary<string, string>();

            //Find correct Questions and Answers
            var questions = await _platformDb
                .Questions
                .Include(pr => pr.Answers)
                .Where(pr => pr.TestId == testID)
                .OrderBy(pr => pr.QuestionId)
                .ToListAsync(token);

            foreach (var question in questions)
            {
                foreach (var answers in question.Answers.OrderBy(pr => pr.QuestionId))
                {
                    if (answers.IsCorrect)
                    {
                        keyValuePairs.Add(question.QuestionId, answers.AnswerId);
                    }
                }
            }
            IList<bool> correctAnswer = [];
            foreach (var item in userAnswers)
            {

                if (keyValuePairs.Where(pr => pr.Key == item.QuestionID && pr.Value == item.AnswerID).Any())
                    correctAnswer.Add(true);
                else
                    correctAnswer.Add(false);
            }

            return new Dictionary<int, int>()
            {
                { correctAnswer.Count(pr => pr == true) / correctAnswer.Count * 100, correctAnswer.Count }
            };
        }

        #endregion
        public async Task<Test> FindTestByIdAsync(string testId, CancellationToken token)
        {

            return await _platformDb.Tests
                .Where(pr => pr.TestID == testId)
                .FirstOrDefaultAsync(token)
                ?? throw new NotFoundException("Invalid Test Id");
        }

        public async Task<Pagination<Test>> FindTestsByTeacherIDAsync(string teacherID,
            bool isComplited, PaginationModelDto paginationModelDto,
            CancellationToken token)
        {
            var pagination = new PaginationBuilder<Test>();
            var findTestBase = _platformDb
                .Tests
                .Where(pr => pr.IsComplited == isComplited && pr.TeacherID == teacherID);

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


        public async Task<IDictionary<Questions, Answers>> FindUserTestAsnwersAsync(string userID, string TestID, CancellationToken token)
        {
            IDictionary<Questions, Answers> keyValuePairs = new Dictionary<Questions, Answers>();

            var questions = await _platformDb
                 .Questions
                 .Where(pr => pr.TestId == TestID)
                 .OrderBy(pr => pr.QuestionId)
                 .ToListAsync(token);

            var answers = await _platformDb
                 .UserAnswers
                 .Where(pr => pr.TestID == TestID && pr.UserID == userID)
                 .Include(pr => pr.Answers)
                 .OrderBy(pr => pr.QuestionID)
                 .ToListAsync(token);

            foreach (var question in questions)
            {
                foreach (var answer in answers)
                {
                    if (question.QuestionId == answer.QuestionID)
                    {
                        keyValuePairs.Add(question, answer.Answers);
                    }
                }
            }

            return keyValuePairs;

        }

        public async Task<IDictionary<Questions, Answers>> FindCorrectTestAnswersAsync(string testId, CancellationToken token)
        {
            IDictionary<Questions, Answers> keyValuePairs = new Dictionary<Questions, Answers>();

            var questions = await _platformDb
               .Questions
               .Where(pr => pr.TestId == testId)
               .Include(pr => pr.Answers)
               .OrderBy(pr => pr.QuestionId)
               .ToListAsync(token);

            foreach (var question in questions)
            {
                foreach (var answer in question.Answers.OrderBy(pr => pr.QuestionId))
                {
                    if (answer.IsCorrect && question.QuestionId == answer.QuestionId)
                    {
                        keyValuePairs.Add(question, answer);
                    }
                }
            }

            return keyValuePairs;
        }

        public async Task<string> CalculateTestGradeAsync(IDictionary<int, int> score)
            => await Task.FromResult(_calculateGradeFactory.CreateGradeBase().CalculateGrade(score));


        public async Task<bool> UpdateTestGradeAsync(string grade, IDictionary<int, int> score, string userID, string testID, string subjectID, CancellationToken token)
        {
            var grades = new Grade()
            {
                GradeValue = grade,
                TestID = testID,
                SubjectID = subjectID,
                StudentID = userID,
                GradeDetails = new GradeDetails
                {
                    GradePointsScore = score.Keys.FirstOrDefault(),
                    TestQuestionsCount = score.Values.FirstOrDefault(),
                }
            };

            await _platformDb.Grades.AddAsync(grades, token);
            await _platformDb.SaveChangesAsync(token);

            return true;
        }
    }
}