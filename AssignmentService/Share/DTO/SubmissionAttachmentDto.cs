using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.DTO
{
    public class SubmissionAttachmentDto
    {
        public string AttachmentId { get; set; }
        public string SubmissionId { get; set; }
        public string? FileUrl { get; set; }
        public string? FileType { get; set; }
        public DateTime UploadedAt { get; set; }
    }
}
