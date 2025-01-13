using ClientSide.Models.DAOs;
using ClientSide.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Web;

namespace ClientSide.Models.Services
{
    public class CartItemService
    {
        private readonly CartItemEFDao _dao = new CartItemEFDao();

        public CartItemDetailVm Get(int id)
        {

            return _dao.Get(id);
        }
    }
}