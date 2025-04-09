using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Model
{
    public class Answer
    {
        public string Id { get; set; }
        public string Content { get; set; }
        public bool IsCorrect { get; set; }

        public string QuestionId { get; set; }
        public Question Question { get; set; }
    }
}
