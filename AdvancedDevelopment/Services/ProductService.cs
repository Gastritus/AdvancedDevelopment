using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using AdvancedDevelopment.Models;
using AdvancedDevelopment.Extensions;

namespace AdvancedDevelopment.Services
{
    /// <summary>
    /// Provides services related to products, including loading from file, adding reviews, and tracking recently viewed products.
    /// </summary>
    public class ProductService : IProductService
    {
        private List<Product> _products;
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductService"/> class.
        /// </summary>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        public ProductService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            LoadProductsFromFile();
        }

        /// <summary>
        /// Loads products from a JSON file.
        /// </summary>
        private void LoadProductsFromFile()
        {
            var jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "products.json");
            if (File.Exists(jsonFilePath))
            {
                try
                {
                    var jsonData = File.ReadAllText(jsonFilePath);
                    _products = JsonSerializer.Deserialize<List<Product>>(jsonData) ?? new List<Product>();
                }
                catch (Exception ex)
                {
                    // Log the exception (ex) or handle it accordingly
                    _products = new List<Product>();
                }
            }
            else
            {
                _products = new List<Product>();
            }
        }

        /// <summary>
        /// Gets all products.
        /// </summary>
        /// <returns>An enumerable of all products.</returns>
        public IEnumerable<Product> GetAllProducts()
        {
            return _products;
        }

        /// <summary>
        /// Clears all products.
        /// </summary>
        public void ClearProducts()
        {
            _products.Clear();
        }

        /// <summary>
        /// Gets a product by its ID.
        /// </summary>
        /// <param name="id">The product ID.</param>
        /// <returns>The product with the specified ID, or null if not found.</returns>
        public Product GetProductById(int id)
        {
            return _products.FirstOrDefault(p => p.Id == id);
        }

        /// <summary>
        /// Adds a new product.
        /// </summary>
        /// <param name="product">The product to add.</param>
        public void AddProduct(Product product)
        {
            _products.Add(product);
            SaveProductsToFile();
        }

        /// <summary>
        /// Adds a review to a product.
        /// </summary>
        /// <param name="productId">The product ID.</param>
        /// <param name="review">The review to add.</param>
        public void AddReview(int productId, Review review)
        {
            var product = GetProductById(productId);
            if (product != null)
            {
                product.Reviews.Add(review);
                SaveProductsToFile();
            }
        }

        /// <summary>
        /// Gets reviews for a specific product by its ID.
        /// </summary>
        /// <param name="id">The product ID.</param>
        /// <returns>An enumerable of reviews for the specified product.</returns>
        public IEnumerable<Review> GetReviewsByProductId(int id)
        {
            var product = GetProductById(id);
            return product?.Reviews ?? Enumerable.Empty<Review>();
        }

        /// <summary>
        /// Gets all unique brands from the product list.
        /// </summary>
        /// <returns>An enumerable of unique brand names.</returns>
        public IEnumerable<string> GetAllBrands()
        {
            return _products.Select(p => p.Brand).Distinct().OrderBy(b => b);
        }

        /// <summary>
        /// Gets products related to a specific product.
        /// </summary>
        /// <param name="productId">The product ID.</param>
        /// <returns>An enumerable of related products.</returns>
        public IEnumerable<Product> GetRelatedProducts(int productId)
        {
            var product = GetProductById(productId);
            if (product == null) return Enumerable.Empty<Product>();

            return _products
                .Where(p => p.Id != productId && (p.Category == product.Category || p.Brand == product.Brand))
                .Take(5)
                .ToList();
        }

        /// <summary>
        /// Tracks a recently viewed product using session storage.
        /// </summary>
        /// <param name="productId">The product ID.</param>
        public void TrackRecentlyViewed(int productId)
        {
            var recentlyViewed = _httpContextAccessor.HttpContext.Session.GetObjectFromJson<List<int>>("RecentlyViewed") ?? new List<int>();

            if (!recentlyViewed.Contains(productId))
            {
                recentlyViewed.Insert(0, productId);
                if (recentlyViewed.Count > 3)
                {
                    recentlyViewed = recentlyViewed.Take(3).ToList();
                }
                _httpContextAccessor.HttpContext.Session.SetObjectAsJson("RecentlyViewed", recentlyViewed);
            }
        }

        /// <summary>
        /// Gets the recently viewed products from session storage.
        /// </summary>
        /// <returns>An enumerable of recently viewed products.</returns>
        public IEnumerable<Product> GetRecentlyViewed()
        {
            var recentlyViewed = _httpContextAccessor.HttpContext.Session.GetObjectFromJson<List<int>>("RecentlyViewed") ?? new List<int>();
            return _products.Where(p => recentlyViewed.Contains(p.Id)).ToList();
        }

        /// <summary>
        /// Gets the average rating of a product by its ID.
        /// </summary>
        /// <param name="productId">The product ID.</param>
        /// <returns>The average rating of the product.</returns>
        public double GetAverageRating(int productId)
        {
            var product = _products.FirstOrDefault(p => p.Id == productId);
            if (product != null && product.Reviews.Any())
            {
                return product.Reviews.Average(r => r.Rating);
            }
            return 0;
        }

        /// <summary>
        /// Saves the product list to a JSON file.
        /// </summary>
        private void SaveProductsToFile()
        {
            var jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "products.json");
            var jsonData = JsonSerializer.Serialize(_products, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(jsonFilePath, jsonData);
        }
    }
}
