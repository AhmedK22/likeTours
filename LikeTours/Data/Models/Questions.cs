using LikeTours.Data.Common.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LikeTours.Data.Models
{
    public class Questions : IDBEntity
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "text")]
        public string Question { get; set; }
        [Column(TypeName = "text")]
        public string Answer { get; set; }
        public string Lang { get; set; }
        public bool Main { get; set; } = false;
        public int? QuestionId { get; set; }
        public Questions MainQuetion { get; set; }
        [JsonIgnore]
        public ICollection<Questions> RefferenceQuestion { get; set; }

    }
}
