using Unicareer.Models;

namespace Unicareer.Services
{
    public interface IExternalArticleService
    {
        Task<List<ExternalArticle>> FetchArticlesAsync(string? keyword = null, int pageSize = 10);
        Task<ExternalArticle?> FetchArticleByIdAsync(string articleId);
        Task<string?> FetchFullContentAsync(string articleId, string sourceName, string? sourceUrl);
    }

    public class ExternalArticle
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Content { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public string? Author { get; set; }
        public DateTime? PublishedAt { get; set; }
        public string? SourceUrl { get; set; }
        public string? SourceName { get; set; }
        public List<string>? Tags { get; set; }
    }
}

