namespace LikeTours.Data.DTO.Payment
{
    public class PaymentQueryParams : QueryParams
    {
       
        public string Title { get; set; }
        public string Lang { get; set; }
        public bool? Main { get; set; } 
     
    }
}
