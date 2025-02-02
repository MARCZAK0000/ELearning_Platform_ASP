﻿using ELearning_Platform.Domain.Database.Enitities;
using ELearning_Platform.Domain.Models.Models.ELearningTestModel;
using ELearning_Platform.Domain.Models.Models.Pagination;
using ELearning_Platform.Domain.Models.Response.Pagination;

namespace ELearning_Platform.Domain.Core.Repository
{
    public interface IElearningTestRepository
    {
        Task<Test> CreateTestAsync(string teacherID, CreateTestModel createTestModel, CancellationToken token);

        Task<Test> FindTestByIdAsync(string testId, CancellationToken token);

        Task<Pagination<Test>> FindTestsByTeacherIDAsync(string teacherID, bool isComplited, PaginationModelDto paginationModelDto, CancellationToken token);

        Task<Pagination<Test>> FindTestsBySubjectIDAsync(string subjectID, PaginationModelDto paginationModelDto, CancellationToken token);

        Task<List<UserAnswers>> CommitTestAsync(string userID, string testID, DoTestModelDto testModelDto, CancellationToken token);

        Task<IDictionary<int, int>> CheckTestAnswersAsync(string testID, List<UserAnswers> userAnswers, CancellationToken token);

        Task<string> CalculateTestGradeAsync(IDictionary<int, int> score);

        Task<bool> UpdateTestGradeAsync(string grade, IDictionary<int, int> score, string userID, string testID, string subjectID, CancellationToken token);

        //Task<>
    }
}
