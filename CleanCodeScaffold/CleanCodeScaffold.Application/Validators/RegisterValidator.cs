using CleanCodeScaffold.Application.Dtos;
using CleanCodeScaffold.Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanCodeScaffold.Application.Validators
{
    internal class RegisterValidator : AbstractValidator<RegisterVM>
    {
        private readonly UserManager<User> _userManager;
        public RegisterValidator(UserManager<User> userManager)
        {
            _userManager = userManager;
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required.").CustomAsync(CheckEmail);
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required.");
            RuleFor(x => x.UserName).NotEmpty().WithMessage("User name is required.");
        }

        private async Task CheckEmail(string email, ValidationContext<RegisterVM> context,
            CancellationToken token)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
                context.AddFailure("Email already register");
        }

        private async Task CheckUserName(string name, ValidationContext<RegisterVM> context,
           CancellationToken token)
        {
            var user = await _userManager.FindByNameAsync(name);
            if (user != null)
                context.AddFailure("User name already register");
        }
    }
}
