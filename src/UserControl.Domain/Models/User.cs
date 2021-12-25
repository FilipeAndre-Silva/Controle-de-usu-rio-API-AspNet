using FluentValidation.Results;
using UserControl.Domain.Validations;

namespace UserControl.Domain.Models
{
    public class User : Entity
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } = Roles.Employee;

        public bool IsValid()
        {   
            validationResult = new UserValidator().Validate(this);
            return validationResult.IsValid;
        }
    }
}