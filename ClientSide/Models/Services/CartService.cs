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

        public void CreateOrder(string account)
        {
            _repo.CreateOrder(account);
        }

        public void EmptyCart(string account)
        {
            _repo.EmptyCart(account);
        }

        public void AddCartItem(int cartId, int ticketId, int qty, string seatName)
        {
            _repo.AddCartItem(cartId, ticketId, qty, seatName);
        }

        public bool CheckIfCartItemsValid(List<int> cartItemIds)
        {
            //已經開播或該場次seatstatus已經不可用
            List<DateTime> startTimes = _repo.GetScreeningStartTime(cartItemIds);
            //是否已經開播
            bool isStartTimeValid = CheckScreeningStartTime(startTimes);

            //該場次seatstatus已經不可用
            var seatStatusList = _repo.GetSeatStatus(cartItemIds);
            bool isSeatStatusValid = CheckSeatStatus(seatStatusList);

            //驗證皆通過就true
           return (isStartTimeValid && isSeatStatusValid) ? true : false;
        }

        public bool CheckSeatStatus(List<string> seatStatusList)
        {
            foreach (var item in seatStatusList)
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