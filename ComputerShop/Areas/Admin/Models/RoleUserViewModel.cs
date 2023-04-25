﻿using System.ComponentModel.DataAnnotations;
namespace ComputerShop.Areas.Admin.Models
{
	public class RoleUserViewModel
	{
        [Required]
        [Display(Name = "User")]
        public string UserId { get; set; }

        [Required]
        [Display(Name = "Role")]
        public string RoleId { get; set; }
    }
}

