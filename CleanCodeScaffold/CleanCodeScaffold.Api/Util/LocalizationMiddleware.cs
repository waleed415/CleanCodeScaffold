using Newtonsoft.Json;

namespace CleanCodeScaffold.Api.Util
{
    public class LocalizationMiddleware
    {
        private readonly RequestDelegate _next;

        public LocalizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            string lang = context.Request.Query["lang"].FirstOrDefault() ?? "en"; // Default language is English

            // Load language resources from the corresponding JSON file
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Languages", $"{lang}.json");
            var json = File.ReadAllText(filePath);
            var resources = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

            context.Items["resources"] = resources;

            await _next(context);
        }
    }
}
