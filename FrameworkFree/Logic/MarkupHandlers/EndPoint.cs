using System.Collections.Generic;
using Own.Permanent;
using Own.Types;
namespace Own.MarkupHandlers
{
    internal static partial class Marker
    {
        internal static string GenerateEndPointLinks(in IEnumerable<IdName> idNames)
        {
            var result = Constants.SE;

            foreach (var idName in idNames)
                result = string.Concat(result,
                            "<p onClick='n(&quot;/s/",
                            idName.Id,
                            "?p=1&quot;);'>",
                            idName.Name,
                            "</p><br />");

            return result;
        }
    }
}