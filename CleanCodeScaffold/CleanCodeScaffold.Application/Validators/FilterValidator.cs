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
    public class FilterValidator<T>  : AbstractValidator<IEnumerable<FilterVM>> where T : class
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public FilterValidator(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            RuleForEach(filter => filter).Must(IsValidPropertyName).WithMessage(_httpContextAccessor.GetResourceString("validations.filter.key"));
        }

        private bool IsValidPropertyName(FilterVM filter)
        {
            var viewModelProperties = typeof(T).GetProperties()
                                        .Select(property => property.Name.ToLower());
            return viewModelProperties.Contains(filter.Key.ToLower());
        }
    }
}
