using Own.Permanent;
namespace Own.MarkupHandlers
{
    internal static partial class Marker
    {
        internal static string GenerateNewPrivateDialogEntry
                        (in int secondId, in string secondNick)
        {
            return string.Concat(Constants.brMarker,
                                Constants.pStart,
                                secondId,
                                Constants.pMiddle,
                                secondNick,
                                Constants.brMarker,
                                Constants.brMarker);
        }
        internal static string GenerateNewPrivateDialogEntryWithEnd
            (in int secondId, in string secondNick)
        {
            return string.Concat(Constants.brMarker,
                                Constants.pStart,
                                secondId,
                                Constants.pMiddle,
                                secondNick,
                                Constants.pEnd,
                                Constants.brMarker,
                                Constants.brMarker);
        }
        internal static string GenerateNewPrivateDialogPage(in int companionId,
            in string companionNick, in int ownerId, in string ownerNick, in string message)
        {
            return string.Concat(Constants.indic,
                companionId,
                "</div><div class='l'><h2 onClick='n(&quot;/d/1&quot;);'>Переписка с ",
                companionNick,
                "</h2>",
                Constants.articleStart,
                "<span onClick='n(&quot;/k/",
                ownerId,
                "&quot;);'>",
                ownerNick,
                "</span><br /><p>",
                message,
                "</p>",
                Constants.articleEnd,
                "<br />",
                Constants.fullSpanMarker,
                Constants.endSpanMarker,
                "<div id='a'><a onClick='f();return false'>Ответить ",
                companionNick,
                "</a></div></div><div class='s'>4</div>");
        }

        internal static string GenerateNewPrivateDialogArticle(int accountId, string accountNick, string message)
        {
            return string.Concat(Constants.articleStart,
                    "<span onClick='n(&quot;/k/",
                    accountId,
                    "&quot;);'>",
                    accountNick,
                    "</span><br /><p>",
                    message,
                    "</p>",
                    Constants.articleEnd,
                    "<br />");
        }
    }
}