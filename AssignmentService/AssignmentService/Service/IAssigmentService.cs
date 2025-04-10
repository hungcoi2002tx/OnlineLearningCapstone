using Share.DTO;
using Share.Other;
using Share.RequestModel;

namespace AssignmentService.Service
{
    public interface IAssigmentService
    {
        Task<ServiceResult> CreateExamAsync(ExamRequestModel assignmentDto);
    }
}
