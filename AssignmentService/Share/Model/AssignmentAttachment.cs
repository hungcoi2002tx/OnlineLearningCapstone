﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Model
{
    public class AssignmentAttachment
    {
        public string AttachmentId { get; set; }// Id tệp đính kèm
        public string AssignmentId { get; set; } // Khóa ngoại assignment
        public string FileUrl { get; set; } // URL chứa file
        public string FileType { get; set; } // Kiểu file: 'DOCUMENT', 'IMAGE', 'VIDEO', 'LINK'
        public DateTime UploadedAt { get; set; } = DateTime.Now; // Thời gian upload

        // Navigation property
        public Assignment Assignment { get; set; }
    }
}
