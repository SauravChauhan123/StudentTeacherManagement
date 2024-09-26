using Application.Common.CQRS.Command;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.CQRS.Validators
{
    public class AddSubjectCommandValidator : AbstractValidator<AddSubjectCommand>
    {
        public AddSubjectCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
            RuleFor(x => x.Class).NotEmpty().WithMessage("Class is required.");
        }
    }
}
