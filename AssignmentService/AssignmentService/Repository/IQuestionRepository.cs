using Share.Model;
using Share.Other.SearchModel;
using Share.RequestModel;

namespace AssignmentService.Repository
{
    public interface IQuestionRepository : IRepository<Question>
    {
        Task<bool> CreateAsync(Question model);
        Task<List<Question>> GetAllByFilterAsync(QuestionSearch model);
        Task<bool> UpdateAsync(Question question, UpdateQuestionRequestModel model);
    }
}
