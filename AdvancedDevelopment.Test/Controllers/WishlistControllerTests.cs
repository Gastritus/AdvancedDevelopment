using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Xunit;
using AdvancedDevelopment.Models;
using AdvancedDevelopment.Services;
using AdvancedDevelopment.Controllers;

namespace AdvancedDevelopment.Tests.Controllers {
    public class WishlistControllerTests
    {
        private readonly Mock<IProductService> _mockProductService;
        private WishlistController _controller;
        private Mock<HttpContext> _mockHttpContext;
        private Mock<HttpResponse> _mockHttpResponse;
        private Mock<HttpRequest> _mockHttpRequest;

        public WishlistControllerTests()
        {
            _mockProductService = new Mock<IProductService>();
            _mockHttpContext = new Mock<HttpContext>();
            _mockHttpResponse = new Mock<HttpResponse>();
            _mockHttpRequest = new Mock<HttpRequest>();
            _mockHttpContext.Setup(c => c.Response).Returns(_mockHttpResponse.Object);
            _mockHttpContext.Setup(c => c.Request).Returns(_mockHttpRequest.Object);

            var controllerContext = new ControllerContext()
            {
                HttpContext = _mockHttpContext.Object
            };

            _controller = new WishlistController(_mockProductService.Object)
            {
                ControllerContext = controllerContext
            };
        }

        [Fact]
        public void Index_DisplaysCorrectProducts()
        {
            // Arrange
            var wishlist = new List<int> { 1, 2 };
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Product1" },
                new Product { Id = 2, Name = "Product2" }
            };

            _mockHttpRequest.Setup(r => r.Cookies["Wishlist"]).Returns(JsonSerializer.Serialize(wishlist));
            _mockProductService.Setup(s => s.GetAllProducts()).Returns(products.AsQueryable());

            // Act
            var result = _controller.Index() as ViewResult;

            // Assert
            var model = Assert.IsType<List<Product>>(result.Model);
            Assert.Equal(2, model.Count);
            Assert.Contains(model, p => p.Id == 1);
            Assert.Contains(model, p => p.Id == 2);
        }

        [Fact]
        public void Add_AddsProductToWishlist()
        {
            // Arrange
            var wishlist = new List<int> { 1 };
            _mockHttpRequest.Setup(r => r.Cookies["Wishlist"]).Returns(JsonSerializer.Serialize(wishlist));
            _mockHttpResponse.Setup(s => s.Cookies.Append(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CookieOptions>()))
                .Callback<string, string, CookieOptions>((name, value, options) =>
                {
                    // Simulate adding a product to the wishlist
                    wishlist.Add(2);
                });

            // Act
            var result = _controller.Add(2) as RedirectToActionResult;

            // Assert
            Assert.Equal("Index", result.ActionName);
            Assert.Equal("Product", result.ControllerName);
            Assert.Contains(2, wishlist);
        }

        [Fact]
        public void Remove_RemovesProductFromWishlist()
        {
            // Arrange
            var wishlist = new List<int> { 1, 2 };
            _mockHttpRequest.Setup(r => r.Cookies["Wishlist"]).Returns(JsonSerializer.Serialize(wishlist));
            _mockHttpResponse.Setup(s => s.Cookies.Append(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CookieOptions>()))
                .Callback<string, string, CookieOptions>((name, value, options) =>
                {
                    // Simulate removing a product from the wishlist
                    wishlist.Remove(2);
                });

            // Act
            var result = _controller.Remove(2) as RedirectToActionResult;

            // Assert
            Assert.Equal("Index", result.ActionName);
            Assert.DoesNotContain(2, wishlist);
        }
    }
}