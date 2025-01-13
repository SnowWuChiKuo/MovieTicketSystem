using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ClientSide.Models.ViewModels
{
    public class CheckoutVm
    {
        //[Display(Name = "優惠碼")]
        //public string CouponCode { get; set; }

        [Display(Name = "總共價錢")]
        public int TotalAmount { get; set; }

        [Display(Name = "付款狀態")]
        public bool Status { get; set; }

        //[Display(Name = "優惠後價格")]
        //public int? DiscountPrice { get; set; } 

        [Display(Name = "訂單時間")]
        public DateTime CreatedAt { get; set; }
    }
}