using System.Collections.Generic;
using AdvancedDevelopment.Models;

namespace AdvancedDevelopment.Services
{
    public interface IProductService
    {
        IEnumerable<Product> GetAllProducts();
        Product GetProductById(int id);
        void AddProduct(Product product);
        void AddReview(int productId, Review review);
        IEnumerable<Review> GetReviewsByProductId(int id);
        IEnumerable<string> GetAllBrands();
        double GetAverageRating(int id);
        IEnumerable<Product> GetRelatedProducts(int productId);
        void TrackRecentlyViewed(int productId);
        IEnumerable<Product> GetRecentlyViewed();
    }
}
