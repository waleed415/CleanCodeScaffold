using AutoMapper;
using CleanCodeScaffold.Application.Dtos;
using CleanCodeScaffold.Domain.Entities;
using FluentValidation.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
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
            return httpContextAccessor.HttpContext.Items.GetResource(resourceString);
        }
        private static string GetResource(this IDictionary<object, object> resourceDictnory, string key)
        {
            IDictionary<string, object> data = null;
            string resource = string.Empty;

            if (resourceDictnory.ContainsKey("resources"))
                data = (Dictionary<string, object>)resourceDictnory["resources"];
            else
                throw new Exception("No language resources found.");

            if (key.Contains("."))
            {
                resource = GetResourceFromObject(data, key);
            }
            else
                resource = resourceDictnory.ContainsKey(key) ? resourceDictnory[key]?.ToString() : string.Empty;
            return resource;
        }

        private static string GetResourceFromObject(IDictionary<string, object> data, string key)
        {
            string resource = string.Empty;

            var keys = key.Split(".");
            string lastKey = keys[keys.Length - 1];
            if (keys.Length > 0)
            {
                IDictionary<string, object> obj = data;
                for (int i = 0; i < keys.Length - 1; i++)
                {
                    string item = keys[i];
                    obj = JsonConvert.DeserializeObject<Dictionary<string, object>>(obj[item].ToString());
                }
                resource = obj.ContainsKey(lastKey) ? obj[lastKey]?.ToString() : string.Empty;
            }
            return resource;
        }
    }
}
