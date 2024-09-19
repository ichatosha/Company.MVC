using System.ComponentModel.DataAnnotations;

namespace Company.Route.PL.ViewModels
{
	public class SignUpViewModel
	{
        [Required(ErrorMessage ="User Name is required !")]
        public string UserName { get; set; }


        [Required(ErrorMessage = "First Name is required !")]
        public string FirstName { get; set; }


        [Required(ErrorMessage = "Last Name is required !")]
        public string LastName { get; set; }


        [Required(ErrorMessage = "Email is required !")]
        [EmailAddress(ErrorMessage ="Invalid Email !")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Password is required !")]
        [DataType(DataType.Password)]
        [MinLength(5, ErrorMessage = "Password Min Length is 5 digits")]
        [MaxLength(25, ErrorMessage = "Password Max Length is 25 digits")]
        public string Password { get; set; }


        [Required(ErrorMessage = "Please Confirm the Password is required !")]
        [Compare(nameof(SignUpViewModel.Password), ErrorMessage = "Please Enter the same Password!")]
        public string ConfirmPassword { get; set; }


        [Required(ErrorMessage = "Check is required !")]
        public bool IsAgree { get; set; }


    }
}
