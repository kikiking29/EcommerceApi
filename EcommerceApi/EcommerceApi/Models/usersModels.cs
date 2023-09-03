using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace EcommerceApi.Models
{
    public class usersModels
    {
        public int id_users { get; set; }
        public string? u_usersname { get; set; }
        public string? u_password { get; set; }
        public string? u_name { get; set; }
        public string? u_email { get; set; }
        public string? u_phonenumber { get; set; }

    }
    public class UserDto
    {

        [Required(ErrorMessage = "Username is required")]
        [RegularExpression(@"^[a-zA-Z0-9_.@#]+", ErrorMessage = "Please enter a valid Username")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [RegularExpression(@"^[a-zA-Z0-9_.#@]+", ErrorMessage = "Please enter a valid password")]
        public string? Password { get; set; }
    }
  
}
