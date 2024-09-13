using LikeTours.Data.Common.Model;
using System.ComponentModel.DataAnnotations;

namespace LikeTours.Data.Models
{
    public class Image : IDBEntity
    {
        [Key]
        public int Id { get; set; }
        public string Url { get; set; }
        public int PackageId { get; set; }
        public Package Package { get; set; }
    }
}
