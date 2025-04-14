using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Other.SearchModel
{
    public class SubmissionSearch
    {
        public string? Id { get; set; }
        public string? AssignmentId { get; set; }
        public string? StudentId { get; set; }
        public string? Status { get; set; }
        public bool? IsQuiz { get; set; }
        public bool? IsAll { get; set; }
        public Page? Page { get; set; }
        public List<string>? Ids { get; set; }

    }
}
