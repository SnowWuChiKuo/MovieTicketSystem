using ClientSide.Models.EFModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClientSide.Models.ViewModels
{
    public class CartVm
    {
        public int Id { get; set; }

        public int MemberId { get; set; }

        public int TotalPrice { get; set; }

        public DateTime CreatedAt { get; set; }

        public virtual IEnumerable<CartItemVm> CartItems { get; set; }

        public virtual Member Member { get; set; }

        public bool AllowCheckout => CartItems.Any();  //至少要有一項商品才可以結帳

    }

    public class CartItemVm
    {
        public int Id { get; set; }

        public int CartId { get; set; }

        public int TicketId { get; set; }

        public int Qty { get; set; }

        public int SubTotal { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public virtual Cart Cart { get; set; }

        public virtual Ticket Ticket { get; set; }

    }
}