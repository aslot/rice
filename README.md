# rice

## Простой и быстрый форум для целей тестирования

### Версия 1.0.0.2 для asp.net core 6

Код последней версии опубликован на самом дешёвом российском shared-хостинге по адресу <a href="https://форумлюбви.рф">https://форумлюбви.рф</a>

Ветвь net6_0 - версия 1.0.0.2 для asp.net core 6<br>

Код в папке FrameworkFree не привязан к фреймворку asp.net, поэтому может быть подключен к HttpListener, что должно повысить быстродействие, но хостинг при этом выйдет дороже.<br>

Код сторонних разработчиков помещен в namespace Inclusions.<br>
В программе использован алгоритм хэширования XXHash (<a href="https://github.com/shibox/XXHash">https://github.com/shibox/XXHash</a>).<br>

Методам, которые не могут вернуть данные, назначен постфикс Void.<br>
Методам, возвращаемый тип которых может принимать значение null, назначен постфикс Nullable.<br>
Методам in-memory хранилища, использующим внутреннюю блокировку lock, назначен постфикс Locked.<br>
Непротестированные методы бизнес-логики находятся в class Unstable и после тестирования будут переноситься в class Stable.<br>
Собственный функционал приложения находится в namespace Own.<br>
Методы бизнес-логики, которые выполняются последовательно и не требуют специальной блокировки, находятся в namespace Own.Sequential.<br>
Методы бизнес-логики, к которым осуществляется конкурентный доступ, и которые требуют использования блокировки, находятся в namespace Own.InRace.<br>
Методы доступа к данным дискового хранилища находятся в классе Slow.<br>
Методы доступа к данным in-memory хранилища и требующие оптимизации находятся в классе Fast.<br>

Не зависящий от фреймворка asp.net код взаимодействует с внешним окружением через класс FriendlyFire.<br>

Для корректной работы в базе данных должны храниться хотя бы одно личное сообщение и хотя бы один аккаунт (они изначально записываются скриптом установки базы данных).

Пакет Linux libgdiplus больше не требуется.

Пример команды публикации: <br>
dotnet publish -r win-x86 -p:PublishSingleFile=false -p:PublishTrimmed=false --self-contained false -c Release

Версия 1.0.0.2 улучшает производительность, устраняет некоторые известные баги, но не добавляет новую функциональность.

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
регистрация по капче, логину, паролю, повтору пароля, секретному слову, повтору секретного слова, имени на форуме<br>
занесение данных в анкету профиля<br>
редактирование анкеты профиля<br>
<br>
Требуется установить базу данных скриптом MSSQLSetup.sql для SQL Server.
