using ClientSide.Models.DAOs;
using ClientSide.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClientSide.Models.Services
{
    public class OrderService
    {
        private readonly OrderEFDao _dao = new OrderEFDao();

        public OrderVm GetOrderInfo(string account)
        {
            return _dao.GetOrderInfo(account);
        }
    }
}