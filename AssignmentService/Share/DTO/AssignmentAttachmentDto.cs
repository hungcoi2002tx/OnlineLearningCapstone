using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.DTO
{
    public class AssignmentAttachmentDto
    {
        public Guid AttachmentId { get; set; }
        public Guid AssignmentId { get; set; }
        public string FileUrl { get; set; }
        public string FileType { get; set; }
        public DateTime UploadedAt { get; set; }
    }
}
