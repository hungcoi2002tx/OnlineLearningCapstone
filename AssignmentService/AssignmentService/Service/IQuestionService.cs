using Share.Other.SearchModel;
using Share.Other;
using Share.RequestModel;

namespace AssignmentService.Service
{
    public interface IQuestionService
    {
        Task<ServiceResult> CreateAsync(string assignmentId, CreateQuestionRequestModel question);
        Task<ServiceResult> UpdateAsync(string id, UpdateQuestionRequestModel model);
        Task<ServiceResult> GetAllByFilterAsync(QuestionSearch model);
        //Task<ServiceResult> DeleteAsync(string id);
    }
}
