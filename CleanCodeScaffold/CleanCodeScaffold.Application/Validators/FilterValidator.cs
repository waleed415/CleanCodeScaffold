using CleanCodeScaffold.Application.Dtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanCodeScaffold.Application.Validators
{
    public class FilterValidator<T>  : AbstractValidator<IEnumerable<FilterVM>> where T : class
    {
        public FilterValidator()
        {
            RuleForEach(filter => filter).Must(IsValidPropertyName).WithMessage("Key must be a valid property in VM.");
        }

        private bool IsValidPropertyName(FilterVM filter)
        {
            var viewModelProperties = typeof(T).GetProperties()
                                        .Select(property => property.Name.ToLower());
            return viewModelProperties.Contains(filter.Key.ToLower());
        }
    }
}
