using MarkupHandlers;
namespace Data
{
    internal sealed class ForumLogic : IForumLogic
    {
        private readonly IStorage Storage;
        private readonly ForumMarkupHandler ForumMarkupHandler;
        public ForumLogic(IStorage storage, ForumMarkupHandler forumMarkupHandler)
        {
            Storage = storage;
            ForumMarkupHandler = forumMarkupHandler;
        }
        public void LoadMainPage()
        {
            Storage.Fast.SetMainPageLocked(Constants.MainPage);
            Storage.Fast.SetMainContentLocked(ForumMarkupHandler.GenerateForumLinks
                (Storage.Slow.GetForumIdNames()));
            Storage.Fast.AddToMainPageLocked(Storage.Fast.GetMainContentLocked());
            Storage.Fast.AddToMainPageLocked(Constants.MainPageEnd);
        }
    }
}
