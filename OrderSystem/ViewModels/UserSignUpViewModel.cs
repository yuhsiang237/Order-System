using OrderSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace OrderSystem.ViewModels
{
    public class UserSignUpViewModel
    {
        [Required(ErrorMessage = "姓名不可為空")]
        public string Name { get; set; }
        [Required(ErrorMessage = "信箱不可為空")]

        public string Email { get; set; }
        [Required(ErrorMessage = "帳號不可為空")]

        public string Account { get; set; }
        [Required(ErrorMessage = "密碼不可為空")]

        public string Password { get; set; }

        [Required(ErrorMessage = "密碼不可為空")]
        public string Password2 { get; set; }

    }
}
