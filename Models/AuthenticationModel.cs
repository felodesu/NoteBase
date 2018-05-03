using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace NoteBase.Models
{
    public class AuthenticationModel
    {
        [Required]
        [Remote(action: "CheckUserExists", controller: "Base", ErrorMessage = "Login not found")]
        public string Login { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Remote(action: "ValidateUser", controller: "Base", AdditionalFields = "Login", ErrorMessage = "Password is incorrect")]
        public string Password { get; set; }

    }
}
