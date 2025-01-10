using Humanizer;
using Microsoft.EntityFrameworkCore;
using ServerSide.Models.DTOs;
using ServerSide.Models.EFModels;
using ServerSide.Models.ViewModels;

namespace ServerSide.Models.DAOs
{
    public class CartItemDao
    {
        private readonly AppDbContext _db;
        public CartItemDao(AppDbContext db)
        {
            _db = db;
        }
        public List<CartItemVm> GetAll()
        {
            var data = _db.CartItems.AsNoTracking()
                .Include(ci => ci.Cart)
                .Include(ci=> ci.Ticket)
                .Select(ci => new CartItemVm
                 {
                    Id = ci.Id,
                    CartId = ci.CartId,
                    TicketId = ci.TicketId,
                    TicketName = $"{ci.Ticket.SalesType} - {ci.Ticket.TicketType}",
                    SubTotal = ci.SubTotal,
                    Qty = ci.Qty,
                    CreatedAt = ci.CreatedAt,
                    UpdatedAt = ci.UpdatedAt
                 });
            return data.ToList();
        }

        public void Create(CartItemDto model)
        {
            //查看是否同個購物車內已經存在相同項目
            var cartItemInDb = _db.CartItems.FirstOrDefault(ci => ci.CartId == model.CartId && ci.TicketId == model.TicketId);

            //尋找資料庫內有無該 Cart 和 Ticket
            var cartInDb = _db.Carts.FirstOrDefault(c => c.Id == model.CartId);
            var ticketInDb = _db.Tickets.FirstOrDefault(t => t.Id == model.TicketId);

            //如果沒有該 Cart 或 Ticket
            if (cartInDb != null && ticketInDb != null)
            {
                //不存在相同項目，Insert 新的 data
                if (cartItemInDb == null)
                {
                    CartItem cartItem = new CartItem
                    {
                        CartId = model.CartId,
                        TicketId = model.TicketId,
                        Qty = model.Qty,
                        SubTotal = model.SubTotal,
                        CreatedAt = DateTime.Now
                    };
                    _db.CartItems.Add(cartItem);
                    _db.SaveChanges();
                }
                else //存在相同項目，加上該相同項目的數量
                {
                    cartItemInDb.Qty += model.Qty;
                    _db.SaveChanges();
                }
            }
            else
            {
                throw new Exception("您要新增的項目不存在");
            }

        }

        public string GetTicketNameForCreate(CartItemCreateVm model)
        {
            var ticket = _db.Tickets.FirstOrDefault(t => t.Id == model.TicketId);
            if (ticket != null)
            {
                return $"{ticket.SalesType} - {ticket.TicketType}";
            }
            else
            {
                throw new Exception("找不到該票種");
            }
        }

        public string GetTicketNameForEdit(CartItemEditVm model)
        {
            var ticket = _db.Tickets.FirstOrDefault(t => t.Id == model.TicketId);
            if (ticket != null)
            {
                return $"{ticket.SalesType} - {ticket.TicketType}";
            }
            else
            {
                throw new Exception("找不到該票種");
            }
        }
        public string GetTicketNameById(int id)
        {
            var ticket = GetTicketById(id);
            if (ticket != null)
            {
                return $"{ticket.SalesType} - {ticket.TicketType}";
            }
            else
            {
                throw new Exception("找不到該票種");
            }
        }

        public Ticket GetTicketById(int id)
        {
            var ticket = _db.Tickets.FirstOrDefault(t => t.Id == id);
            if (ticket != null)
            {
                return ticket;
            }
            else
            {
                throw new Exception("找不到該票種");
            }
        }

        public int GetSubTotalForCreate(CartItemCreateVm model)
        {
            var data = _db.Tickets.FirstOrDefault(t=>t.Id == model.TicketId);
            if(data != null)
            {
                return data.Price * model.Qty;
            }
            else
            {
                throw new Exception("找不到該票種");
            }
        }
        public int GetSubTotalForEdit(CartItemEditVm model)
        {
            var data = _db.Tickets.FirstOrDefault(t => t.Id == model.TicketId);
            if (data != null)
            {
                return data.Price * model.Qty;
            }
            else
            {
                throw new Exception("找不到該票種");
            }
        }

        public CartItemDto Get(int id)
        {
            var cartItemInDb = _db.CartItems.AsNoTracking().FirstOrDefault(ci => ci.Id == id);
            if (cartItemInDb != null)
            {
                var data = new CartItemDto()
                {
                    Id = cartItemInDb.Id,
                    CartId = cartItemInDb.CartId,
                    TicketId = cartItemInDb.TicketId,
                    TicketName = GetTicketNameById(cartItemInDb.TicketId),
                    Qty = cartItemInDb.Qty,
                    SubTotal = cartItemInDb.SubTotal,
                    CreatedAt = cartItemInDb.CreatedAt,
                    UpdatedAt = cartItemInDb.UpdatedAt
                };
                return data;
            }
            else
            {
                throw new Exception("找不到該購物車項目");
            }
        }

        public void Edit(CartItemDto dto)
        {
            //該 cartitem
             var cartItem = _db.CartItems.FirstOrDefault(ci => ci.Id == dto.Id);
            

            //尋找資料庫內有無該 Cart 和 Ticket
            var cartInDb = _db.Carts.FirstOrDefault(c => c.Id == dto.CartId);
            var ticketInDb = _db.Tickets.FirstOrDefault(t => t.Id == dto.TicketId);

            //查看該購物車是否已存在相同的 CartItem 
            var cartItemInDb = _db.CartItems.FirstOrDefault(ci => ci.CartId == dto.CartId && ci.TicketId == dto.TicketId);

            //確認資料庫中真有該 Cart , Ticket
            if (cartInDb != null && ticketInDb != null )
            {
                //不存在相同項目
                if(cartItemInDb == null) {

                    cartItem.CartId = dto.CartId;
                    cartItem.TicketId = dto.TicketId;
                    cartItem.Qty = dto.Qty;
                    cartItem.SubTotal = dto.SubTotal;

                    _db.SaveChanges();
                }
                //已存在相同項目
                else
                {
                    //該相同項目與原本項目不變(只單純改數量(Qty))
                    if (cartItem.Id == cartItemInDb.Id)
                    {
                        // 將編輯後的數量寫入到已存在的 cartitem 內
                        cartItem.Qty = dto.Qty;
                        cartItemInDb.Qty = dto.Qty;
                        _db.SaveChanges();
                    }

                    //有改 CartId 或 TicketId
                    else
                    { //將編輯後的數量寫入到已存在的 cartitem 內
                        cartItemInDb.Qty += dto.Qty;
                       //刪掉原先的項目的資料
                        DeleteById(cartItem.Id);
                        _db.SaveChanges();
                    }
                }
            }
            else
            {
                throw new Exception("找不到該購物車或票種");
            }
        }
        public void DeleteById(int id )
        {
            var cartItem = _db.CartItems.FirstOrDefault(ci => ci.Id == id);
            if (cartItem != null)
            {
                _db.Remove(cartItem);
                _db.SaveChanges();
            }
            else
            {
                throw new Exception("找不到該購物車物品項目");
            }
        }

        public void Delete(int cartId, int ticketId)
        {
            var cartItem = _db.CartItems.FirstOrDefault(ci => ci.CartId == cartId && ci.TicketId == ticketId);
            if (cartItem != null)
            {
                _db.Remove(cartItem);
                _db.SaveChanges();
            }
            else
            {
                throw new Exception("找不到該購物車物品項目");
            }
        }
        //這個方法會傳入 cartId 和選擇性的排除的 ticketId。
        // GetTotalQuantityInCart() 會回傳指定的 cart 內所有 cartItems 的數量總和，並且會排除 ticketIdToExclude 的數量。
        public int GetTotalQuantityInCart(int cartId, int? ticketIdToExclude = null)
        {
            var query = _db.CartItems.Where(ci => ci.CartId == cartId);

            if (ticketIdToExclude.HasValue)
            {
                query = query.Where(ci => ci.TicketId != ticketIdToExclude.Value);
            }

            return query.Sum(ci => ci.Qty);
        }


    }
}
