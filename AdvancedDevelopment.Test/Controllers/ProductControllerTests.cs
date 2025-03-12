using System.Collections.Generic;
using AdvancedDevelopment.Controllers;
using AdvancedDevelopment.Models;
using AdvancedDevelopment.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace AdvancedDevelopment.Tests.Controllers
{
    public class ProductControllerTests
    {
        private readonly Mock<IProductService> _productServiceMock;
        private readonly ProductController _controller;

        public ProductControllerTests()
        {
            _productServiceMock = new Mock<IProductService>();
            _controller = new ProductController(_productServiceMock.Object);
        }


        [Fact]
        public void Index_AppliesFiltersAndSorting()
        {
            // Arrange
            var mockProductService = new Mock<IProductService>();
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Product1", Price = 100, Brand = "BrandA", Gender = "Male" },
                new Product { Id = 2, Name = "Product2", Price = 200, Brand = "BrandB", Gender = "Female" },
                new Product { Id = 3, Name = "Product3", Price = 150, Brand = "BrandA", Gender = "Male" }
            };
            mockProductService.Setup(service => service.GetAllProducts()).Returns(products);
            var controller = new ProductController(mockProductService.Object);

            // Act
            var result = controller.Index("PriceLowToHigh", 150, "BrandA", "Male") as ViewResult;

            // Assert
            var model = Assert.IsAssignableFrom<IEnumerable<Product>>(result?.Model);

            // Only two products match all filters: Price <= 150, BrandA, Male
            var expectedProducts = products
                .Where(p => p.Price <= 150 && p.Brand == "BrandA" && p.Gender == "Male")
                .OrderBy(p => p.Price)
                .ToList();

            Assert.Equal(expectedProducts.Count, model.Count());
            Assert.Equal(expectedProducts.First().Id, model.First().Id); // First product should be Product1
            Assert.Equal(expectedProducts.Last().Id, model.Last().Id);   // Last product should be Product3
        }



        [Fact]
        public void Details_ReturnsNotFound_ForInvalidId()
        {
            // Arrange
            _productServiceMock.Setup(s => s.GetProductById(It.IsAny<int>())).Returns<Product>(null);

            // Act
            var result = _controller.Details(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Details_ReturnsViewResult_WithProductAndRelatedData()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Product 1" };
            var reviews = new List<Review> { new Review { Rating = 5, Comment = "Great!" } };
            var relatedProducts = new List<Product> { new Product { Id = 2, Name = "Product 2" } };
            var recentlyViewedProducts = new List<Product> { new Product { Id = 3, Name = "Product 3" } };

            _productServiceMock.Setup(s => s.GetProductById(1)).Returns(product);
            _productServiceMock.Setup(s => s.GetReviewsByProductId(1)).Returns(reviews);
            _productServiceMock.Setup(s => s.GetAverageRating(1)).Returns(5.0);
            _productServiceMock.Setup(s => s.GetRelatedProducts(1)).Returns(relatedProducts);
            _productServiceMock.Setup(s => s.GetRecentlyViewed()).Returns(recentlyViewedProducts);

            // Act
            var result = _controller.Details(1) as ViewResult;

            // Assert
            var model = Assert.IsAssignableFrom<Product>(result?.Model);
            Assert.Equal(product, model);

            Assert.Equal(reviews, result?.ViewData["Reviews"]);
            Assert.Equal(5.0, result?.ViewData["AverageRating"]);
            Assert.Equal(relatedProducts, result?.ViewData["RelatedProducts"]);
            Assert.Equal(recentlyViewedProducts, result?.ViewData["RecentlyViewedProducts"]);
        }

    }
}
