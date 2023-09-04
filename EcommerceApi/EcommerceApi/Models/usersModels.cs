using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
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
    public class newusersModels
    {
        public string? u_usersname { get; set; }
        public string? u_password { get; set; }
        public string? u_name { get; set; }
        public string? u_email { get; set; }
        public string? u_phonenumber { get; set; }

    }
    public class PasswordModels
    {

        //[Required(ErrorMessage = "UsersId is required")]
        [RegularExpression(@"\d+", ErrorMessage = "Please enter a valid usersId")]
        public int id_users { get; set; }

        //[Required(ErrorMessage = "Username is required")]
        [RegularExpression(@"^[a-zA-Z0-9_.@#]{4,20}$", ErrorMessage = "Please enter a valid username")]
        public string? username { get; set; }

        [Required(ErrorMessage = "Oldpassword is required")]
        [RegularExpression(@"^[a-zA-Z0-9_.#@]{8,20}$", ErrorMessage = "Please enter a valid oldpassword")]
        public string old_password { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [RegularExpression(@"^[a-zA-Z0-9_.#@]{8,20}$", ErrorMessage = "Please enter a valid password")]
        public string password { get; set; }

        [Required(ErrorMessage = "Recheckpassword is required")]
        [RegularExpression(@"^[a-zA-Z0-9_.#@]{8,20}$", ErrorMessage = "Please enter a valid recheckpassword")]
        public string recheck_password { get; set; }
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

    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;

        [JsonIgnore]
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime TokenCreated { get; set; }
        public DateTime TokenExpires { get; set; }

        public string Role { get; set; }

    }

}
