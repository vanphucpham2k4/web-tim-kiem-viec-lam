using System.Text.Json;
using Unicareer.Models;

namespace Unicareer.Services
{
    public class ExternalArticleService : IExternalArticleService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ExternalArticleService> _logger;
        private readonly IConfiguration _configuration;

        public ExternalArticleService(HttpClient httpClient, ILogger<ExternalArticleService> logger, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _logger = logger;
            _configuration = configuration;
            _httpClient.Timeout = TimeSpan.FromSeconds(30);
            
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "Unicareer/1.0 (Blog Aggregator)");
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        public async Task<List<ExternalArticle>> FetchArticlesAsync(string? keyword = null, int pageSize = 10)
        {
            var articles = new List<ExternalArticle>();
            
            try
            {
                var searchQuery = keyword ?? "career";
                
                var devToEnabled = _configuration.GetValue<bool>("ExternalAPIs:DevTo:Enabled", true);
                if (devToEnabled)
                {
                    var devToArticles = await FetchFromDevToAsync(searchQuery, pageSize);
                    articles.AddRange(devToArticles);
                }
                
                var newsApiKey = _configuration["ExternalAPIs:NewsAPI:ApiKey"];
                var newsApiEnabled = _configuration.GetValue<bool>("ExternalAPIs:NewsAPI:Enabled", false);
                if (newsApiEnabled && !string.IsNullOrEmpty(newsApiKey) && newsApiKey != "YOUR_NEWSAPI_KEY_HERE")
                {
                    var newsArticles = await FetchFromNewsAPIAsync(searchQuery, pageSize, newsApiKey);
                    articles.AddRange(newsArticles);
                }
                
                var mediaStackKey = _configuration["ExternalAPIs:MediaStack:ApiKey"];
                var mediaStackEnabled = _configuration.GetValue<bool>("ExternalAPIs:MediaStack:Enabled", false);
                if (mediaStackEnabled && !string.IsNullOrEmpty(mediaStackKey) && mediaStackKey != "YOUR_MEDIASTACK_KEY_HERE")
                {
                    var mediaStackArticles = await FetchFromMediaStackAsync(searchQuery, pageSize, mediaStackKey);
                    articles.AddRange(mediaStackArticles);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching articles from external APIs");
            }

            return articles.DistinctBy(a => a.Id).Take(pageSize * 2).ToList();
        }

        public async Task<ExternalArticle?> FetchArticleByIdAsync(string articleId)
        {
            try
            {
                var articles = await FetchArticlesAsync();
                return articles.FirstOrDefault(a => a.Id == articleId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching article by ID: {ArticleId}", articleId);
                return null;
            }
        }

        private async Task<List<ExternalArticle>> FetchFromNewsAPIAsync(string query, int pageSize, string apiKey)
        {
            try
            {
                var url = $"https://newsapi.org/v2/everything?q={Uri.EscapeDataString(query)}&language=en&sortBy=publishedAt&pageSize={pageSize}&apiKey={apiKey}";
                var response = await _httpClient.GetAsync(url);
                
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("NewsAPI returned status code: {StatusCode}", response.StatusCode);
                    return new List<ExternalArticle>();
                }

                var json = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<NewsApiResponse>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (result?.Articles == null)
                    return new List<ExternalArticle>();

                return result.Articles.Select(a => new ExternalArticle
                {
                    Id = $"newsapi_{a.Url?.GetHashCode() ?? Guid.NewGuid().GetHashCode()}",
                    Title = a.Title ?? string.Empty,
                    Description = a.Description,
                    Content = a.Content ?? a.Description ?? string.Empty,
                    ImageUrl = a.UrlToImage,
                    Author = a.Author ?? a.Source?.Name,
                    PublishedAt = a.PublishedAt,
                    SourceUrl = a.Url,
                    SourceName = a.Source?.Name ?? "NewsAPI",
                    Tags = ExtractTags(a.Title, a.Description)
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching from NewsAPI");
                return new List<ExternalArticle>();
            }
        }

        private async Task<List<ExternalArticle>> FetchFromDevToAsync(string query, int pageSize)
        {
            try
            {
                _logger.LogInformation("=== STARTING Dev.to fetch ===");
                _logger.LogInformation("Query: {Query}, PageSize: {PageSize}", query, pageSize);
                
                var tag = string.IsNullOrEmpty(query) ? "career" : query.ToLower().Replace(" ", "");
                var url = $"https://dev.to/api/articles?tag={tag}&per_page={Math.Min(pageSize, 30)}";
                
                _logger.LogInformation("Dev.to API URL: {Url}", url);
                _logger.LogInformation("HttpClient Timeout: {Timeout}s", _httpClient.Timeout.TotalSeconds);
                
                var response = await _httpClient.GetAsync(url);
                
                _logger.LogInformation("Dev.to API Response - Status: {StatusCode}, Headers: {Headers}", 
                    response.StatusCode, 
                    string.Join(", ", response.Headers.Select(h => $"{h.Key}={string.Join(",", h.Value)}")));
                
                if (!response.IsSuccessStatusCode)
                {
                    var errorBody = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Dev.to API ERROR - Status: {StatusCode}, Body: {ErrorBody}", response.StatusCode, errorBody);
                    return new List<ExternalArticle>();
                }

                var json = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("Dev.to API response length: {Length} characters", json?.Length ?? 0);
                
                if (string.IsNullOrEmpty(json))
                {
                    _logger.LogError("Dev.to returned EMPTY response!");
                    return new List<ExternalArticle>();
                }
                
                _logger.LogInformation("First 200 chars of response: {Sample}", json.Length > 200 ? json.Substring(0, 200) : json);
                
                var result = JsonSerializer.Deserialize<List<DevToArticle>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (result == null || result.Count == 0)
                {
                    _logger.LogWarning("Dev.to API returned no articles for tag: {Tag}, trying fallback...", tag);
                    
                    var fallbackUrl = $"https://dev.to/api/articles?per_page={Math.Min(pageSize, 30)}";
                    _logger.LogInformation("Fallback URL: {FallbackUrl}", fallbackUrl);
                    
                    var fallbackResponse = await _httpClient.GetAsync(fallbackUrl);
                    
                    if (fallbackResponse.IsSuccessStatusCode)
                    {
                        var fallbackJson = await fallbackResponse.Content.ReadAsStringAsync();
                        _logger.LogInformation("Fallback response length: {Length}", fallbackJson?.Length ?? 0);
                        
                        result = JsonSerializer.Deserialize<List<DevToArticle>>(fallbackJson, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });
                        _logger.LogInformation("Fallback returned {Count} articles", result?.Count ?? 0);
                    }
                    else
                    {
                        _logger.LogError("Fallback also failed with status: {Status}", fallbackResponse.StatusCode);
                    }
                }

                if (result == null)
                {
                    _logger.LogError("Result is NULL after all attempts!");
                    return new List<ExternalArticle>();
                }

                var articles = result.Select(a => new ExternalArticle
                {
                    Id = $"devto_{a.Id}",
                    Title = a.Title ?? string.Empty,
                    Description = a.Description,
                    Content = a.Description ?? string.Empty,
                    ImageUrl = a.CoverImage ?? a.SocialImage,
                    Author = a.User?.Name ?? "Dev.to",
                    PublishedAt = a.PublishedAt,
                    SourceUrl = a.Url,
                    SourceName = "Dev.to",
                    Tags = a.TagList?.Select(t => t.Trim()).ToList() ?? new List<string>()
                }).ToList();
                
                _logger.LogInformation("Successfully fetched {Count} articles from Dev.to", articles.Count);
                return articles;
            }
            catch (HttpRequestException httpEx)
            {
                _logger.LogError(httpEx, "HTTP REQUEST ERROR when calling Dev.to API - Message: {Message}, StatusCode: {StatusCode}", 
                    httpEx.Message, 
                    httpEx.StatusCode?.ToString() ?? "N/A");
                _logger.LogError("InnerException: {Inner}", httpEx.InnerException?.Message ?? "None");
                return new List<ExternalArticle>();
            }
            catch (TaskCanceledException timeoutEx)
            {
                _logger.LogError(timeoutEx, "TIMEOUT ERROR when calling Dev.to API - Message: {Message}", timeoutEx.Message);
                _logger.LogError("This might be a network issue or Dev.to is slow. Timeout was: {Timeout}s", _httpClient.Timeout.TotalSeconds);
                return new List<ExternalArticle>();
            }
            catch (JsonException jsonEx)
            {
                _logger.LogError(jsonEx, "JSON PARSING ERROR from Dev.to - Message: {Message}", jsonEx.Message);
                return new List<ExternalArticle>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UNEXPECTED ERROR fetching from Dev.to API - Type: {Type}, Message: {Message}", 
                    ex.GetType().Name, 
                    ex.Message);
                _logger.LogError("StackTrace: {StackTrace}", ex.StackTrace);
                return new List<ExternalArticle>();
            }
        }

        private async Task<List<ExternalArticle>> FetchFromMediaStackAsync(string query, int pageSize, string apiKey)
        {
            try
            {
                var url = $"http://api.mediastack.com/v1/news?access_key={apiKey}&keywords={Uri.EscapeDataString(query)}&languages=en&limit={Math.Min(pageSize, 25)}";
                var response = await _httpClient.GetAsync(url);
                
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("MediaStack API returned status code: {StatusCode}", response.StatusCode);
                    return new List<ExternalArticle>();
                }

                var json = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<MediaStackResponse>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (result?.Data == null)
                    return new List<ExternalArticle>();

                return result.Data.Select(a => new ExternalArticle
                {
                    Id = $"mediastack_{a.Url?.GetHashCode() ?? Guid.NewGuid().GetHashCode()}",
                    Title = a.Title ?? string.Empty,
                    Description = a.Description,
                    Content = a.Description ?? string.Empty,
                    ImageUrl = a.Image,
                    Author = a.Author ?? a.Source,
                    PublishedAt = a.PublishedAt,
                    SourceUrl = a.Url,
                    SourceName = a.Source ?? "MediaStack",
                    Tags = ExtractTags(a.Title, a.Description)
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching from MediaStack API");
                return new List<ExternalArticle>();
            }
        }

        private List<string> ExtractTags(string? title, string? content)
        {
            var tags = new List<string>();
            if (string.IsNullOrEmpty(title) && string.IsNullOrEmpty(content))
                return tags;

            var text = $"{title} {content}".ToLower();
            var jobKeywords = new[] { "job", "career", "skill", "hiring", "recruitment", "employment", "work", "professional", "developer", "engineer", "manager", "analyst", "designer", "marketing", "sales", "finance", "hr", "it", "tech", "software", "programming", "coding", "interview", "resume", "cv", "salary", "remote", "freelance" };
            
            foreach (var keyword in jobKeywords)
            {
                if (text.Contains(keyword))
                {
                    tags.Add(keyword);
                }
            }

            return tags.Take(5).ToList();
        }

        private class NewsApiResponse
        {
            public List<NewsApiArticle>? Articles { get; set; }
        }

        private class NewsApiArticle
        {
            public string? Title { get; set; }
            public string? Description { get; set; }
            public string? Content { get; set; }
            public string? Url { get; set; }
            public string? UrlToImage { get; set; }
            public string? Author { get; set; }
            public DateTime? PublishedAt { get; set; }
            public NewsApiSource? Source { get; set; }
        }

        private class NewsApiSource
        {
            public string? Name { get; set; }
        }

        private class DevToArticle
        {
            public int Id { get; set; }
            public string? Title { get; set; }
            public string? Description { get; set; }
            public string? Url { get; set; }
            public string? CoverImage { get; set; }
            public string? SocialImage { get; set; }
            public DateTime? PublishedAt { get; set; }
            public List<string>? TagList { get; set; }
            public DevToUser? User { get; set; }
        }

        private class DevToUser
        {
            public string? Name { get; set; }
        }

        private class MediaStackResponse
        {
            public List<MediaStackArticle>? Data { get; set; }
        }

        private class MediaStackArticle
        {
            public string? Title { get; set; }
            public string? Description { get; set; }
            public string? Url { get; set; }
            public string? Image { get; set; }
            public string? Author { get; set; }
            public DateTime? PublishedAt { get; set; }
            public string? Source { get; set; }
        }
    }
}

