using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Model
{
    public class AssignmentComment
    {
        public string CommentId { get; set; } // Id comment
        public string AssignmentId { get; set; } // Khóa ngoại assignment
        public string UserId { get; set; } // Id người dùng (học sinh, giáo viên)
        public string Content { get; set; } // Nội dung comment
        public string? ParentCommentId { get; set; } // Cmt cha nếu có
        public DateTime CreatedAt { get; set; } = DateTime.Now; // Thời gian tạo
        public DateTime UpdatedAt { get; set; } = DateTime.Now; // Thời gian cập nhật

        // Navigation properties
        public Assignment Assignment { get; set; }
        public AssignmentComment ParentComment { get; set; }
        public ICollection<AssignmentComment> Replies { get; set; }
    }
}
