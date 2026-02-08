using BookShoppingCart.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Security.Claims;

namespace BookShoppingCart.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartRepository(ApplicationDbContext db, UserManager<IdentityUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<int> AddItem(int bookId, int qty)
        {
            using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
            {
                string userId = GetUserId();
                try
                {
                    if (string.IsNullOrEmpty(userId))
                        throw new Exception("User is not logg-in");
                    ShoppingCart cart = await GetCart(userId);
                    if (cart is null)
                    {
                        cart = new ShoppingCart()
                        {
                            UserId = userId,
                        };
                        _db.ShoopingCarts.Add(cart);
                    }
                    _db.SaveChanges();
                    CartDetail cartItem = _db.CartDetails.FirstOrDefault(a => a.ShoppingCartId == cart.Id && a.BookId == bookId);
                    if (cartItem is not null)
                    {
                        cartItem.Quantity += qty;
                    }
                    else
                    {
                        cartItem = new CartDetail()
                        {
                            BookId = bookId,
                            ShoppingCartId = cart.Id,
                            Quantity = qty
                        };
                        _db.CartDetails.Add(cartItem);
                    }
                    _db.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                }
                int cartItemCount = await GetCartItemCount(userId);
                return cartItemCount;
            }
        }
        public async Task<int> RemoveItem(int bookId)
        {
            string userId = GetUserId();
            try
            {
                if (string.IsNullOrEmpty(userId))
                    throw new Exception("User is not logg-in");

                ShoppingCart cart = await GetCart(userId);
                if (cart is null)
                    throw new Exception("Cart is empty");

                CartDetail cartItem = _db.CartDetails.FirstOrDefault(a => a.ShoppingCartId == cart.Id && a.BookId == bookId);
                if (cartItem is null)
                    throw new Exception("No items in the cart");
                else if (cartItem.Quantity == 1)
                    _db.CartDetails.Remove(cartItem);
                else
                    cartItem.Quantity -= 1;
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
            }
            int cartItemCount = await GetCartItemCount(userId);
            return cartItemCount;
        }
        public async Task<ShoppingCart> GetUserCart()
        {
            string userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
                throw new Exception("Invalid UserId");
            ShoppingCart shoppinCart = await _db.ShoopingCarts
                .Include(a => a.cartDetails)
                .ThenInclude(a => a.Book)
                .ThenInclude(a => a.Genre)
                .Where(a => a.UserId == userId).FirstOrDefaultAsync();
            return shoppinCart;
        }
        public async Task<ShoppingCart> GetCart(string userId) {
            ShoppingCart cart = await _db.ShoopingCarts.FirstOrDefaultAsync(x => x.UserId == userId);
            return cart;
        }
        public async Task<int> GetCartItemCount(string userId = "")
        {
            if (string.IsNullOrEmpty(userId))
                userId = GetUserId();
            
            var data = await (from cart in _db.ShoopingCarts
                        join cartDetail in _db.CartDetails
                        on cart.Id equals cartDetail.ShoppingCartId
                        select new { cartDetail.Id }
                ).ToListAsync();
            return data.Count;
        }
        private string GetUserId()
        {
            ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User;
            string userId = _userManager.GetUserId(principal);
            return userId;
        }
    }
}
