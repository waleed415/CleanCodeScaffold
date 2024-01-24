using AutoMapper;
using CleanCodeScaffold.Application.Dtos;
using CleanCodeScaffold.Application.Handlers.Interface;
using CleanCodeScaffold.Application.Util;
using CleanCodeScaffold.Domain.Entities;
using CleanCodeScaffold.Domain.Interfaces;
using FluentValidation;
using FluentValidation.Results;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanCodeScaffold.Application.Handlers.Implimentation
{
    public class BaseHandler<TVM, TModel> : IBaseHandler<TVM> where TVM : class where TModel : BaseEntity 
    {
        protected readonly IBaseRepository<TModel> _repo;
        protected readonly IMapper _mapper;
        protected readonly IValidator<TVM> _validator;

        public BaseHandler(IBaseRepository<TModel> repo, IMapper mapper, IValidator<TVM> validator)
        {
            _repo = repo;
            _mapper = mapper;
            _validator = validator;
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

        public virtual async Task<Response<PagerModel<TVM>>> GetAllAsync(int pageSize = 10, int currentPage = 1)
        {
            Response<PagerModel<TVM>> response = new Response<PagerModel<TVM>>();
            var query = _repo.GetAllAsync();
            //if (filters?.Count > 0)
            //    query = GetDynamicQuery(query, filters);
            List<TModel> data =  query.OrderByDescending(x => x.Id).ToList();
            var list = _mapper.Map<IEnumerable<TVM>>(data);
            response.Data = list.ToPage<TVM>(pageSize, currentPage);
            response.Status = "Success";
            return response;
        }

        //public virtual async Task<Response<IEnumerable<MasterDataModel>>> GetForDropDown(List<KeyValuePair<string, string>> filters = null)
        //{
        //    Response<IEnumerable<MasterDataModel>> response = new Response<IEnumerable<MasterDataModel>>();
        //    var query = _repo.GetAllAsync();
        //    if (filters?.Count > 0)
        //        query = GetDynamicQuery(query, filters, "0");
        //    List<TModel> data = await query.OrderByDescending(x => x.Id).ToListAsync();
        //    response.Data = _mapper.Map<IEnumerable<MasterDataModel>>(data);
        //    response.Status = "Success";
        //    return response;
        //}

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

        private IQueryable<TModel> GetDynamicQuery(IQueryable<TModel> query, List<KeyValuePair<string, string>> filter)
        {
            var type = typeof(TModel);
            var properties = type.GetProperties();
            List<Object> values = new List<Object>();
            string condition = string.Empty;
            int iterator = 0;
            foreach (var item in filter)
            {
                if (condition?.Length > 0)
                    condition = condition + " || ";
                var selected = properties.FirstOrDefault(x => x.Name.ToLower().Equals(item.Key.ToLower()));
                if (selected?.PropertyType.Name.ToLower().Equals("string") == true)
                    condition += $"{item.Key}.ToLower().Contains(@{iterator})";
                else
                    condition += $"{item.Key} = @{iterator}";

                values.Add(item.Value.ToLower());
                iterator++;
            }
            //query = query.Where(condition, values.ToArray());

            return query;
        }
    }
}
