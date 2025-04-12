using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.RequestModel
{
    public class QuizRequestDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Deadline { get; set; }
        public int TeacherId { get; set; }
        public int ClassroomId { get; set; }
        public string Type { get; set; }

        public List<CreateQuestionDto> Questions { get; set; }
    }
}
