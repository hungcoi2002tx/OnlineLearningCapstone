using Share.Other;
using Share.Other.SearchModel;
using Share.RequestModel;

namespace AssignmentService.Service
{
    public interface ISubmissionService
    {
        Task<ServiceResult> CreateExamSubmissionAsync(CreateExamSubmissionRequest model);
        Task<ServiceResult> CreateQuizSubmissionAsync(CreateQuizSubmissionRequest model);
        Task<ServiceResult> GetAllBySearch(SubmissionSearch search);
        Task<byte[]> GetFileAsync(SubmissionSearch search);
        Task<ServiceResult> GradeAsync(GradeRequestModel model);
    }
}
