using System.Collections.Generic;
namespace Own.Permanent
{
    internal static class Constants
    {
        public const string LoginRequirement =
              "<span id='n'><a onClick='j();return false'>Войти</a>&nbsp;или&nbsp;" +
              "<a onClick='h();return false'>Зарегистрироваться</a></span>";
        public const string SE = "";
        public const byte EndPointPagesCount = 5;
        public const byte Zero = 0;
        public const byte One = 1;
        public const string PageToReturn = "<p>Получатель сообщения:</p>" +
                 "<input type='text' tabindex='0' autofocus required maxlength='25' autocomplete='off' />" +
                 "<p>Текст сообщения:</p>" +
                 "<textarea id='x' tabindex='1' maxlength='1000' wrap='soft' spellcheck='true'></textarea>" +
                 "<div id='c'></div><br /><span><a id='s' onClick='i();return false'>Отправить</a></span>";
        public const string pMarker = "<p";
        public const string fullSpanMarker = "<span id='w'>";
        public const string endSpanMarker = "</span>";
        public const string brMarker = "<br />";
        public const string endNavMarker = "</nav>";
        public const byte Fifty = 50;
        public const string h2End = "</h2>";
        public const string pStart = "<p onclick='n(&quot;/p/";
        public const string pMiddle = "?p=1&quot;);'>";
        public const string pEnd = "</p>";
        public const string a = "<div class='s'>0</div>";
        public const string indic = "<div class='s'>";
        public const string articleStart = "<article>";
        public const string articleEnd = "</article>";
        public const string answerMarker = "<div id='a'>";
        public const string PrivateReplyPage = "<p>Ваш ответ:</p>" +
                "<textarea id='x' autofocus maxlength='1000' wrap='soft' spellcheck='true'></textarea>" +
                "<div id='c'></div><br /><span><a id='s' onClick='e();return false'>Отправить</a></span>";
        public const string indicEnd = "</div>";
        public const string spanEnd = "</span>";
        public const byte DialogsOnPage = 9;
        public const byte five = 5;
        public const byte threadsOnPage = 9;
        public const byte len = 25;
        public const short LoginPagesCount = 3000;
#if DEBUG
        public const uint TestRegistrationEnterCaptchaHash = 315702632; //"3ЁЗЙ"
        public const uint TestAuthenticationEnterCaptchaHash = 1916808717; //"ЫЪЬЩ" 
#endif
        internal static List<char> AlphabetRusLower =
                             new List<char> { 'а', 'б', 'в', 'г', 'д', 'е', 'ё',
                                'ж', 'з', 'и', 'й', 'к', 'л', 'м', 'н', 'о', 'п',
                                'р', 'с', 'т', 'у', 'ф', 'х', 'ц', 'ч', 'ш', 'щ',
                                'ъ', 'ы', 'ь', 'э', 'ю', 'я' };

        internal static char[] Special = new char[12]{
            '.', ',', '-', ' ', '!', '?', ';', ':', '"', '(', ')', '\0'
        };

        public const string PageToReturnTopic = "<p>Заголовок темы:</p>" +
                "<input type='text' tabindex='0' autofocus required maxlength='99' autocomplete='off' />" +
                "<p>Текст сообщения:</p>" +
                "<textarea id='x' tabindex='1' maxlength='1000' wrap='soft' spellcheck='true'></textarea>" +
                "<div id='c'></div><br /><span><a id='s' onClick='l();return false'>Отправить</a></span>";
        public const int RegistrationPagesCount = 120;
        public const string MainPage = "<!DOCTYPE html><html lang='ru' dir='ltr' spellcheck='false'><head><meta content='text/html;charset=utf-8' http-equiv='Content-Type'><meta charset='utf-8' http-equiv='encoding'>" +
                "<meta name='description' content='Первый мобильный форум бесплатных онлайн-знакомств на русском языке с экономией трафика поможет Вам построить хорошие отношения с партнерами для любых целей посредством добрых бесед; форум любви - это место, где встречаются люди.' />" +
                "<meta name='keywords' content='честные знакомства, порядочные знакомства, бесплатные знакомства, брачные знакомства, поиск друзей, форум любви, форум знакомств, дружеское общение, как познакомиться' />" +
                "<link rel='stylesheet' type='text/css' href='m.css'>" +
                "<meta name='viewport' content='width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no' />" +
                "<base target='_blank' />" +
                "<title>Форум любви</title></head><body><header><h1 onClick='z();' title='На главную'>Форум любви</h1>"
                //+ "<div id='back' title='Назад' onClick='back();'>&#8617;</div>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" 
                + "<div class='p' title='Личные сообщения' onClick='b();'>&#9993;</div>"
                + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                + "<div class='p' title='Анкета' onClick='p();'>&#9786;</div>"//+"&#9829;" 
                                                                              //+"<div id='forward' title='Вперёд' onClick='forward();'>&#8618;</div>"
                + "</header><div id='o'>";
        public const string MainPageEnd = "</div><small><a id='m' href='mailto:russianloveforum@gmail.com'>Обратная связь</a></small>" +
                "<script src='m.js' async></script></body></html>";
        public const string NewDialog = "<div id='d'><span><a onclick=" +
                "'k();return false;'>Новый диалог</a>" +
                        "</span></div><nav class='n'></nav></div>";
        public const string navMarker = "<nav class='n'>";
        public const string spanMarker = "<span";
        public const string endLinkMarker = "»";
        public const string pageMarker = "?p=";
        public const string nextLinkMarker = "►";
        public const string ReplyPage = "<p>Ваш ответ:</p>" +
            "<textarea id='x' autofocus maxlength='1000' wrap='soft' spellcheck='true'></textarea>" +
            "<div id='c'></div><br /><span><a id='s' onClick='s();return false'>Отправить</a></span>";
        public const string buttonTxt
                = "<div id='t'><span><a onClick='q();return false;'>Новая тема</a></span></div>";
        public const char TagStartSymbol = '<';
        public const short MaxReplyMessageTextLength = 1000;
        public const byte MaxForumThreadNameTextLength = 99;
        public const byte MaxNickTextLength = 25;
        public const short MaxFirstLineLength = 10000;
        public const int MaxProfileImageSizeBytes = 500 * 1024;
        public const byte MaxAttemptsCountPerIp = byte.MaxValue;
        public const byte ProfileQuestionsCount = 25;
        public const byte CaptchaLength = 4;
        public const short MinProfileImageSizeBytes = 1024;
        public const short ProfileImageHeightPixels = 809;
        public const short ProfileImageWidthPixels = 500;
        public const string Checked = " checked";
        public const string Yes = " Да.";
        public const string No = " Нет.";
        public const string CaptchaLetters = "0123456789"; //"12456789АБВГДЕЖИКЛМНПРСТУФХЦЧШЭЮЯ";
        public const ushort TimerPeriodMilliseconds = 1000;
    }
}