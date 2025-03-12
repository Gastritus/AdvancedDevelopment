using Microsoft.AspNetCore.Mvc.ViewEngines;
using System.Collections.Generic;
using System.Globalization;

namespace AdvancedDevelopment.Models
{
    public class Product
    {
        public int Id { get; set; }             // Unique identifier for the product
        public string Name { get; set; }        // Name of the product
        public string Description { get; set; } // Description of the product
        public decimal Price { get; set; }      // Price of the product
        public string ImageUrl { get; set; }    // URL for the product image
        public string Brand { get; set; }       // Brand of product
        public string Gender { get; set; }      // Target gender
        public string Category { get; set; }    // Category of shoe i.e running, walking, hiking
        public List<Review> Reviews { get; set; } = new List<Review>(); // Reviews associated with the product

        public string FormattedPrice => Price.ToString("C", new CultureInfo("en-GB")); // Change currencey to GBP
    }

}