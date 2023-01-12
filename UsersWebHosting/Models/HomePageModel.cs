namespace UsersWebHosting.Models
{
    public class HomePageModel
    {
        public bool IsUserSignedIn { get; set; }

        public IEnumerable<UserItem> Users { get; set; }
    }
}
