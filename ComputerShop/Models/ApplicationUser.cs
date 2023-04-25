using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Identity;

namespace ComputerShop.Models
{
	public class ApplicationUser : IdentityUser
	{
        public override string PasswordHash { get => base.PasswordHash; set => base.PasswordHash = value; }

        public override string UserName { get => base.UserName; set => base.UserName = value; }

        public override string Email { get => base.Email; set => base.Email = value; }

        public string FirstName { get; set; }

		public string LastName { get; set; }

		public static implicit operator ApplicationUser(DateTimeOffset? v)
		{
			throw new NotImplementedException();
		}
	}
}

