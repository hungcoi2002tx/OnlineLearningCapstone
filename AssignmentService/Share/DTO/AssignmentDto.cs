using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.DTO
{
    public class AssignmentDto
    {
        public string AssignmentId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Deadline { get; set; }
        public string TeacherId { get; set; }
        public string ClassroomId { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Các collection ở đây là DTO, không chứa tham chiếu ngược lại
        // Collections are DTOs that do not reference back to the parent
        public List<AssignmentAttachmentDto> Attachments { get; set; }
        public List<AssignmentCommentDto> Comments { get; set; }
        public List<AssignmentSubmissionDto> Submissions { get; set; }
    }
}
