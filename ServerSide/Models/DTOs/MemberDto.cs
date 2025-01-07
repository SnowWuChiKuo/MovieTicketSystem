using System.ComponentModel.DataAnnotations;

namespace ServerSide.Models.DTOs
{
    public class MemberDto
    {
        public string Account { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string PasswordHash { get; set; }

        public string Name { get; set; }

        public bool IsDeleted { get; set; }
  
        public bool IsBlackList { get; set; }

        public bool IsConfirmed { get; set; }
 
        public string ConfirmCode { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
