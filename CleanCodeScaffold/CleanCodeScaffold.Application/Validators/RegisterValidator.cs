using CleanCodeScaffold.Application.Dtos;
using CleanCodeScaffold.Application.Util;
using CleanCodeScaffold.Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Http;
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
        private readonly IHttpContextAccessor _httpContextAccessor;
        public RegisterValidator(UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            RuleFor(x => x.Email).NotEmpty().WithMessage(_httpContextAccessor.GetResourceString("validations.required")).CustomAsync(CheckEmail);
            RuleFor(x => x.Password).NotEmpty().WithMessage(_httpContextAccessor.GetResourceString("validations.required"));
            RuleFor(x => x.UserName).NotEmpty().WithMessage(_httpContextAccessor.GetResourceString("validations.required"));
        }

        private async Task CheckEmail(string email, ValidationContext<RegisterVM> context,
            CancellationToken token)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
                context.AddFailure(_httpContextAccessor.GetResourceString("validations.email.registerd"));
        }

        private async Task CheckUserName(string name, ValidationContext<RegisterVM> context,
           CancellationToken token)
        {
            var user = await _userManager.FindByNameAsync(name);
            if (user != null)
                context.AddFailure(_httpContextAccessor.GetResourceString("validations.email.registerd"));
        }
    }
}
