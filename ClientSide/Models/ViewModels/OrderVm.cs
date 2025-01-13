using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClientSide.Models.ViewModels
{
    public class OrderVm
    {
        public int Id { get; set; }

        public int MemberId { get; set; }

        public int? CouponId { get; set; }

        public int TotalAmount { get; set; }

        public bool Status { get; set; }

        public int? DiscountPrice { get; set; }

        public DateTime CreatedAt { get; set; }

        public IEnumerable<OrderItemVm> OrderItems { get; set; }
        public bool AllowCheckout => OrderItems.Any(); //至少有一件商品才允許結帳
    }
}