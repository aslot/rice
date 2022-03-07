using Own.Permanent;
using Own.Storage;
namespace Own.InRace
{
    internal static partial class Unstable
    {
        internal static string GetEndPointPageNullable(in int? id)
        {
            if (id == null)
                return Constants.SE;
            else
            {
                if (id > Constants.Zero && id <= Constants.EndPointPagesCount)
                    return Fast.GetEndPointPageLocked((int)id - Constants.One);
                else
                    return Constants.SE;
            }
        }
    }
}
