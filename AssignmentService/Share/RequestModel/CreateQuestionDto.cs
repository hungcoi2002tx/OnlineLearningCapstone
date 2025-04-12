namespace Share.RequestModel
{
    public class CreateQuestionDto
    {
        public string Content { get; set; }
        public List<CreateAnswerDto> Answers { get; set; }
    }
}