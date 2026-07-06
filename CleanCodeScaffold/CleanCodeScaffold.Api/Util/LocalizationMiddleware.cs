using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json.Linq;

namespace CleanCodeScaffold.Api.Util
{
    public class LocalizationMiddleware
    {
        private const string ResourcesKey = "resources";
        private const string DefaultResourcesKey = "defaultResources";
        private const string DefaultLanguage = "en";

        private readonly RequestDelegate _next;
        private readonly IMemoryCache _cache;
        private readonly ILogger<LocalizationMiddleware> _logger;

        public LocalizationMiddleware(RequestDelegate next, IMemoryCache cache, ILogger<LocalizationMiddleware> logger)
        {
            _next = next;
            _cache = cache;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            string requestedLanguage = context.Request.Query["lang"].FirstOrDefault() ?? GetLanguageFromHeader(context);
            string lang = NormalizeLanguage(requestedLanguage);

            var resources = await GetOrLoadResourcesAsync(lang);
            var defaultResources = lang.Equals(DefaultLanguage, StringComparison.OrdinalIgnoreCase)
                ? resources
                : await GetOrLoadResourcesAsync(DefaultLanguage);

            context.Items[ResourcesKey] = resources;
            context.Items[DefaultResourcesKey] = defaultResources;

            await _next(context);
        }

        private static string GetLanguageFromHeader(HttpContext context)
        {
            var header = context.Request.Headers.AcceptLanguage.ToString();
            if (string.IsNullOrWhiteSpace(header))
            {
                return DefaultLanguage;
            }

            var first = header.Split(',', StringSplitOptions.RemoveEmptyEntries)
                .FirstOrDefault();
            if (string.IsNullOrWhiteSpace(first))
            {
                return DefaultLanguage;
            }

            return first.Split(';', StringSplitOptions.RemoveEmptyEntries)
                .FirstOrDefault() ?? DefaultLanguage;
        }

        private static string NormalizeLanguage(string language)
        {
            if (string.IsNullOrWhiteSpace(language))
            {
                return DefaultLanguage;
            }

            var normalized = language.Trim().Replace('_', '-').ToLowerInvariant();
            var segments = normalized.Split('-', StringSplitOptions.RemoveEmptyEntries);
            if (segments.Length < 1)
            {
                return DefaultLanguage;
            }

            return segments[0];
        }

        private Task<IReadOnlyDictionary<string, string>> GetOrLoadResourcesAsync(string lang)
        {
            return _cache.GetOrCreateAsync($"localization:{lang}", async entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromMinutes(30);
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(6);

                var loaded = await TryLoadLanguageResourcesAsync(lang);
                if (loaded.Count > 0)
                {
                    return loaded;
                }

                if (!lang.Equals(DefaultLanguage, StringComparison.OrdinalIgnoreCase))
                {
                    _logger.LogWarning("Localization file for language '{Lang}' was not found or empty. Falling back to '{DefaultLanguage}'.", lang, DefaultLanguage);
                    return await TryLoadLanguageResourcesAsync(DefaultLanguage);
                }

                return loaded;
            });
        }

        private async Task<IReadOnlyDictionary<string, string>> TryLoadLanguageResourcesAsync(string lang)
        {
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Languages", $"{lang}.json");
            if (!File.Exists(filePath))
            {
                return new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            }

            try
            {
                var json = await File.ReadAllTextAsync(filePath);
                var token = JToken.Parse(json);
                var flattened = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                FlattenResource(token, string.Empty, flattened);
                return flattened;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load localization file for language '{Lang}'.", lang);
                return new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            }
        }

        private static void FlattenResource(JToken token, string prefix, IDictionary<string, string> result)
        {
            if (token is JObject obj)
            {
                foreach (var property in obj.Properties())
                {
                    var key = string.IsNullOrEmpty(prefix) ? property.Name : $"{prefix}.{property.Name}";
                    FlattenResource(property.Value, key, result);
                }
                return;
            }

            if (token is JValue value)
            {
                result[prefix] = value.ToString();
            }
        }
    }
}
