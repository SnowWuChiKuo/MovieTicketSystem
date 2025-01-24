using ClientSide.Models.DAOs;
using ClientSide.Models.EFModels;
using ClientSide.Models.Repository;
using ClientSide.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ClientSide.Models.Services
{
    public class CartService
    {
        private readonly CartEFRepository _repo = new CartEFRepository();
        public CartVm GetCartInfo(string account)
        {
            CartVm cart = _repo.GetCartInfo(account);

            return cart;
        }

        public void CreateOrder(string account, string seatName, int screeningId)
        {
            var seatstatus = _repo.CheckSeatStatus(seatName, screeningId);

            foreach (var item in seatstatus) 
            {
                if (item == null) throw new Exception("找不到此座位!");
            }

            _repo.CreateOrder(account, seatstatus);
        }

        public void EmptyCart(string account)
        {
            _repo.EmptyCart(account);
        }

        public void AddCartItem(int cartId, int ticketId, int qty, string seatName)
        {
            var cartItemTotalQty = _repo.CartItemTotalQty(cartId);

            if (cartItemTotalQty == 6) throw new Exception("購物車已經有6張電影票! 請先結帳後再繼續購買!");

            if (cartItemTotalQty + qty > 6) throw new Exception($"購物車有{cartItemTotalQty}張，購物車不可以超過6張票!請重新選擇票的數量!");

            _repo.AddCartItem(cartId, ticketId, qty, seatName);
        }

        public bool CheckIfCartItemsValid(List<int> cartItemIds, string seatName )
        {
            //已經開播或該場次SeatStatus已經不可用
            List<DateTime> startTimes = _repo.GetScreeningStartTime(cartItemIds);
            //是否已經開播
            bool isStartTimeValid = CheckScreeningStartTime(startTimes);

            ////該場次SeatStatus已經不可用
            //var SeatStatusList = _repo.GetSeatStatus(cartItemIds, seatName, screeningId);
            //bool isSeatStatusValid = CheckSeatStatus(SeatStatusList);

            //驗證皆通過就true
            //return (isStartTimeValid && isSeatStatusValid) ? true : false;
            return isStartTimeValid  ? true : false;

        }

        public bool CheckSeatStatus(List<string> SeatStatusList)
        {
            foreach (var item in SeatStatusList)
            {
                if (item == "不可使用")
                {
                    return false;
                }
            }
            return true;
        }

        public bool CheckScreeningStartTime(List<DateTime> startTimes)
        {
            foreach (var item in startTimes)
            {
                if(item <= DateTime.Now)
                {
                    return false;
                }
            }
            return true;
        }

        public List<int> GetCartItemIds(int cartId)
        {
            return _repo.GetCartItemIds(cartId);
        }
    }
}