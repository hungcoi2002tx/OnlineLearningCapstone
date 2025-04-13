using Share.Other.SearchModel;
using Share.Other;
using Share.RequestModel;

namespace AssignmentService.Service
{
    public interface IQuestionService
    {
        Task<ServiceResult> CreateAsync(string assignmentId, CreateQuestionRequestModel question);
        //Task<ServiceResult> DeleteAsync(string id);
        //Task<ServiceResult> GetAllByFilterAsync(AssignmentSearch model);
        //Task<ServiceResult> UpdateAsync(string id, string teacherId, UpdateAssignmentRequestModel updateModel);
    }
}
