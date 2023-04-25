using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComputerShop.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public decimal Price { get; set; }

        public string? Image { get; set; } = null;

        [Required]
        [Display(Name ="Product Manufacturer")]
        public string? ProductManufacturer { get; set; }

        [Required]
        [Display(Name = "Available")]
        public bool IsAvailable { get; set; }

        [Required]
        [Display(Name = "Product Type")]
        public int ProductTypeId { get; set; }

        [ForeignKey("ProductTypeId")]
        public ProductTypes? ProductType { get; set; }

        [Required]
        [Display(Name = "Tag")]
        public int TagId { get; set; }

        [ForeignKey("TagId")]
        public Tag? Tag { get; set; }

    }
}