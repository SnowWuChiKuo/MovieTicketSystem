using ServerSide.Models.DAOs;
using ServerSide.Models.DTOs;
using ServerSide.Models.EFModels;
using ServerSide.Models.Infra;
using ServerSide.Models.ViewModels;

namespace ServerSide.Models.Services
{
    public class CartItemService
    {
        private readonly CartItemDao _dao;
        public CartItemService(CartItemDao dao)
        {
            _dao = dao;
        }
        public List<CartItemVm> GetAll()
        {
            return _dao.GetAll();
        }
        public void Create(CartItemDto model)
        {
            // 檢查單一商品數量是否超過 6
            if (model.Qty > 6)
            {
                throw new Exception("單一商品數量不能超過 6");
            }

            // 檢查購物車總數量是否超過 6
            CheckCartTotalQuantity(model.CartId, model.Qty);


            _dao.Create(model);
        }
        public void CheckCartTotalQuantityForEdit(int cartId, int quantityToAdd, int cartItemQty)
        {
            int currentExcludeTotal = _dao.GetTotalQuantityInCartForEdit(cartId, cartItemQty);

            if (currentExcludeTotal + quantityToAdd > 6)
            {
                throw new Exception($"購物車內商品總數量不能超過 6。 目前數量: {currentExcludeTotal} ，您想新增數量: {quantityToAdd}");
            }
        }
        public void CheckCartTotalQuantity(int cartId, int quantityToAdd, int? ticketIdToExclude = null)
        {
            int currentTotal = _dao.GetTotalQuantityInCart(cartId, ticketIdToExclude);

            if (currentTotal + quantityToAdd > 6)
            {
                throw new Exception($"購物車內商品總數量不能超過 6。 目前數量: {currentTotal} ，您想新增數量: {quantityToAdd}");
            }
        }

        public int GetTotalQuantityInCart(int cartId)
        {
            return _dao.GetTotalQuantityInCart(cartId); 
        }

        // 新增方法簽名
        public int GetTotalQuantityInCart(int cartId, int? ticketIdToExclude = null)
        {
            return _dao.GetTotalQuantityInCart(cartId, ticketIdToExclude);
        }

        public CartItemDto Get(int id)
        {
            return _dao.Get(id);

        }
        public CartItemDto ConvertToDtoForCreate(CartItemCreateVm model)
        {
            var dto = new CartItemDto
            {
                CartId = model.CartId,
                TicketId = model.TicketId,
                TicketName = _dao.GetTicketNameForCreate(model),
                Qty = model.Qty,
                SubTotal = _dao.GetSubTotalForCreate(model)
            };
            return dto;
        }
        public CartItemDto ConvertToDtoForEdit(CartItemEditVm model)
        {
            var dto = new CartItemDto
            {
                Id = model.Id,
                CartId = model.CartId,
                TicketId = model.TicketId,
                TicketName = _dao.GetTicketNameForEdit(model),
                Qty = model.Qty,
                SubTotal = _dao.GetSubTotalForEdit(model)
            };
            return dto;
        }

        public Ticket GetTicketById(int ticketId)
        {
            return _dao.GetTicketById(ticketId);
        }

        public void Edit(CartItemDto dto ,int cartItemQty)
        {
            // 檢查單一商品數量是否超過 6
            if (dto.Qty > 6)
            {
                throw new Exception("單一商品數量不能超過 6");
            }

            // 檢查購物車總數量是否超過 6，並且排除自身數量
            CheckCartTotalQuantityForEdit(dto.CartId, dto.Qty, cartItemQty);

            _dao.Edit(dto);
        }

        public void Delete(int cartId , int ticketId)
        {
            _dao.Delete(cartId , ticketId);
        }

    }
}
