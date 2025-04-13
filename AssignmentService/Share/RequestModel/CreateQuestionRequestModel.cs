namespace Share.RequestModel
{
    public class CreateQuestionRequestModel
    {
        public string Content { get; set; }
        public List<CreateAnswerRequestModel> Answers { get; set; }
    }
}