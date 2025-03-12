using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using AdvancedDevelopment.Models;
using AdvancedDevelopment.Services;

namespace AdvancedDevelopment.Controllers
{
    /// <summary>
    /// Controller for managing the wishlist.
    /// </summary>
    public class WishlistController : Controller
    {
        private readonly IProductService _productService;

        /// <summary>
        /// Initializes a new instance of the <see cref="WishlistController"/> class.
        /// </summary>
        /// <param name="productService">The product service.</param>
        public WishlistController(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Displays the wishlist.
        /// </summary>
        /// <returns>The wishlist view with the list of products.</returns>
        public IActionResult Index()
        {
            var wishlist = GetWishlist();
            var products = _productService.GetAllProducts().Where(p => wishlist.Contains(p.Id)).ToList();
            return View(products);
        }

        /// <summary>
        /// Adds a product to the wishlist.
        /// </summary>
        /// <param name="productId">The ID of the product to add.</param>
        /// <returns>A redirect to the product index or details page.</returns>
        [HttpPost]
        public IActionResult Add(int productId)
        {
            var wishlist = GetWishlist();
            if (!wishlist.Contains(productId))
            {
                wishlist.Add(productId);
                SaveWishlist(wishlist);
            }
            return RedirectToAction("Index", "Product");
        }

        /// <summary>
        /// Removes a product from the wishlist.
        /// </summary>
        /// <param name="productId">The ID of the product to remove.</param>
        /// <returns>A redirect to the wishlist index page.</returns>
        public IActionResult Remove(int productId)
        {
            var wishlist = GetWishlist();
            if (wishlist.Contains(productId))
            {
                wishlist.Remove(productId);
                SaveWishlist(wishlist);
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Gets the wishlist from the cookies.
        /// </summary>
        /// <returns>A list of product IDs in the wishlist.</returns>
        private List<int> GetWishlist()
        {
            var cookieValue = Request.Cookies["Wishlist"];
            if (string.IsNullOrEmpty(cookieValue))
            {
                return new List<int>();
            }
            return JsonSerializer.Deserialize<List<int>>(cookieValue) ?? new List<int>();
        }

        /// <summary>
        /// Saves the wishlist to the cookies.
        /// </summary>
        /// <param name="wishlist">The wishlist to save.</param>
        private void SaveWishlist(List<int> wishlist)
        {
            var cookieOptions = new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddDays(30),
                HttpOnly = true,
                Secure = true
            };
            var jsonData = JsonSerializer.Serialize(wishlist);
            Response.Cookies.Append("Wishlist", jsonData, cookieOptions);
        }
    }
}
