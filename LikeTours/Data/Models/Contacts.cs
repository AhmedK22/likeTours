using LikeTours.Data.Common.Model;
using System.ComponentModel.DataAnnotations;

namespace LikeTours.Data.Models
{
    public class Contact : IDBEntity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string ContactWay { get; set; }
    }
}
