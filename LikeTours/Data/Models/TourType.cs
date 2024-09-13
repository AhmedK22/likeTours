using LikeTours.Data.Common.Model;
using System.ComponentModel.DataAnnotations;

namespace LikeTours.Data.Models
{
    public class TourType : IDBEntity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Lang { get; set; }
        public bool Main { get; set; } = false;
        public int? TourTypeId  { get; set; }
        public TourType MainTourType { get; set; }
        public ICollection<TourType> RefferenceTypes { get; set; }
        public ICollection<Package> Packages { get; set; }
    }
}
