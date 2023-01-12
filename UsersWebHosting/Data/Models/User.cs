using Microsoft.AspNetCore.Identity;

namespace UsersWebHosting.Data.Models
{
    public class User : IdentityUser
    {
        public virtual AvatarImage Avatar { get; set; }
    }
}
