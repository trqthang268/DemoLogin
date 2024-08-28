using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace RazorWeb.Models
{
    public class ProductDto
    {
        public int ProductID { get; set; }
        [Required, MaxLength(100)]
        public string ProductName { get; set; }
        [Required]
        [StringLength(500, ErrorMessage = "Product description can't be longer than 500 characters.")]
        public string Description { get; set; }
        [Required]
        [Range(0.01, 10000.00, ErrorMessage = "Price must be between 0.01 and 10000.00")] public decimal Price { get; set; }
        public IFormFile? ImageFile { get; set; }
        public Nullable<long> CategoryID { get; set; }
        public Nullable<long> BrandID { get; set; }
        public Nullable<bool> Active { get; set; }
    }
}
