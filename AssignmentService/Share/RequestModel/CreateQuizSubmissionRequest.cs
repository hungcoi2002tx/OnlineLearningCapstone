using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.RequestModel
{
    public class CreateQuizSubmissionRequest
    {
        public string AssignmentId { get; set; } 
        public string StudentId { get; set; }      
        public DateTime SubmitDate { get; set; } = DateTime.Now;

        public List<QuizAnswerModel> QuizAnswers { get; set; }
    }

    public class QuizAnswerModel
    {
        public string QuestionId { get; set; }
        public string AnswerId { get; set; }
    }
}
