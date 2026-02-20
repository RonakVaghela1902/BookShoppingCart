using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShoppingCart.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartRepository _cartRepository;
        public CartController(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }
        public async Task<IActionResult> AddItem(int bookId, int qty = 1, int redirect = 0)
        {
            var cartCount = await _cartRepository.AddItem(bookId, qty);
            if (redirect == 0)
                return Ok(cartCount);
            return RedirectToAction("GetUserCart");
        }
        public async Task<IActionResult> RemoveItem(int bookId)
        {
            int cartCount = await _cartRepository.RemoveItem(bookId);
            return RedirectToAction("GetUserCart");
        }
        public async Task<IActionResult> GetUserCart()
        {
            ShoppingCart cart = await _cartRepository.GetUserCart();
            return View(cart);
        }
        public async Task<IActionResult> GetTotalItemInCart(string userId = "")
        {
            int cartItemCount = await _cartRepository.GetCartItemCount();
            return Ok(cartItemCount);
        }
        public IActionResult Checkout()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Checkout(CheckoutModel checkoutModel)
        {
            if (!ModelState.IsValid)
                return View(checkoutModel);
            bool isCheckedOut = await _cartRepository.Checkout(checkoutModel);
            if (!isCheckedOut)
                return RedirectToAction(nameof(OrderFailure));
            return RedirectToAction(nameof(OrderSuccess));
        }
        public IActionResult OrderSuccess()
        {
            return View();
        }
        public IActionResult OrderFailure()
        {
            return View();
        }
    }
}
