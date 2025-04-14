using Share.Model;
using Share.Other.SearchModel;

namespace AssignmentService.Repository
{
    public interface ISubmissionRepository : IRepository<AssignmentSubmission>
    {
        Task<bool> CreateAsync(AssignmentSubmission submission);
        Task<(List<AssignmentSubmission>,int)> GetAllByFilterAsync(SubmissionSearch search);
        Task<bool> UpdateAsync(AssignmentSubmission dataSubmission);
    }
}
