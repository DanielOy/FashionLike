using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class RegisterDto
    {
        [Required]
        [RegularExpression(@"[a-zA-Z\s]{2,}", ErrorMessage = "Only alphabetic characters and spaces are allowed")]
        public string FirstName { get; set; }

        [Required]
        [RegularExpression(@"[a-zA-Z\s]{2,}", ErrorMessage = "Only alphabetic characters and spaces are allowed")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"(?=^.{6,10}$)(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&amp;*()_+}{&quot;:;'?/&gt;.&lt;,])(?!.*\s).*$",
            ErrorMessage = "Password must have 1 Uppercase, 1 Lowercase, 1 number, 1 non alphanumeric and at least 8 characters")]
        public string Password { get; set; }
    }
}
