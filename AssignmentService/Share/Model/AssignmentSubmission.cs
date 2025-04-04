﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Model
{
    // Represents the "assignment_submissions" table
    public class AssignmentSubmission
    {
        public Guid SubmissionId { get; set; } = Guid.NewGuid(); // Id bài nộp
        public Guid AssignmentId { get; set; } // Khóa ngoại assignment
        public Guid StudentId { get; set; } // Id sinh viên
        public DateTime SubmittedAt { get; set; } = DateTime.Now; // Thời gian nộp bài
        public string Content { get; set; } // Nội dung bài nộp (văn bản)
        public string Status { get; set; } = "SUBMITTED"; // Trạng thái: 'SUBMITTED', 'GRADED', 'REVIEWED'
        public float? Grade { get; set; } // Điểm số nếu đã chấm
        public string Feedback { get; set; } // Nhận xét từ giáo viên

        // Navigation properties
        public Assignment Assignment { get; set; }
        public ICollection<SubmissionAttachment> Attachments { get; set; }
    }
}
