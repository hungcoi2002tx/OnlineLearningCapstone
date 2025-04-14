using Share.Other;
using Share.RequestModel;

namespace AssignmentService.Service
{
    public interface ISubmissionService
    {
        Task<ServiceResult> CreateQuizSubmissionAsync(CreateQuizSubmissionRequest model);
    }
}
