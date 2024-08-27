using System.ComponentModel.DataAnnotations;

namespace RazorWeb.Models
{
    public class Brand
    {
        [Key]
        public long BrandID { get; set; }
        public string BrandName { get; set; }
    }
}
