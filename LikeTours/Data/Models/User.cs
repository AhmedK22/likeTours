using LikeTours.Data.Common.Model;
using System.ComponentModel.DataAnnotations;

namespace LikeTours.Data.Models
{
    public class User: IDBEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
