using System.Collections.Generic;
using Data;
namespace MarkupHandlers // move to string.Concat() if more than one constant is presented
{
    internal sealed class EndPointMarkupHandler
    {
        internal string GenerateEndPointLinks(in IEnumerable<IdName> idNames)
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