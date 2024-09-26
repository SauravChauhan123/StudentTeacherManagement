using Application.Common.CQRS.Command;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.CQRS.Validators
{
    public class AddStudentCommandValidator : AbstractValidator<AddStudentCommand>
    {
        public AddStudentCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
            RuleFor(x => x.Age).GreaterThan(0).WithMessage("Age must be greater than 0.");
            RuleFor(x => x.Class).NotEmpty().WithMessage("Class is required.");
            RuleFor(x => x.RollNumber).NotEmpty().WithMessage("Roll Number is required.");
            RuleFor(x => x.ImageFile).NotEmpty().WithMessage("Image is required");
        }
    }

}
