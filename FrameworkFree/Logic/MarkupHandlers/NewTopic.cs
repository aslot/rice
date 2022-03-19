namespace Own.MarkupHandlers
{
    internal static partial class Marker
    {
        internal static string GetTemp(in int threadId, in string threadName)
        {
            return string.Concat("<br /><p onclick='n(&quot;/f/",
                     threadId,
                     "?p=1&quot;);'>",
                     threadName,
                     "</p><br /><br />");
        }
        internal static string GetLastPage(in int endpointId)
        {
            return string.Concat("<div id='t'><span><a onclick='q();return false;'>Новая тема</a>",
                                        "</span></div><nav class='n'></nav><div class='s'>",
                                        endpointId,
                                        "</div></div>");
        }
        internal static string GetNewPage
            (int threadId, int endpointId, string threadName,
             int accountId, string nick, string message)
        {
            return string.Concat("<div class='s'>",
                    threadId,
                    "</div><div class='l'><h2 onclick='n(&quot;/s/",
                    endpointId,
                    "?p=1&quot;);'>",
                    threadName,
                    "</h2><article><span onclick='n(&quot;/k/",
                    accountId,
                    "&quot;);'>",
                    nick,
                    "</span><br /><p>",
                    message,
                    "</p></article><br /><div id='a'><a onclick='u();return false'>Ответить</a>",
                    "</div></div><div class='s'>4</div>");
        }
    }
}