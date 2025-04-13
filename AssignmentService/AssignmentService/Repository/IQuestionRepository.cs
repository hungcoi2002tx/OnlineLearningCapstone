using Share.Model;

namespace AssignmentService.Repository
{
    public interface IQuestionRepository : IRepository<Question>
    {
        Task<bool> CreateAsync(Question model);
    }
}
