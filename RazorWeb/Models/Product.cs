﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
namespace RazorWeb.Models
{
    public class Product
    {
        [Key]
        public int ProductID { get; set; }
        [MaxLength(100)]
        public string ProductName { get; set; }
        public string Description { get; set; }
        [Precision(16, 2)]
        public decimal Price { get; set; }
        public string ImageFileName { get; set; }
        public DateTime CreateAt { get; set; }
        public Nullable<long> CategoryID { get; set; }
        public Nullable<long> BrandID { get; set; }
        public Nullable<bool> Active { get; set; }
        public virtual Brand Brand { get; set; }
        public virtual Category Category { get; set; }
    }
}
