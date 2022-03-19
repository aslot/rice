using Own.Permanent;
using Own.Storage;
namespace Own.InRace
{
    internal static partial class Unstable
    {
        internal static string GetSectionPageNullable(in int? id, in int? page)
        {
            if (id == null || page == null)
                return Constants.SE;
            else
            {
                if (id > Constants.Zero && id <= Fast.GetSectionPagesLengthLocked())
                {
                    int index = (int)id - Constants.One;

                    if (page > Constants.Zero
                        && page <= Fast.GetSectionPagesPageDepthLocked(index))
                        return Fast.GetSectionPagesPageLocked
                            (index, (int)page - Constants.One);
                    else return Constants.SE;
                }
                else
                    return Constants.SE;
            }
        }
    }
}
