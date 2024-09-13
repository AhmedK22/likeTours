using LikeTours.Data.Common.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LikeTours.Data.Models
{
    public class Review : IDBEntity
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "text")]
        public string Header { get; set; }
        public string? Image { get; set; }
        [Column(TypeName = "text")]
        public string Body { get; set; }
        public int TypeId { get; set; }
        public int PackageId { get; set; }
        public bool IsConfirmed { get; set; } = false;
        public virtual Package Package { get; set; }

        public string Lang { get; set; }
        public bool Main { get; set; } = false;
        public int? ReviewId { get; set; }
        public Review MainReview { get; set; }
        [JsonIgnore]
        public ICollection<Review> RefferenceReview { get; set; }

    }
}
