using CleanCodeScaffold.Application.Dtos;
using CleanCodeScaffold.Application.Util;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanCodeScaffold.Application.Validators
{
    internal class LoginValidator : AbstractValidator<LoginVM>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public LoginValidator(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            RuleFor(x => x.UserName).NotEmpty().WithMessage(_httpContextAccessor.GetResourceString("validations.required"));
            RuleFor(x => x.Password).NotEmpty().WithMessage(_httpContextAccessor.GetResourceString("validations.required"));
        }
    }
}
