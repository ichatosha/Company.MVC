using System.ComponentModel.DataAnnotations;

namespace Company.Route.PL.ViewModels
{
	public class ResetPasswordViewModel
	{


		[Required(ErrorMessage = "Password is required !")]
		[DataType(DataType.Password)]
		[MinLength(5, ErrorMessage = "Password Min Length is 5 digits")]
		[MaxLength(25, ErrorMessage = "Password Max Length is 25 digits")]
		public string Password { get; set; }


		[Required(ErrorMessage = "Please Confirm the Password is required !")]
		[Compare(nameof(SignUpViewModel.Password), ErrorMessage = "Please Enter the same Password!")]
		public string ConfirmPassword { get; set; }




	}
}
