namespace LikeTours.Data.DTO.Question
{
    public class CreateQuestionDto
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public string Lang { get; set; }
        public bool Main { get; set; }
        public int? QuestionId { get; set; }
    }
}
