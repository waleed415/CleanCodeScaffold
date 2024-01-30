using CleanCodeScaffold.Application.Dtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanCodeScaffold.Application.Validators
{
    public class ChangePasswordValidator : AbstractValidator<ChangePasswordVM>
    {
        public ChangePasswordValidator()
        {
            RuleFor(x => x.NewPassword).NotEmpty().WithMessage("New Passowrd is required.");
            RuleFor(x => x.OldPassword).NotEmpty().WithMessage("Passowrd is required.");
        }
    }
}
