using System.Collections.Generic;
using System.Linq;
using Own.Types;
namespace Own.MarkupHandlers
{
    internal static partial class Marker
    {
        internal static string GenerateForumLinks(in IEnumerable<IdName> idNames)
        {
            var result = "<nav class='o'>";

            if (idNames.Any())
            {
                string elId;

                foreach (var idName in idNames)
                {
                    elId = idName.Id.ToString();
                    result = string.Concat(result,
                        "<br /><span onClick='m(&quot;/e/",
                        elId,
                        "&quot;,&quot;e",
                        elId,
                        "&quot;);'>",
                        idName.Name,
                        "</span><div id='e",
                        elId,
                        "'><br /></div>");
                }
            }

            return string.Concat(result, "</nav>");
        }
    }
}