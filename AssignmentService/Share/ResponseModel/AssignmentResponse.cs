﻿using Share.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.ResponseModel
{
    public class AssignmentResponse
    {
        public string? AssignmentId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime Deadline { get; set; }
        public string? TeacherId { get; set; }
        public string? ClassroomId { get; set; }
        public string? Status { get; set; }
        public string? Type { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<QuestionDto>? Questions { get; set; }
        public List<AssignmentAttachmentDto>? Attachments { get; set; }
    }
}
