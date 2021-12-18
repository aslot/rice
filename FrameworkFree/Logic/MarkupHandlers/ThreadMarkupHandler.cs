using Data;
namespace MarkupHandlers
{
    internal sealed class ThreadMarkupHandler
    {
        internal string GetThreadDiv(in int i)
        {
            return string.Concat("</div><div class='s'>",
                        Constants.five - i,
                        Constants.indicEnd);
        }
        internal string GetIndicAndA()
        {
            return string.Concat(Constants.indicEnd, Constants.a);
        }
        internal string GetArticle(in int accountId, in string nick, in string msgText)
        {
            return string.Concat("<article><span onClick='n(&quot;/k/",
                        accountId,
                        "&quot;);'>",
                        nick,
                        "</span><br /><p>",
                        msgText,
                        "</p></article><br />"); //br2
        }
        internal string GetSectionHeader(in int number, in int sectionNum, in string threadName)
        {
            return string.Concat(Constants.indic,
                        number,
                        "</div><div class='l'><h2 onClick='n(&quot;/s/",
                        sectionNum,
                        "?p=1&quot;);'>",
                        threadName,
                        "</h2>");
        }
        internal string SetNavigation
                    (in int pageNumber, in int pagesCount, in int number)
        {
            return string.Concat(GetArrows(pageNumber, pagesCount, number),
                        "<div id='a'><a onClick='u();return false'>Ответить</a></div>"); //br1
        }
        internal string GetArrows
                  (in int pageNumber, in int pagesCount, in int number)
        {
            string result = Constants.SE;
            string thread = (number + Constants.One).ToString();
            const string a = "<span id='w'><a onClick='n(&quot;/f/";
            const string b = "?p=1&quot;);return false' title='Первая страница'>«</a>&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='n(&quot;/f/";

            if ((pageNumber >= 2)
                    && (pageNumber + 3 <= pagesCount))
            {
                result = string.Concat(a,
                            thread,
                            b,
                            thread,
                            "?p=",
                            pageNumber,
                            "&quot;);return false' title='Предыдущая страница'>◄</a>",
                            "&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='n(&quot;/f/",
                            thread,
                            "?p=",
                            pageNumber + 2,
                            "&quot;);return false' title='Следующая страница'>►</a>&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='n(&quot;/f/",
                            thread,
                            "?p=",
                            pagesCount,
                            "&quot;);return false' title='Последняя страница'>»</a></span>");
            }
            else if ((pageNumber >= 2)
                    && (pageNumber + 2 == pagesCount))
            {
                result = string.Concat(a,
                            thread,
                            b,
                            thread,
                            "?p=",
                            pageNumber,
                            "&quot;);return false' title='Предыдущая страница'>◄</a>",
                            "&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='n(&quot;/f/",
                            thread,
                            "?p=",
                            pageNumber + 2,
                            "&quot;);return false' title='Следующая страница'>►</a>&nbsp;&nbsp;&nbsp;&nbsp;</span>");
            }
            else if ((pageNumber >= 2)
                && (pageNumber + Constants.One == pagesCount))
            {
                result = string.Concat(a,
                            thread,
                            b,
                            thread,
                            "?p=",
                            pageNumber,
                            "&quot;);return false' title='Предыдущая страница'>◄</a>",
                            "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>");
            }
            else if ((pageNumber == Constants.One) && (pageNumber + 3 <= pagesCount))
            {
                result = string.Concat("<span id='w'>&nbsp;&nbsp;&nbsp;&nbsp;",
                            "<a onClick='n(&quot;/f/",
                            thread,
                            "?p=",
                            pageNumber,
                            "&quot;);return false' title='Предыдущая страница'>◄</a>",
                            "&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='n(&quot;/f/",
                            thread,
                            "?p=",
                            pageNumber + 2,
                            "&quot;);return false' title='Следующая страница'>►</a>&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='n(&quot;/f/",
                            thread,
                            "?p=",
                            pagesCount,
                            "&quot;);return false' title='Последняя страница'>»</a></span>");
            }
            else if ((pageNumber == Constants.Zero) && (pageNumber + 3 <= pagesCount))
            {
                result = string.Concat("<span id='w'>&nbsp;&nbsp;&nbsp;&nbsp;",
                            "&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='n(&quot;/f/",
                            thread,
                            "?p=",
                            pageNumber + 2,
                            "&quot;);return false' title='Следующая страница'>►</a>&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='n(&quot;/f/",
                            thread,
                            "?p=",
                            pagesCount,
                            "&quot;);return false' title='Последняя страница'>»</a></span>");
            }
            else if ((pageNumber == Constants.One) && (pagesCount == 3))
            {
                result = string.Concat("<span id='w'>&nbsp;&nbsp;&nbsp;&nbsp;",
                            "<a onClick='n(&quot;/f/",
                            thread,
                            "?p=",
                            pageNumber,
                            "&quot;);return false' title='Предыдущая страница'>◄</a>",
                            "&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='n(&quot;/f/",
                            thread,
                            "?p=",
                            pageNumber + 2,
                            "&quot;);return false' title='Следующая страница'>►</a>&nbsp;&nbsp;&nbsp;&nbsp;</span>");
            }
            else if ((pageNumber == Constants.One) && (pagesCount == 2))
            {
                result = string.Concat("<span id='w'>&nbsp;&nbsp;&nbsp;&nbsp;",
                            "<a onClick='n(&quot;/f/",
                            thread,
                            "?p=",
                            pageNumber,
                            "&quot;);return false' title='Предыдущая страница'>◄</a>",
                            "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>");
            }
            else if ((pageNumber == Constants.Zero) && (pagesCount == 2))
            {
                result = string.Concat("<span id='w'>&nbsp;&nbsp;&nbsp;&nbsp;",
                            "&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='n(&quot;/f/",
                            thread,
                            "?p=",
                            pageNumber + 2,
                            "&quot;);return false' title='Следующая страница'>►</a>&nbsp;&nbsp;&nbsp;&nbsp;</span>");
            }

            return result;
        }
    }
}