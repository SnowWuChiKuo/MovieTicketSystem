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

        //public int MemberId { get; set; }

        //public DateTime CreatedAt { get; set; }

        public IEnumerable<CartItemVm> CartItems { get; set; }

        public int Total => CartItems.Sum(p => p.SubTotal); //總計
        public bool AllowCheckout => CartItems.Any(); //至少有一件商品才允許結帳

    }

  
}