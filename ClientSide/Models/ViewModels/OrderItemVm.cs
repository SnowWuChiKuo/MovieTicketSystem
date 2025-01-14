using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ClientSide.Models.ViewModels
{
    public class OrderItemVm
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        public int TicketId { get; set; }

        public string TicketName { get; set; }

        public int Price { get; set; }

        public int Qty { get; set; }

        public int SubTotal { get; set; }

        public string ImgPath { get; set; }

        public string SeatName { get; set; }

        public string MovieTitle { get; set; }

        public string MovieTime { get; set; }
    }

    public class OrderItemDetailVm
    {
        public int Id { get; set; }//

        [Display(Name = "廳")]
        public string TheaterName { get; set; }//

        [Display(Name = "電影名稱")]
        public string MovieTitle { get; set; }

        [Display(Name = "場次時間")]
        public string MovieTime { get; set; }

        [Display(Name = "座位")]
        public string SeatName { get; set; }

        [Display(Name = "購票數")]
        public int Qty { get; set; }

        [Display(Name = "單票價格")]
        public int Price { get; set; }

        [Display(Name = "該項總價")]
        public int SubTotal { get; set; }

    }
}