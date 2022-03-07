using System.Collections.Generic;
using System.Linq;
using Own.MarkupHandlers;
using Own.Permanent;
using Own.Types;
using Own.Storage;
namespace Own.Sequential
{
    internal static partial class Unstable
    {
        internal static void LoadEndPointPagesVoid()
        {
            Fast.InitializeEndPointPagesLocked(Constants.EndPointPagesCount);

            for (int i = Constants.Zero;
                    i < Constants.EndPointPagesCount; i++)
                ProcessEndPointReaderVoid(Slow.GetEndpointIdNamesNullable(i + Constants.One), i);
        }

        private static void ProcessEndPointReaderVoid
            (in IEnumerable<IdName> idNames, in int number)
        {
            if (idNames.Any())
                Fast.SetEndPointPageLocked(number,
                    Marker.GenerateEndPointLinks(idNames));
        }
    }
}