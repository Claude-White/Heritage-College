using HFA.Models.ViewModels;

namespace HFA.Models
{
    public interface IDbAccess
    {

        #region AboutUs

        Task<AboutUsViewModel> GetAboutUsAsync();

        Task<Mission> GetMissionAsync();

        Task UpdateMissionAsync(Mission mission);

        Task<List<Executive>> GetExecutivesAsync();

        Task<List<ExecutiveViewModel>> GetExecutivesViewModelAsync();

        Task<int> CreateExecutiveAsync(Executive executive);

        Task<Executive> GetExecutiveAsync(int id);

        Task UpdateExecutiveAsync(Executive executive);  

        Task DeleteExecutiveAsync(int id);

        Task<List<ElectedGroupViewModel>> GetElectedMembersAsync();

        Task<ElectedGroup> GetElectedGroupAsync(int id);

        Task<int> CreateElectedGroupAsync(ElectedGroup electedGroup);

        Task UpdateElectedGroupAsync(ElectedGroup electedGroup);

        Task DeleteElectedGroupAsync(int id);

        #endregion

        #region FAQ

        Task<List<FrequentlyAskedQuestion>> GetFAQAsync();

        Task<int> CreateFAQAsync(FrequentlyAskedQuestion frequentlyAskedQuestion);

        Task UpdateFAQAsync(FrequentlyAskedQuestion frequentlyAskedQuestion);

        Task DeleteFAQAsync(int id);

        #endregion

        #region Articles

        Task<Article?> GetArticleAsync(int articleId);
        Task<List<Article>> GetArticlesAsync();
        Task<List<Article>> GetPublishedArticlesAsync();
        Task<int> CreateArticleAsync(Article article);
        Task UpdateArticleAsync(Article article);
        Task DeleteArticleAsync(int id);

        Task<List<string?>> GetArticleCategoriesAsync();

        #endregion

        #region Home

        Task<List<ReleaseNotes>> GetReleaseNotesAsync();

        Task<ReleaseNotes> GetReleaseNoteAsync(int releaseNoteId);

        Task<List<ReleaseNotesViewModel>> GetReleaseNotesViewModelsAsync(List<ReleaseNotes> notes);

        Task<bool> CreateReleaseNoteAsync(ReleaseNotes note);

        Task<bool> UpdateReleaseNoteAsync(ReleaseNotes note);

        Task<bool> DeleteReleaseNoteAsync(int releaseNoteId);

        #endregion

        #region Images

        Task<Image?> CreateImageAsync(SixLabors.ImageSharp.Image imageSharpImage);

        Task<Image?> GetImageAsync(int imageId);

        #endregion
    }
}
