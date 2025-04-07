using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.DTO
{
    public class QuizAnswerDto
    {
        public string QuestionId { get; set; }
        public List<string> SelectedAnswerIds { get; set; }
    }
}
