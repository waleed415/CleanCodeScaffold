using CleanCodeScaffold.Application.Dtos;
using CleanCodeScaffold.Application.Util;
using FluentValidation;
using Microsoft.AspNetCore.Http;


namespace CleanCodeScaffold.Application.Validators
{
    public class ResetPasswordValidator : AbstractValidator<ResetPasswordVM>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ResetPasswordValidator(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            RuleFor(x => x.Password).NotEmpty().WithMessage(_httpContextAccessor.GetResourceString("validations.required"));
            RuleFor(x => x.Token).NotEmpty().WithMessage(_httpContextAccessor.GetResourceString("validations.required"));
            RuleFor(x => x.EmailorPhone).NotEmpty().WithMessage(_httpContextAccessor.GetResourceString("validations.required"));
        }
    }
}
