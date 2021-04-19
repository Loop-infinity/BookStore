using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BookStore.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string ISBN { get; set; }
        public string Author { get; set; }
        [Required]
        [Range(1,10000)]
        public double Price { get; set; }
        [Required]
        [Range(1, 10000)]
        public double ListPrice { get; set; }

        public string ImageUrl { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

    }
}
