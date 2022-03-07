namespace Own.MarkupHandlers
{
    internal static partial class Marker
    {
        internal static string GenerateLoginPage(in string captchaImageCode)
        {
            return string.Concat("<div id='g'>",
                captchaImageCode,
                "<br /><a>Текст на картинке</a><br><input id='y' maxlength='4' type='text'><br />",
                "<a>Логин</a><br /><input id='l' maxlength='25' type='text'><br />",
                "<a>Пароль</a><br /><input id='q' maxlength='50'",
                " type='password'><br /><div id='c'></div><br />",
                "<a onClick='d();return false'>Войти</a></div>");
        }
    }
}