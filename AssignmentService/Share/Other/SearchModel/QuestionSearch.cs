using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Other.SearchModel
{
    public class QuestionSearch
    {
        public string? Id { get; set; }
        public string? Content { get; set; }
        public string? AssignmentId { get; set; }
        public List<string>? Ids { get; set; }
    }
}
