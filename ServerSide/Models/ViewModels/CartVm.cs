namespace ServerSide.Models.ViewModels
{
    public class CartVm
    {
        public int Id { get; set; }

        public int MemberId { get; set; }

        public string? MemberAccount { get; set; }

        public string? MemberName { get; set; }
    }
}
