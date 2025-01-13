using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using ClientSide.Models.DAOs;

namespace ClientSide.Models.Services
{
    /// <summary>
    /// 自定義 Action Filter，用於在使用者瀏覽購物車或相關頁面時，清除過期的購物車項目。
    /// </summary>
    public class CartItemCleanupFilter : ActionFilterAttribute
    {
        //private readonly CartItemEFDao _dao = new CartItemEFDao();

        /// <summary>
        /// 在 Action 方法執行前執行。
        /// </summary>
        /// <param name="filterContext">提供 Action 執行上下文的 ActionExecutingContext 物件。</param>
        public override async void OnActionExecuting(ActionExecutingContext filterContext)
        {
            bool shouldReload = false;
            // 檢查請求路徑是否以 "/Carts" 或 "/CartItems" 開頭 (忽略大小寫)。
            if (filterContext.HttpContext.Request.Path.StartsWith("/Carts", System.StringComparison.OrdinalIgnoreCase) ||
                filterContext.HttpContext.Request.Path.StartsWith("/CartItems", System.StringComparison.OrdinalIgnoreCase))
            {
                // 使用依賴注入取得 CartService 的實例。
                var _dao = DependencyResolver.Current.GetService<CartItemEFDao>();

                // 確保 _service 不是 null，如果存在，呼叫 ClearExpiredCartItems 方法。
                if (_dao != null)
                    //await _dao.ClearExpiredCartItems(); // 使用 _dao 執行清理方法
                    shouldReload = await _dao.ClearExpiredCartItems(); // 使用 _dao 執行清理方法
            }
            filterContext.Controller.ViewBag.ShouldReload = shouldReload; // 將是否需要重新整理的值，存在 ViewBag
            // 呼叫基底類別的方法，繼續執行 Action 方法。
            base.OnActionExecuting(filterContext);
        }
    }
}