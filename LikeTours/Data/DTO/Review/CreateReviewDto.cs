using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LikeTours.Data.DTO.Review
{
    public class CreateReviewDto
    {
       
        public int Id { get; set; }  
        public string Header { get; set; }
        public IFormFile? Image { get; set; }
        public string Body { get; set; }
        public int TypeId { get; set; }
        public int PackageId { get; set; }
        public bool IsConfirmed { get; set; } 
        public string Lang { get; set; }
        public bool Main { get; set; } = false;
        public int? ReviewId { get; set; }
       
    }
}
