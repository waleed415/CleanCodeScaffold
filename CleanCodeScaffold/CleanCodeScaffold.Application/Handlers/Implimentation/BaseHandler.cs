using AutoMapper;
using CleanCodeScaffold.Application.Dtos;
using CleanCodeScaffold.Application.Handlers.Interface;
using CleanCodeScaffold.Application.Util;
using CleanCodeScaffold.Domain.Entities;
using CleanCodeScaffold.Domain.Interfaces;
using FluentValidation;
using FluentValidation.Results;
using AutoMapper.Internal;
using CleanCodeScaffold.Application.Exceptions;
using CleanCodeScaffold.Application.Validators;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Globalization;



namespace CleanCodeScaffold.Application.Handlers.Implimentation
{
    public class BaseHandler<TVM, TModel> : IBaseHandler<TVM> where TVM : class where TModel : BaseEntity
    {
        protected readonly IBaseRepository<TModel> _repo;
        protected readonly IMapper _mapper;
        protected readonly IValidator<TVM> _validator;
        protected readonly FilterValidator<TVM> _filterValidator;

        public BaseHandler(IBaseRepository<TModel> repo, IMapper mapper, IValidator<TVM> validator, FilterValidator<TVM> filterValidator)
        {
            _repo = repo;
            _mapper = mapper;
            _validator = validator;
            _filterValidator = filterValidator;
        }

        public virtual async Task<Response<TVM>> CreateAsync(TVM model)
        {
            Response<TVM> response = new Response<TVM>();
            ValidationResult result = await _validator.ValidateAsync(model);
            if (result.IsValid)
            {
                TModel obj = _mapper.Map<TModel>(model);
                obj.IsActive = true;
                await _repo.CreateAsync(obj);
                response.Data = model;
                response.Status = "Success";
            }
            else
            {
                response.Status = "Error";
                response.Message = result.ToErrorMessage();
            }
            return response;
        }

        public virtual async Task<Response<TVM>> DeleteAsync(long Id)
        {
            await _repo.DeleteAsync(Id);
            Response<TVM> response = new Response<TVM>();
            response.Status = "Success";
            response.Message.Add("Entity Deleted Successfully!");
            return response;
        }

        public virtual async Task<Response<PagerModel<TVM>>> GetAllAsync(int pageSize = 10, int currentPage = 1, IEnumerable<FilterVM> filters = null)
        {
            Response<PagerModel<TVM>> response = new Response<PagerModel<TVM>>() { Status = "Error" };
            IQueryable<TModel> queryable;
            if (filters == null || filters.Count() < 1)
                queryable = _repo.GetAllAsync();
            else
            {
                var validationResult = await _filterValidator.ValidateAsync(filters);
                if (!validationResult.IsValid)
                {
                    response.Message = validationResult.ToErrorMessage();
                    return response;
                }
                queryable = _repo.GetAllAsync(GetFitlerPredicate(filters));
            }
            response.Data = await queryable.ToPageAsync<TVM, TModel>(pageSize, currentPage, _mapper);
            response.Status = "Success";
            return response;
        }


        public virtual async Task<Response<TVM>> GetByIdAsync(long Id)
        {
            Response<TVM> response = new Response<TVM>();
            response.Status = "Success";
            TModel model = await _repo.GetByIdAsync(Id);
            response.Data = _mapper.Map<TVM>(model);
            return response;
        }

        public virtual async Task<Response<TVM>> UpdateAsync(long Id, TVM model)
        {

            Response<TVM> response = new Response<TVM>();
            ValidationResult result = await _validator.ValidateAsync(model);
            if (result.IsValid)
            {
                TModel obj = _mapper.Map<TModel>(model);
                await _repo.UpdateAsync(obj);
                response.Data = model;
                response.Status = "Success";
            }
            else
            {
                response.Status = "Error";
                response.Message = result.ToErrorMessage();
            }
            return response;
        }

        private string GetDBModelPropertyByVMProperty<TSrc, TDest>(string vmProperty, IConfigurationProvider configuration)
        {
            var internalAPI = InternalApi.Internal(configuration);
            var map = internalAPI.FindTypeMapFor<TSrc, TDest>();
            var destProp = map.PropertyMaps.FirstOrDefault(x => x.SourceMember?.Name.ToLower().Equals(vmProperty.ToLower()) == true)?
                                .DestinationMember?.Name;
            return destProp;
        }

        private Expression<Func<TModel, bool>> GetFitlerPredicate(IEnumerable<FilterVM> filters)
        {
            var parameter = Expression.Parameter(typeof(TModel), "x");
            Expression predicate = Expression.New(typeof(TModel));
            string? oldOperator = string.Empty;
            foreach (var filter in filters)
            {
                var property = Expression.Property(parameter, GetDBModelPropertyByVMProperty<TVM, TModel>(filter.Key, _mapper.ConfigurationProvider));
                var constant = Expression.Constant(Convert.ChangeType(filter.Value, property.Type));
                var binaryExpression = GetBinaryExpression(property, constant, filter.Operator);
                var filterPredicate = Expression.Lambda<Func<TModel, bool>>(binaryExpression, parameter).Body;
                if (!string.IsNullOrEmpty(oldOperator))
                    predicate = CombinePredicates(predicate, filterPredicate, oldOperator);
                else
                    predicate = filterPredicate;
                oldOperator = filter.PostOperator;
            }

            return Expression.Lambda<Func<TModel, bool>>(predicate, parameter);
        }

        private Expression CombinePredicates(Expression left, Expression right, string postOperator)
        {
            switch (postOperator.ToLower())
            {
                case "and":
                    return Expression.AndAlso(left, right);
                case "or":
                    return Expression.OrElse(left, right);
                default:
                    throw new NotSupportedException($"PostOperator '{postOperator}' is not supported.");
            }
        }

        private Expression GetBinaryExpression(Expression left, Expression right, string @operator)
        {
            switch (@operator.ToLower())
            {
                case "equals":
                    return Expression.Equal(left, right);
                case "contains":
                    return Expression.Call(left, typeof(string).GetMethod("Contains", new[] { typeof(string) }), right);
                case "lessthan":
                    return Expression.LessThan(left, right);
                case "greaterthan":
                    return Expression.GreaterThan(left, right);
                case "lessthanorequal":
                    return Expression.LessThanOrEqual(left, right);
                case "greaterthanorequal":
                    return Expression.GreaterThanOrEqual(left, right);
                case "startswith":
                    return Expression.Call(left, typeof(string).GetMethod("StartsWith", new[] { typeof(string) }), right);
                case "endswith":
                   return Expression.Call(left, typeof(string).GetMethod("EndsWith", new[] { typeof(string) }), right);
                // Add more cases for other operators as needed
                default:
                    throw new NotSupportedException($"Operator '{@operator}' is not supported.");
            }
        }
    }
}
