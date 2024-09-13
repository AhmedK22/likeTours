using LikeTours.Data.Common.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LikeTours.Data.Models
{
    public class About: IDBEntity
    {
        [Key]
        public int Id { get; set; }
        public string MainImage { get; set; }
        public string WhoAreImage { get; set; }
        [Column(TypeName ="Text")]
        public string WhoAreText { get; set; }

        public string Lang { get; set; }
        public bool Main { get; set; } = false;
        public int? AboutId { get; set; }
        public About MainAbout { get; set; }
        [JsonIgnore]
        public ICollection<About> RefferenceAbout { get; set; }

    }
}
