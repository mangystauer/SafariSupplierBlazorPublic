using DataAccessLibrary.Models.Shatem.Models;

namespace DataAccessLibrary.DataService.Shatem
{
    public interface IShatemDataService
    {
        Task<ShatemFoundArticle> ArticleInfoAsync(string articleId, bool includeAnalogs = false);
        Task<ShatemFullArticle> FullArticleInfoAsync(int articleId);
        Task<List<ShatemFoundArticle>> SearchArticlesAsync(string lineToSearch, string tradeMarkName = null);
        Task<List<ShatemArticlePrice>> SearchAvailableQuantityAsync(string articleId, bool includeAnalogs = false);
        Task<List<string>> SearchContentsAsync(string contentId, int heightSize = 400, int widthSize = 400);
    }
}