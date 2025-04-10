using Share.Model;

namespace AssignmentService.Repository
{
    public interface IAssignmentRepository : IRepository<Assignment>
    {
        Task<bool> CreateExamAsync();
        Task<bool> CreateQuizAsync();
    }
}
