using Share.DTO;
using Share.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.ResponseModel
{
    public class AssignmentSubmissionResponse
    {
        public string? SubmissionId { get; set; } // Id bài nộp
        public string? AssignmentId { get; set; } // Khóa ngoại assignment
        public string? StudentId { get; set; } // Id sinh viên
        public DateTime SubmittedAt { get; set; } = DateTime.Now; // Thời gian nộp bài
        public string? Content { get; set; } // Nội dung bài nộp (văn bản)
        public string? Status { get; set; } // Trạng thái: 'SUBMITTED', 'GRADED', 'REVIEWED'
        public float? Grade { get; set; } // Điểm số nếu đã chấm
        public string? Feedback { get; set; } // Nhận xét từ giáo viên
        public string? QuizAnswer { get; set; } // Đáp án quiz lưu kiểu json

        // Navigation properties
        public List<SubmissionAttachmentDto>? Attachments { get; set; }
    }
}
