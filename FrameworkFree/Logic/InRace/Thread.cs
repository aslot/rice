using Own.Permanent;
using Own.Storage;
namespace Own.InRace
{
    internal static partial class Unstable
    {
        internal static string GetThreadPageNullable(in int? id, in int? page)
        {
            if (id == null || page == null)
                return Constants.SE;
            else
            {
                int index = (int)id;

                if (index > Constants.Zero
                    && Fast.ThreadPagesContainsThreadIdLocked(index))
                {
                    if (page > Constants.Zero
                        && page <= Fast.GetThreadPagesPageDepthLocked(index))
                        return Own.Storage.Fast
                            .GetThreadPagesPageLocked(index,
                                (int)page - Constants.One);
                    else
                        return Constants.SE;
                }
                else
                    return Constants.SE;
            }
        }
    }
}
