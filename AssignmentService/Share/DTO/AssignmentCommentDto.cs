using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.DTO
{
    public class AssignmentCommentDto
    {
        public string CommentId { get; set; }
        public string AssignmentId { get; set; }
        public string UserId { get; set; }
        public string Content { get; set; }
        public string? ParentCommentId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Nếu cần thể hiện dạng cây comment, ta có thể thêm collection cho các reply,
        // nhưng cần chú ý rằng nếu reply chứa lại cha thì sẽ tạo vòng lặp. Ở đây, ta chỉ include các reply dưới dạng DTO.
        // If you need to display a comment tree, you can add a collection for replies.
        // However, note that including parent objects in replies may create cycles. Here, we only include replies as DTO.
        public List<AssignmentCommentDto> Replies { get; set; }
    }
}
