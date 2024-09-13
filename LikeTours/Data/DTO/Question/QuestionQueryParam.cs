namespace LikeTours.Data.DTO.Question
{
    public class QuestionQueryParam : QueryParams
    {
        public string? Question { get; set; }
        public string? Answer { get; set; }
        public string? Lang { get; set; } 
        public bool? Main { get; set; }
    }
}
