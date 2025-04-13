using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Other.SearchModel
{
    public class AttachmentSearch
    {
        public string? Id { get; set; }
        public string? AssignmentId { get; set; }
        public string? FileUrl { get; set; }
        public string? FileType { get; set; }
        public List<string>? Ids { get; set; }
    }
}
