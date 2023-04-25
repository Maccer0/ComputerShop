using System.ComponentModel.DataAnnotations;

namespace ComputerShop.Models
{
    public class Tag
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Tag")]
        public string TagName { get; set; }
    }
}