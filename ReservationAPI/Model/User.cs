namespace ReservationAPI.Model
{
    public class CreateUser
    {

        public string Username { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string PasswordHash { get; set; } = null!;


    }

    public class LoginUser
    {
        public string Username { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public bool RememberMe { get; set; }
    }
}
