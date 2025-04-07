using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Model
{
    public class Assignment
    {
        public string AssignmentId { get; set; }
        public string Title { get; set; } // Tiêu đề
        public string Description { get; set; } // Mô tả
        public DateTime Deadline { get; set; } // Hạn nộp bài
        public string TeacherId { get; set; } // Id Teacher
        public string ClassroomId { get; set; } // Id Class
        public string Status { get; set; } // 'DRAFT', 'PUBLISHED', 'CLOSED'
        public string Type { get;set; }  // 'ASSIGNMENT', 'QUIZ', 'EXAM'
        public DateTime CreatedAt { get; set; } = DateTime.Now; // Thời gian tạo
        public DateTime UpdatedAt { get; set; } = DateTime.Now; // Thời gian update

        // Navigation properties
        public ICollection<AssignmentAttachment> Attachments { get; set; }
        public ICollection<AssignmentComment> Comments { get; set; }
        public ICollection<AssignmentSubmission> Submissions { get; set; }
        public ICollection<Question>? Questions { get; set; }
    }
}
