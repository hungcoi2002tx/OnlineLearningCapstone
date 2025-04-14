using Share.Model;

namespace AssignmentService.Repository
{
    public interface ISubmissionRepository : IRepository<AssignmentSubmission>
    {
        Task<bool> CreateAsync(AssignmentSubmission submission);
    }
}
