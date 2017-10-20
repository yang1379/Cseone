using System.ComponentModel.DataAnnotations;
namespace Cseone.Models
{
    public class RegisterViewModel : BaseEntity
    {
        [Required(ErrorMessage = "First Name is required.")]
        [MinLength(2, ErrorMessage = "Last Name must contain at least 2 characters.")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "First Name can only contain characters.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
 
        [Required(ErrorMessage = "Last Name is required.")]
        [MinLength(2, ErrorMessage = "Last Name must contain at least 2 characters.")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "First Name can only contain characters.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }
 
        [Required(ErrorMessage = "Password is required")]
        [StringLength(255, ErrorMessage = "Must be between 8 and 255 characters", MinimumLength = 8)]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[$@$!%*#?&])[A-Za-z\d$@$!%*#?&]{8,}$", ErrorMessage = "Password must contain at least 1 number, 1 letter, and 1 special character.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
 
        [Required(ErrorMessage = "Confirm Password is required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and confirmation must match.")]
        public string PasswordConfirmation { get; set; }
    }
}
