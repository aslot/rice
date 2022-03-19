using Own.Permanent;
using Own.Types;
using Own.Storage;
namespace Own.InRace
{
    internal static partial class Unstable
    {
        internal static string GetPersonalPageNullable
                            (in int? id, in int? page, in Pair pair)
        {
            if (page == null || id == null)
                return Constants.SE;
            else
            {
                int? index = Own.InRace.Unstable.GetAccountIdNullable(pair);

                if (index.HasValue)
                {
                    int id_ = (int)id;
                    int limit = Fast.GetPersonalPagesPageDepthLocked(index.Value, id_);

                    if (page > Constants.Zero
                        && page <= limit)
                    {
                        return Fast.GetMessageLocked(index.Value, id_, (int)page - Constants.One);
                    }
                    else
                        return Constants.SE;
                }
                else
                    return Constants.SE;
            }
        }
    }
}
