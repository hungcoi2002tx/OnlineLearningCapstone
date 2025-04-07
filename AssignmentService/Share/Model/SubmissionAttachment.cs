using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Model
{
    public class SubmissionAttachment
    {
        public string AttachmentId { get; set; } // Id tệp đính kèm của bài nộp
        public string SubmissionId { get; set; } // Khóa ngoại bài nộp
        public string FileUrl { get; set; } // URL chứa file
        public string FileType { get; set; } // Kiểu file: 'DOCUMENT', 'IMAGE', 'VIDEO', 'LINK'
        public DateTime UploadedAt { get; set; } = DateTime.Now; // Thời gian upload

        // Navigation property
        public AssignmentSubmission Submission { get; set; }
    }
}
