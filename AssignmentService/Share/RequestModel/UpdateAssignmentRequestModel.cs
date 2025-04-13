using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.RequestModel
{
    public class UpdateAssignmentRequestModel
    {
        public string Title { get; set; } // Tiêu đề
        public string Description { get; set; } // Mô tả
        public DateTime Deadline { get; set; } // Hạn nộp bài
        public string Status { get; set; } // 'DRAFT', 'PUBLISHED', 'CLOSED'
    }
}
