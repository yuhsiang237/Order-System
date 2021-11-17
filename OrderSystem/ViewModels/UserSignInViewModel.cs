using OrderSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace OrderSystem.ViewModels
{
    public class UserSignInViewModel
    {

        [Required(ErrorMessage = "帳號不可為空")]
        public string Account { get; set; }
        [Required(ErrorMessage = "密碼不可為空")]
        public string Password { get; set; }

    }
}
