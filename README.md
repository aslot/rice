# rice

## Простой и быстрый форум для целей тестирования

### Версия 1.0.0.1 для asp.net core 6

Для использования в Linux необходимо установить пакет libgdiplus. Пример команды установки:<br>
sudo apt-get install libgdiplus

Пример команды публикации: <br>
dotnet publish -r win-x86 -p:PublishSingleFile=false -p:PublishTrimmed=false --self-contained false -c Release

#### Функциональность 
аутентификация по капче, логину и паролю<br>
раскрытие списка разделов<br>
создание новой темы<br>
создание нового сообщения темы<br>
создание нового личного сообщения<br>
просмотр списка тем<br>
листание списка тем в конец<br>
листание списка тем в начало<br>
листание списка тем к следующей порции<br>
листание списка тем к предыдущей порции<br>
просмотр списка сообщений темы<br>
листание списка сообщений темы в конец<br>
листание списка сообщений темы в начало<br>
листание списка сообщений темы к следующей порции<br>
листание списка сообщений темы к предыдущей порции<br>
возврат на главную страницу<br>
отправка обратной связи<br>
просмотр списка личных сообщений<br>
листание списка личных сообщений в конец<br>
листание списка личных сообщений в начало<br>
листание списка личных сообщений к следующей порции<br>
листание списка личных сообщений к предыдущей порции<br>
регистрация по капче, логину, паролю, повтору пароля, почте, повтору почты, имени на форуме<br>
занесение данных в анкету профиля<br>
редактирование анкеты профиля<br>
<br>
Требуется установить базу данных скриптом MSSQLSetup.sql для SQL Server.
