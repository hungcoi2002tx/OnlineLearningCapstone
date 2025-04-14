using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Other.SearchModel
{
    public class AssignmentSearch
    {
        public bool? IsAll { get; set; }
        public Page? Page { get; set; }
        public string? Id { get; set; }
        public string? ClassroomId { get; set; }
        public string? TeacherId { get; set; }
        public string? Status { get; set; }

        public string? Type { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
