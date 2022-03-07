using Own.Permanent;
using Own.Types;
using Own.Storage;
namespace Own.InRace
{
    internal static partial class Unstable
    {
        internal static string GetDialogPageNullable(in int? page, in Pair pair)
        {
            if (page == null)
                return Constants.SE;
            else
            {
                int? accountId = Own.InRace.Unstable.GetAccountIdNullable(pair);

                if (accountId.HasValue)
                {
                    int index = accountId.Value - Constants.One;

                    if (page > Constants.Zero
                        && page
                        <= Fast.GetDialogPagesPageDepthLocked(index))
                    {
                        return Own.Storage.Fast
                          .GetDialogPagesPageLocked(index
                             , (int)page - Constants.One);
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
