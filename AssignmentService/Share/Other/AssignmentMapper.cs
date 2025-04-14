using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Share.DTO;
using Share.Model;
using Share.ResponseModel;

namespace Share.Other
{
    public static class AssignmentMapper
    {
        public static AssignmentResponse MapToAssignmentResponse(Assignment assignment)
        {
            if (assignment == null) return null;

            return new AssignmentResponse
            {
                AssignmentId = assignment.AssignmentId,
                Title = assignment.Title,
                Description = assignment.Description,
                Deadline = assignment.Deadline,
                TeacherId = assignment.TeacherId,
                ClassroomId = assignment.ClassroomId,
                Status = assignment.Status,
                Type = assignment.Type,

                //Attachments = assignment.Attachments?.Select(a => new AssignmentAttachmentDto
                //{
                //    AttachmentId = a.AttachmentId,
                //    AssignmentId = a.AssignmentId,
                //    FileUrl = a.FileUrl,
                //    FileType = a.FileType,
                //    UploadedAt = a.UploadedAt
                //}).ToList(),

                //Questions = assignment.Questions?.Select(q => new QuestionDto
                //{
                //    Id = q.Id,
                //    Content = q.Content,
                //    AssignmentId = q.AssignmentId,
                //    Answers = q.Answers?.Select(ans => new AnswerDto
                //    {
                //        Id = ans.Id,
                //        Content = ans.Content,
                //        IsCorrect = ans.IsCorrect
                //    }).ToList()
                //}).ToList()
            };
        }

        public static List<AssignmentResponse> MapToAssignmentResponseList(List<Assignment> assignments)
        {
            return assignments.Select(MapToAssignmentResponse).ToList();
        }
    }

}
