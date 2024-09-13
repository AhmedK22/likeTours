namespace LikeTours.Data.DTO.Payment
{
    public class CreatePaymentDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Desc { get; set; }
        public string Lang { get; set; }
        public bool Main { get; set; } = false;
        public int? PaymentId { get; set; }
    }
}
