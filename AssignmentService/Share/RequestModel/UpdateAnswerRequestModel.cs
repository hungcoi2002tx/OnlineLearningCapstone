using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.RequestModel
{
    public class UpdateAnswerRequestModel
    {
        public string Content { get; set; }
        public bool IsCorrect { get; set; }
    }
}
