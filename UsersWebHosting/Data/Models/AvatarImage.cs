using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UsersWebHosting.Data.Models
{
    public class AvatarImage
    {
        [Key]
        public int Id { get; set; }

        public byte[] ImageData { get; set; }

        public string ContentType { get; set; }

        public string FileName { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        public virtual User User { get; set; }
    }
}