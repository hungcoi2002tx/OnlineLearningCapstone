﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.DTO
{
    public class AssignmentSubmissionDto
    {
        public string SubmissionId { get; set; }
        public string AssignmentId { get; set; }
        public string StudentId { get; set; }
        public DateTime SubmittedAt { get; set; }
        public string? Content { get; set; }
        public string? Status { get; set; }
        public float? Grade { get; set; }
        public string? Feedback { get; set; }

        public List<SubmissionAttachmentDto> Attachments { get; set; }
    }
}
