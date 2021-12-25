using System.ComponentModel.DataAnnotations;

namespace UserControl.Api.ViewModels.User
{
    public class LoginUserViewModel
    {
        [Required(ErrorMessage = "Este campo é obrigatório")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório")]
        public string Password { get; set; }
    }
}