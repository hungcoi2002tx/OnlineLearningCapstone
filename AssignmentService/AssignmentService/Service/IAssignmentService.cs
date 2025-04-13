using Share.DTO;
using Share.Other;
using Share.Other.SearchModel;
using Share.RequestModel;

namespace AssignmentService.Service
{
    public interface IAssignmentService
    {
        Task<ServiceResult> CreateExamAsync(ExamRequestModel assignmentDto);
        Task<ServiceResult> CreateQuizAsync(QuizRequestDto quizDto);
        Task<ServiceResult> DeleteAsync(string id, string teacherId);
        Task<ServiceResult> GetAllByFilterAsync(AssignmentSearch model);
        Task<ServiceResult> UpdateAsync(string id,string teacherId, UpdateAssignmentRequestModel updateModel);
        Task<ServiceResult> UpdateAsync(string id,string teacherId, UpdateExamRequestModel updateModel);
    }
}
