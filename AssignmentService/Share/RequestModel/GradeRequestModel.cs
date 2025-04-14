using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.RequestModel
{
    public class GradeRequestModel
    {
        public string SubmissionId { get; set; }
        public float? Grade { get; set; }
        public string? Feedback { get; set; }
    }
}
