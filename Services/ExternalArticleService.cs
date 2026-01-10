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

        /// <summary>
        /// Lấy nội dung đầy đủ của bài viết từ API gốc
        /// </summary>
        public async Task<string?> FetchFullContentAsync(string articleId, string sourceName, string? sourceUrl)
        {
            try
            {
                // Dev.to API: Lấy full content từ endpoint chi tiết
                if (articleId.StartsWith("devto_"))
                {
                    var devToId = articleId.Replace("devto_", "");
                    if (int.TryParse(devToId, out int id))
                    {
                        return await FetchDevToFullContentAsync(id);
                    }
                }
                
                // NewsAPI: Content thường đã có trong response ban đầu
                // NewsAPI có thể trả về content đầy đủ hoặc bị cắt ngắn tùy vào nguồn
                // Nếu content từ response ban đầu quá ngắn, có thể cần scrape từ URL
                // Tuy nhiên, việc scrape có thể vi phạm ToS của một số trang web
                if (articleId.StartsWith("newsapi_") && !string.IsNullOrEmpty(sourceUrl))
                {
                    _logger.LogInformation("NewsAPI article - Content đã được lấy từ response ban đầu. URL: {Url}", sourceUrl);
                    // Note: NewsAPI thường trả về content đầy đủ trong response
                    // Nếu content bị cắt ngắn, có thể do nguồn gốc không cung cấp full content
                }
                
                // MediaStack: Chỉ có Description, không có Content riêng
                // Description từ MediaStack thường đã là tóm tắt đầy đủ
                if (articleId.StartsWith("mediastack_") && !string.IsNullOrEmpty(sourceUrl))
                {
                    _logger.LogInformation("MediaStack article - Chỉ có Description. URL: {Url}", sourceUrl);
                    // Note: MediaStack không cung cấp full content, chỉ có description
                }
                
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching full content for article: {ArticleId}", articleId);
                return null;
            }
        }

        /// <summary>
        /// Lấy full content từ Dev.to API bằng cách gọi endpoint chi tiết bài viết
        /// </summary>
        private async Task<string?> FetchDevToFullContentAsync(int articleId)
        {
            try
            {
                var url = $"https://dev.to/api/articles/{articleId}";
                _logger.LogInformation("=== FetchDevToFullContentAsync START ===");
                _logger.LogInformation("ArticleId: {ArticleId}, URL: {Url}", articleId, url);
                
                var response = await _httpClient.GetAsync(url);
                
                _logger.LogInformation("Response Status: {StatusCode}", response.StatusCode);
                
                if (!response.IsSuccessStatusCode)
                {
                    var errorBody = await response.Content.ReadAsStringAsync();
                    _logger.LogWarning("Dev.to API returned status code: {StatusCode} for article {ArticleId}, Error: {Error}", 
                        response.StatusCode, articleId, errorBody);
                    return null;
                }

                var json = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("Response JSON length: {Length}", json?.Length ?? 0);
                _logger.LogInformation("Response JSON preview (first 500 chars): {Preview}", 
                    json?.Length > 500 ? json.Substring(0, 500) + "..." : json);
                
                // Parse JSON trực tiếp để tìm field chứa content (vì Dev.to có thể dùng tên field khác)
                using (JsonDocument doc = JsonDocument.Parse(json))
                {
                    var root = doc.RootElement;
                    
                    // Log tất cả các keys để debug
                    var allKeys = root.EnumerateObject().Select(p => p.Name).ToList();
                    _logger.LogInformation("All fields in Dev.to response: {Fields}", string.Join(", ", allKeys));
                    
                    // Tìm field chứa content - Dev.to có thể dùng tên khác
                    string? fullContent = null;
                    string? contentFieldName = null;
                    
                    // Thử các tên field có thể có (theo thứ tự ưu tiên)
                    var possibleFields = new[] { "body_html", "body_markdown", "body", "html", "markdown", "content", "body_html_content" };
                    foreach (var fieldName in possibleFields)
                    {
                        if (root.TryGetProperty(fieldName, out var fieldValue) && fieldValue.ValueKind == JsonValueKind.String)
                        {
                            var value = fieldValue.GetString();
                            if (!string.IsNullOrWhiteSpace(value) && value.Length > 100) // Đảm bảo là content thật, không phải description
                            {
                                fullContent = value;
                                contentFieldName = fieldName;
                                _logger.LogInformation("✅ Found content in field '{Field}', length: {Length}", fieldName, value.Length);
                                break;
                            }
                        }
                    }
                    
                    // Nếu không tìm thấy, tìm field nào có chứa "body" hoặc "html" hoặc "markdown" trong tên
                    if (string.IsNullOrWhiteSpace(fullContent))
                    {
                        foreach (var prop in root.EnumerateObject())
                        {
                            var propName = prop.Name.ToLower();
                            if ((propName.Contains("body") || propName.Contains("html") || propName.Contains("markdown") || propName.Contains("content")) 
                                && prop.Value.ValueKind == JsonValueKind.String)
                            {
                                var value = prop.Value.GetString();
                                if (!string.IsNullOrWhiteSpace(value) && value.Length > 100) // Đảm bảo là content thật
                                {
                                    fullContent = value;
                                    contentFieldName = prop.Name;
                                    _logger.LogInformation("✅ Found content in field '{Field}', length: {Length}", prop.Name, value.Length);
                                    break;
                                }
                            }
                        }
                    }
                    
                    if (string.IsNullOrWhiteSpace(fullContent))
                    {
                        _logger.LogWarning("❌ Dev.to article {ArticleId} has no body content found in any field. Available fields: {Fields}", 
                            articleId, string.Join(", ", allKeys));
                        // Log một phần JSON để debug
                        _logger.LogWarning("JSON sample (first 2000 chars): {Json}", json.Length > 2000 ? json.Substring(0, 2000) + "..." : json);
                        return null;
                    }
                    
                    _logger.LogInformation("✅ Successfully extracted content from field '{Field}', length: {Length}", 
                        contentFieldName, fullContent.Length);
                    _logger.LogInformation("Content preview (first 300 chars): {Preview}",
                        fullContent.Length > 300 ? fullContent.Substring(0, 300) + "..." : fullContent);
                    
                    _logger.LogInformation("=== FetchDevToFullContentAsync END ===");
                    return fullContent;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error fetching Dev.to full content for article {ArticleId}: {Message}, StackTrace: {StackTrace}", 
                    articleId, ex.Message, ex.StackTrace);
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

                return result.Articles.Select(a => 
                {
                    // Ưu tiên Content, nếu Content quá ngắn hoặc null thì dùng Description
                    // Nếu cả hai đều có, ưu tiên cái dài hơn
                    string? finalContent = null;
                    if (!string.IsNullOrWhiteSpace(a.Content) && a.Content.Length > 100)
                    {
                        // Content từ NewsAPI có thể bị cắt ngắn (thường kết thúc bằng "...")
                        // Nếu content dài hơn 100 ký tự, dùng nó
                        finalContent = a.Content;
                    }
                    else if (!string.IsNullOrWhiteSpace(a.Description))
                    {
                        finalContent = a.Description;
                    }
                    
                    // Nếu có cả Content và Description, ưu tiên cái dài hơn
                    if (!string.IsNullOrWhiteSpace(a.Content) && !string.IsNullOrWhiteSpace(a.Description))
                    {
                        finalContent = a.Content.Length > a.Description.Length ? a.Content : a.Description;
                    }
                    
                    return new ExternalArticle
                    {
                        Id = $"newsapi_{a.Url?.GetHashCode() ?? Guid.NewGuid().GetHashCode()}",
                        Title = a.Title ?? string.Empty,
                        Description = a.Description,
                        Content = finalContent ?? a.Content ?? a.Description ?? string.Empty,
                        ImageUrl = a.UrlToImage,
                        Author = a.Author ?? a.Source?.Name,
                        PublishedAt = a.PublishedAt,
                        SourceUrl = a.Url,
                        SourceName = a.Source?.Name ?? "NewsAPI",
                        Tags = ExtractTags(a.Title, a.Description)
                    };
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
                    // MediaStack chỉ có Description, không có Content riêng
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
            public string? BodyMarkdown { get; set; }
            public string? BodyHtml { get; set; }
            // Dev.to API có thể trả về các field khác
            public string? Body { get; set; }
            public string? BodyHtmlContent { get; set; }
            public string? Html { get; set; }
            public string? Markdown { get; set; }
            // Sử dụng JsonExtensionData để capture tất cả fields không map được
            [System.Text.Json.Serialization.JsonExtensionData]
            public Dictionary<string, object>? AdditionalData { get; set; }
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

