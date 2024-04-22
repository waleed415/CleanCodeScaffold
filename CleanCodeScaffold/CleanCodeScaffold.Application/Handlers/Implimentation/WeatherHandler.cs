using CleanCodeScaffold.Application.Dtos;
using CleanCodeScaffold.Application.Dtos.Configs;
using CleanCodeScaffold.Application.Handlers.Interface;
using CleanCodeScaffold.Application.Mappers;
using CleanCodeScaffold.Application.Util;
using CleanCodeScaffold.Application.Validators;
using CleanCodeScaffold.Domain.Entities;
using CleanCodeScaffold.Domain.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CleanCodeScaffold.Application.Handlers.Implimentation
{
    public class WeatherHandler : BaseHandler<WeatherVM, Weather>, IWeatherHandler
    {
        public WeatherHandler(IWeatherRepository repo, IValidator<WeatherVM> validator,
            FilterValidator<WeatherVM> filterValidator, IHttpContextAccessor httpContextAccessor) 
            : base(repo, WeatherMapper.Mapper, validator, filterValidator, httpContextAccessor)
        {
          
        }

        public async Task<byte[]> GetReport()
        {
            var weathers = _repo.GetAllAsync();
            var reportConfigs = new PDFReportConfig();
            reportConfigs.ReportTitle = "Weather Report";
            string currentDirectory = Directory.GetCurrentDirectory();
            reportConfigs.TemplateURL = Path.Combine(currentDirectory, "Template/pdf.html"); 
            reportConfigs.CssURL = Path.Combine(currentDirectory, "Template/pdf.css");
            reportConfigs.RecordPerPage = 8;
            return await weathers.AsEnumerable().GetPDFContent(reportConfigs);
        }
    }
}
