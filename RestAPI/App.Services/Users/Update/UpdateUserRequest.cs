
namespace App.Services.Users.Update
{
    public class UpdateUserRequest : UserIdentity
    {
        public string user_display_name { get; set; }

        public string user_status_message { get; set; }
    }
}
