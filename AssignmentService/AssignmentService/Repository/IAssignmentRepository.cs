using Share.Model;
using Share.Other.SearchModel;

namespace AssignmentService.Repository
{
    public interface IAssignmentRepository : IRepository<Assignment>
    {
        Task<bool> CreateAssignmentAsync(Assignment assignment);
        Task<(List<Assignment>, int)> GetAllByFilterAsync(AssignmentSearch model);
        Task<bool> UpdateAssignment(Assignment assignment);
    }
}
