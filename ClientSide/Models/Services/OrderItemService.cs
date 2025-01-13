using ClientSide.Models.DAOs;
using ClientSide.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClientSide.Models.Services
{
    public class OrderItemService
    {
        private readonly OrderItemEFDao _dao = new OrderItemEFDao();

        public OrderItemDetailVm Detail(int id)
        {
            return _dao.Get(id);
        }

    }
}