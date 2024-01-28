using CleanCodeScaffold.Application.Dtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanCodeScaffold.Application.Validators
{
    public class ResetPasswordValidator : AbstractValidator<ResetPasswordVM>
    {
        public ResetPasswordValidator()
        {
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required.");
            RuleFor(x => x.Token).NotEmpty().WithMessage("Token is required.");
            RuleFor(x => x.EmailorPhone).NotEmpty().WithMessage("Email or Phone is required.");
        }
    }
}
