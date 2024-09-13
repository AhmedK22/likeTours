using LikeTours.Data.Common.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LikeTours.Data.Models
{
    public class Payment : IDBEntity
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        [Column(TypeName = "text")]
        public string Desc { get; set; }
        public string Lang { get; set; }
        public bool Main { get; set; } = false;
        public int? PaymentId { get; set; }
        public Payment MainPayment { get; set; }
        [JsonIgnore]
        public ICollection<Payment> RefferencePayments { get; set; }

    }
}
