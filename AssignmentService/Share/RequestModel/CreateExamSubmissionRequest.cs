using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.RequestModel
{
    public class CreateExamSubmissionRequest
    {
        public string AssignmentId { get; set; }
        public string StudentId { get; set; }
        public string? Content { get; set; }
        public DateTime SubmitDate { get; set; } = DateTime.Now;

        public IFormFile? Attachments { get; set; }
    }
}
