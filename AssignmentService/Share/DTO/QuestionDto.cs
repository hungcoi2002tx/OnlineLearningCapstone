﻿namespace Share.DTO
{
    public class QuestionDto
    {
        public string Id { get; set; }
        public string Content { get; set; }

        public int AssignmentId { get; set; }
        public AssignmentDto Assignment { get; set; }

        public List<AnswerDto> Answers { get; set; }
    }
}
