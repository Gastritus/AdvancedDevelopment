using System.Collections.Generic;
using AdvancedDevelopment.Models;
using AdvancedDevelopment.Services;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace AdvancedDevelopment.Tests.Services
{
    public class ProductServiceTests
    {
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly ProductService _productService;

        public ProductServiceTests()
        {
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            var sessionMock = new Mock<ISession>();
            _httpContextAccessorMock.Setup(x => x.HttpContext.Session).Returns(sessionMock.Object);
            _productService = new ProductService(_httpContextAccessorMock.Object);
        }

        private void ResetProductService()
        {
            _productService.ClearProducts(); // Ensure this method is implemented in ProductService
        }

        [Fact]
        public void AddProduct_AddsProductToList()
        {
            ResetProductService(); // Clear any existing data

            var product = new Product { Id = 1, Name = "Product 1" };
            _productService.AddProduct(product);
            var result = _productService.GetAllProducts();

            Assert.Contains(result, p => p.Id == product.Id);
        }

        [Fact]
        public void GetProductById_ReturnsCorrectProduct()
        {
            ResetProductService(); // Clear any existing data

            var productId = 1;
            var expectedProduct = new Product
            {
                Id = productId,
                Name = "Running Shoes",
                Price = 99.99m,
                Brand = "Nike",
                Gender = "Male",
                Description = "Comfortable running shoes",
                Reviews = new List<Review>()
            };
            _productService.AddProduct(expectedProduct);

            var result = _productService.GetProductById(productId);

            Assert.NotNull(result);
            Assert.Equal(expectedProduct, result);
        }

        [Fact]
        public void AddReview_AddsReviewToProduct()
        {
            ResetProductService(); // Clear any existing data

            var product = new Product { Id = 1, Name = "Product 1", Reviews = new List<Review>() };
            var review = new Review { Rating = 5, Comment = "Great product!" };
            _productService.AddProduct(product);

            _productService.AddReview(1, review);
            var result = _productService.GetProductById(1);

            Assert.Contains(result.Reviews, r => r.Rating == review.Rating && r.Comment == review.Comment);
        }

        [Fact]
        public void GetAverageRating_ReturnsCorrectAverage()
        {
            ResetProductService(); // Clear any existing data

            var product = new Product
            {
                Id = 1,
                Reviews = new List<Review>
                {
                    new Review { Rating = 5 },
                    new Review { Rating = 4 }
                }
            };
            _productService.AddProduct(product);

            var averageRating = _productService.GetAverageRating(1);

            Assert.Equal(4.5, averageRating);
        }

        [Fact]
        public void GetRelatedProducts_ReturnsCorrectProducts()
        {
            ResetProductService(); // Clear any existing data

            var mainProduct = new Product
            {
                Id = 1,
                Name = "Main Product",
                Brand = "Nike",
                Category = "Sportswear",
                Price = 99.99m,
            };

            var relatedProduct1 = new Product { Id = 2, Name = "Product A", Brand = "Nike", Category = "Sportswear", Price = 49.99m };
            var relatedProduct2 = new Product { Id = 3, Name = "Product B", Brand = "Nike", Category = "Sportswear", Price = 69.99m };
            var unrelatedProduct = new Product { Id = 4, Name = "Unrelated Product", Brand = "Adidas", Category = "Casual", Price = 39.99m };

            _productService.AddProduct(mainProduct);
            _productService.AddProduct(relatedProduct1);
            _productService.AddProduct(relatedProduct2);
            _productService.AddProduct(unrelatedProduct);

            var result = _productService.GetRelatedProducts(mainProduct.Id);

            Assert.Contains(relatedProduct1, result);
            Assert.Contains(relatedProduct2, result);
            Assert.DoesNotContain(unrelatedProduct, result);
        }
    }
}
