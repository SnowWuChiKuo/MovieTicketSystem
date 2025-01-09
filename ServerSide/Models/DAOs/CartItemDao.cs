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
                .Include(u => u.Cart)
                .Include(u=> u.Ticket)
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
            var data = _db.Tickets.AsNoTracking().FirstOrDefault(t=>t.Id == model.TicketId);
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
            var data = _db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == model.TicketId);
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
            var cartItem = _db.CartItems.FirstOrDefault(ci => ci.CartId == dto.CartId && ci.TicketId == dto.TicketId);
            if (cartItem != null)
            {
                cartItem.CartId = dto.CartId;
                cartItem.TicketId = dto.TicketId;
                cartItem.Qty = dto.Qty;
                cartItem.SubTotal = dto.SubTotal;

                _db.SaveChanges();
            }
            else
            {
                throw new Exception("找不到該購物車項目");
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
