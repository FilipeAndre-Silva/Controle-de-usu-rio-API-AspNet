using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation.Results;

namespace UserControl.Domain.Models
{
    public abstract class Entity
    {
        [NotMapped]
        public ValidationResult validationResult { get; set; }
    }
}