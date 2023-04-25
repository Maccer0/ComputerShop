using System;
using System.ComponentModel.DataAnnotations;

namespace ComputerShop.Models
{
	public class Order
	{
		public Order()
		{
			Details = new List<OrderDetails>();
			OrderDate = DateTime.Now;
		}

		[Key]
		public int Id { get; set; }

		[Display(Name = "Order No")]
		public string? OrderNo { get; set; }

		[Required]
		public string Name { get; set; }

		[Required]
		[Display(Name = "Phone Number")]
		public string PhoneNo { get; set; }

		[Required]
		[EmailAddress]
		public string Email { get; set; }

		[Required]
		public string Address { get; set; }

		public DateTime OrderDate { get; set; }

		[Required]
		public List<OrderDetails>? Details { get; set; }
	}
}

