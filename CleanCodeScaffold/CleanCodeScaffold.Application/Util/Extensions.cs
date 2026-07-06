using AutoMapper;
using CleanCodeScaffold.Application.Dtos;
using CleanCodeScaffold.Domain.Entities;
using FluentValidation.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;
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

        public static async Task<PagerModel<T>> ToPageAsync<T, TM>(this IQueryable<TM> data, int recordsPerPage, int currentPage, IMapper mapper) where T : class where TM : BaseEntity
        {
            PagerModel<T> pager = new PagerModel<T>();
            pager.RecordsPerPage = recordsPerPage;
            pager.CurrentPage = currentPage;
            pager.TotalRecords = await data.CountAsync();
            var vmData = await data.Skip((currentPage - 1) * recordsPerPage).Take(recordsPerPage).ToListAsync();
            pager.PageData = mapper.Map<List<T>>(vmData);
            return pager;
        }

        public static string GetResourceString(this IHttpContextAccessor httpContextAccessor, string resourceString)
        {
            var context = httpContextAccessor.HttpContext;
            if (context == null)
            {
                return resourceString;
            }

            return context.Items.GetResource(resourceString);
        }

        private static string GetResource(this IDictionary<object, object> resourceDictnory, string key)
        {
            var resources = resourceDictnory.TryGetValue("resources", out var currentResourceObj)
                ? currentResourceObj as IReadOnlyDictionary<string, string>
                : null;
            var defaultResources = resourceDictnory.TryGetValue("defaultResources", out var defaultResourceObj)
                ? defaultResourceObj as IReadOnlyDictionary<string, string>
                : null;

            if (resources != null && resources.TryGetValue(key, out var localizedValue) && !string.IsNullOrWhiteSpace(localizedValue))
            {
                return localizedValue;
            }

            if (defaultResources != null && defaultResources.TryGetValue(key, out var defaultValue) && !string.IsNullOrWhiteSpace(defaultValue))
            {
                return defaultValue;
            }

            return key;
        }
    }
}
