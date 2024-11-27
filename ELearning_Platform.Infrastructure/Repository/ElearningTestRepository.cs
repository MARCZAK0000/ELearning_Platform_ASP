using ELearning_Platform.Domain.Enitities;
using ELearning_Platform.Domain.Exceptions;
using ELearning_Platform.Domain.Models.ELearningTestModel;
using ELearning_Platform.Domain.Models.Pagination;
using ELearning_Platform.Domain.Repository;
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
                        AnswerId = Guid.NewGuid(),
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

        public async Task<Test> FindTestByIdAsync(string testId, CancellationToken token)
        {
            if (!Guid.TryParse(testId, out var test)) throw new BadRequestException("Invalid TestID");

            return await _platformDb.Tests
                .Where(pr => pr.TestID == test)
                .FirstOrDefaultAsync(token)
                ??
                throw new NotFoundException("Test Not Found");
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
            if (Guid.TryParse(subjectID, out var subject))
            {
                throw new BadRequestException("Invalid GUID");
            }

            var findTestBase = await _platformDb
                .Tests
                .Include(pr=>pr.Questions!)
                .ThenInclude(pr=>pr.Answers)
                .AsSplitQuery()
                .Where(pr => pr.SubjectID == subject)
                .Skip((paginationModelDto.PageSize*paginationModelDto.PageIndex)+1)
                .Take(paginationModelDto.PageSize)
                .OrderBy(pr=>pr.StartTime)
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
    }
}