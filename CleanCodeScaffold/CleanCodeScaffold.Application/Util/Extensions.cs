using AutoMapper;
using CleanCodeScaffold.Application.Dtos;
using CleanCodeScaffold.Domain.Entities;
using FluentValidation.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanCodeScaffold.Application.Util
{
    internal static class Extensions
    {
        public static List<string> ToErrorMessage(this ValidationResult result)
        {
            return result.Errors.Select(x => x.ErrorMessage).ToList();
        }

        public static PagerModel<T> ToPage<T>(this IEnumerable<T> data, int recordsPerPage, int currentPage) where T : class
        {
            PagerModel<T> pager = new PagerModel<T>();
            pager.RecordsPerPage = recordsPerPage;
            pager.CurrentPage = currentPage;
            pager.TotalRecords = data.Count();
            pager.PageData = data.Skip((currentPage - 1) * recordsPerPage).Take(recordsPerPage).ToList();
            return pager;
        }

        public static PagerModel<T> ToPage<T, TM>(this IQueryable<TM> data, int recordsPerPage, int currentPage, IMapper mapper) where T : class where TM : BaseEntity
        {
            PagerModel<T> pager = new PagerModel<T>();
            pager.RecordsPerPage = recordsPerPage;
            pager.CurrentPage = currentPage;
            pager.TotalRecords = data.Count();
            var vmData = data.Skip((currentPage - 1) * recordsPerPage).Take(recordsPerPage).ToList();
            pager.PageData = mapper.Map<List<T>>(vmData);
            return pager;
        }

       

    }
}
