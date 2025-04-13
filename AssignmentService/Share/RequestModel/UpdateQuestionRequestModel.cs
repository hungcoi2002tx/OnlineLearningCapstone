using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.RequestModel
{
    public class UpdateQuestionRequestModel
    {
        public string Content { get; set; }
        public List<UpdateAnswerRequestModel> Answers { get; set; }
    }
}
