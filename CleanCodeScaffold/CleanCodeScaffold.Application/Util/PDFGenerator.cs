using CleanCodeScaffold.Application.Dtos.Configs;
using CleanCodeScaffold.Application.Handlers.Implimentation;
using Microsoft.Extensions.Logging;
using PuppeteerSharp;
using PuppeteerSharp.Media;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;


namespace CleanCodeScaffold.Application.Util
{
    internal static class PDFGenerator
    {
        public static async Task<byte[]> GetPDFContent<T>(this IEnumerable<T> data, PDFReportConfig configs)
        {
            StringBuilder htmlBuilder = new StringBuilder();
            htmlBuilder.AppendLine("<!DOCTYPE html>");
            htmlBuilder.AppendLine("<html lang=\"en\">");
            htmlBuilder.AppendLine("<head>\r\n");
            htmlBuilder.AppendLine("<meta charset=\"UTF-8\">");
            htmlBuilder.AppendLine("<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">");
            htmlBuilder.AppendLine("<title>Header Design</title>");
            htmlBuilder.AppendLine("</head>");
            htmlBuilder.AppendLine("<body>");
            htmlBuilder.AppendLine("<style>");
            htmlBuilder.AppendLine(await File.ReadAllTextAsync(configs.CssURL));
            htmlBuilder.AppendLine("</style>");
            var htmlTables = GetHTMLTablesFromList(data, configs.RecordPerPage);
            string htmlContent = await File.ReadAllTextAsync(configs.TemplateURL);
            int pageCount = 1;
            foreach (var htmlTable in htmlTables)
            {
                string htmltbl = htmlContent.Replace("[content]", htmlTable);
                htmltbl = htmltbl.Replace("[title]", configs.ReportTitle);
                htmltbl = htmltbl.Replace("[pager]", $"Page {pageCount} of {htmlTables.Count}");
                htmlBuilder.AppendLine(htmltbl);
                pageCount++;
            }
            htmlBuilder.AppendLine("</body>");
            htmlBuilder.AppendLine("</html>");
            return await RenderHtmlToPdf(htmlBuilder.ToString());
        }

        private static async Task<byte[]> RenderHtmlToPdf(string htmlContent)
        {
            await new BrowserFetcher().DownloadAsync();
            var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true });

            var page = await browser.NewPageAsync();
            await page.SetContentAsync(htmlContent);
            var pdfBytes = await page.PdfDataAsync(new PdfOptions
            {
                Format = PaperFormat.A4,
                PrintBackground = true,
            });
            await browser.CloseAsync();

            return pdfBytes;
        }

        private static List<string> GetHTMLTablesFromList<T>(IEnumerable<T> data, int recordPerPage)
        {
            List<string> result = new List<string>();
            int totalPages = CalculateTotalPages(data.Count(), recordPerPage);
            for (int i = 1; i <= totalPages; i++)
            {
                result.Add(ListToHTML(data.Skip((i - 1) * recordPerPage).Take(recordPerPage)));
            }
            return result;
        }
        private static int CalculateTotalPages(int totalItems, int itemsPerPage)
        {
            if (itemsPerPage <= 0)
            {
                throw new ArgumentException("Items per page must be greater than zero.", nameof(itemsPerPage));
            }

            int totalPages = totalItems / itemsPerPage;

            // Check if there are any remaining items after dividing by itemsPerPage
            if (totalItems % itemsPerPage != 0)
            {
                totalPages++;
            }

            return totalPages;
        }

        private static string ListToHTML<T>(IEnumerable<T> data)
        {
            StringBuilder htmlBuilder = new StringBuilder();
            if (data != null)
            {
                htmlBuilder.AppendLine("<div class='table-container'>");
                // Start a table for each object
                htmlBuilder.AppendLine("<table>");

                // Add table rows for each property in the object
                var properties = typeof(T).GetProperties();
                htmlBuilder.AppendLine("<thead>");
                htmlBuilder.AppendLine("<tr>");
                foreach (var prop in properties)
                {
                    htmlBuilder.AppendLine($"<th>{prop.Name}</th>");
                }
                htmlBuilder.AppendLine("</tr>");
                htmlBuilder.AppendLine("</thead>");
                htmlBuilder.AppendLine("<tbody>");
                foreach (var item in data)
                {
                    htmlBuilder.AppendLine("<tr>");
                    foreach (var prop in properties)
                    {
                        htmlBuilder.AppendLine($"<td>{prop.GetValue(item)}</td>");
                    }
                    htmlBuilder.AppendLine("</tr>");
                }
                htmlBuilder.AppendLine("</tbody>");
                htmlBuilder.AppendLine("</table>");
                htmlBuilder.AppendLine("</div>");
            }
            return htmlBuilder.ToString();
        }

    }
}
