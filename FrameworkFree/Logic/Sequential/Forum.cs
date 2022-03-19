using Own.MarkupHandlers;
using Own.Permanent;
using Own.Storage;
namespace Own.Sequential
{
    internal static partial class Unstable
    {
        internal static void LoadMainPageVoid()
        {
            Fast.SetMainPageLocked(Constants.MainPage);
            Fast.SetMainContentLocked(Marker.GenerateForumLinks
                (Slow.GetForumIdNamesNullable()));
            Fast.AddToMainPageLocked(Fast.GetMainContentLocked());
            Fast.AddToMainPageLocked(Constants.MainPageEnd);
        }
    }
}
