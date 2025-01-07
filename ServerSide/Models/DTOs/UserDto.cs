namespace ServerSide.Models.DTOs
{
    public class UserDto
    {
        public string Account { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string PasswordHash { get; set; }

        public string Name { get; set; }

        public bool IsAdmin { get; set; }

    }
}
