using LikeTours.Data.Common.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LikeTours.Data.Models
{
    public class Section : IDBEntity
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string SectionDetails { get; set; }
        public int PackageId { get; set; }
        public Package Package { get; set; }
    }
}
