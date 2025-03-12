using Microsoft.AspNetCore.Mvc;
using AdvancedDevelopment.Models;
using AdvancedDevelopment.Services;
using System.Linq;

namespace AdvancedDevelopment.Controllers
{
    /// <summary>
    /// Controller for managing products.
    /// </summary>
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductController"/> class.
        /// </summary>
        /// <param name="productService">The product service.</param>
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Displays the list of products with optional filters and sorting.
        /// </summary>
        /// <param name="sortBy">Sort order for products.</param>
        /// <param name="maxPrice">Maximum price for filtering products.</param>
        /// <param name="brand">Brand name for filtering products.</param>
        /// <param name="gender">Target gender for filtering products.</param>
        /// <returns>The product list view with applied filters and sorting.</returns>
        public IActionResult Index(string sortBy, decimal? maxPrice, string brand, string gender)
        {
            var products = _productService.GetAllProducts();

            if (maxPrice.HasValue)
            {
                products = products.Where(p => p.Price <= maxPrice.Value);
            }

            if (!string.IsNullOrEmpty(brand))
            {
                products = products.Where(p => p.Brand.Equals(brand, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(gender))
            {
                products = products.Where(p => p.Gender.Equals(gender, StringComparison.OrdinalIgnoreCase));
            }

            products = sortBy switch
            {
                "PriceLowToHigh" => products.OrderBy(p => p.Price),
                "PriceHighToLow" => products.OrderByDescending(p => p.Price),
                _ => products
            };

            ViewBag.Brands = _productService.GetAllBrands();
            ViewBag.RecentlyViewedProducts = _productService.GetRecentlyViewed();

            return View(products);
        }

        /// <summary>
        /// Displays the details of a product.
        /// </summary>
        /// <param name="id">The ID of the product to display.</param>
        /// <returns>The product details view.</returns>
        public IActionResult Details(int id)
        {
            _productService.TrackRecentlyViewed(id);

            var product = _productService.GetProductById(id);
            if (product == null) return NotFound();

            ViewBag.Reviews = _productService.GetReviewsByProductId(id);
            ViewBag.AverageRating = _productService.GetAverageRating(id);
            ViewBag.RelatedProducts = _productService.GetRelatedProducts(id);
            ViewBag.RecentlyViewedProducts = _productService.GetRecentlyViewed();

            return View(product);
        }

        /// <summary>
        /// Adds a review to a product.
        /// </summary>
        /// <param name="productId">The ID of the product to review.</param>
        /// <param name="review">The review to add.</param>
        /// <returns>A redirect to the product details view.</returns>
        [HttpPost]
        public IActionResult AddReview(int productId, Review review)
        {
            _productService.AddReview(productId, review);
            return RedirectToAction("Details", new { id = productId });
        }
    }
}
