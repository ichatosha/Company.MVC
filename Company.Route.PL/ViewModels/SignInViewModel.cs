using System.ComponentModel.DataAnnotations;

namespace Company.Route.PL.ViewModels
{
	public class SignInViewModel
	{
		[Required(ErrorMessage = "Email is required !")]
		[EmailAddress(ErrorMessage = "Invalid Email !")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Password is required !")]
		[DataType(DataType.Password)]
		[MinLength(5, ErrorMessage = "Password Min Length is 5 digits")]
		[MaxLength(25, ErrorMessage = "Password Max Length is 25 digits")]
		public string Password { get; set; }

		public bool RememberMe { get; set; }

	}
}
