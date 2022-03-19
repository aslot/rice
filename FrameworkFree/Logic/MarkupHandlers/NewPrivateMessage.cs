using Own.Permanent;
namespace Own.MarkupHandlers
{
    internal static partial class Marker
    {
        internal static string GenerateNewPrivateMessagePageIfEmpty(in int companionId,
            in string companionNick, in byte order, in int ownerId, in string ownerNick, in string text)
        {
            var result = string.Concat(Constants.indic,
                            companionId,
                            "</div><div class='l'><h2 onClick='n(&quot;/d/1&quot;);'>Переписка с ",
                            companionNick,
                            "</h2>");

            if (order == Constants.One)
                result = string.Concat(result,
                            Constants.articleStart,
                            "<span onClick='n(&quot;/k/",
                            ownerId,
                            "&quot;);'>",
                            ownerNick,
                            "</span><br />");
            else
                result = string.Concat(result,
                            Constants.articleStart,
                            "<span onClick='n(&quot;/k/",
                            companionId,
                            "&quot;);'>",
                            companionNick,
                            "</span><br />");

            return string.Concat(result,
                        "<p>",
                        text,
                        "</p>",
                        Constants.articleEnd,
                        Constants.brMarker,
                        Constants.fullSpanMarker,
                        Constants.spanEnd,
                        "<div id='a'><a onClick='f();return false'>Ответить ",
                        companionNick,
                        "</a></div></div><div class='s'>4</div>");
        }
        internal static string GenerateNewPrivateMessagePage(in byte order, in int ownerId,
            in string ownerNick, in int companionId, in string companionNick, in string text)
        {
            var result = Constants.SE;

            if (order == Constants.One)
                result = string.Concat(Constants.articleStart,
                            "<span onClick='n(&quot;/k/",
                            ownerId,
                            "&quot;);'>",
                            ownerNick,
                            "</span><br />");
            else
                result = string.Concat(Constants.articleStart,
                            "<span onClick='n(&quot;/k/",
                            companionId,
                            "&quot;);'>",
                            companionNick,
                            "</span><br />");

            return string.Concat(result,
                    "<p>",
                    text,
                    "</p>",
                    Constants.articleEnd,
                    "<br />");
        }
    }
}