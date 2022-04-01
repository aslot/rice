# rice

## Простой и быстрый форум для эксплуатации в условиях высокой нагрузки

### Версия 1.0.1.0 для asp.net core mvc 6

Код последней версии опубликован на самом дешёвом российском shared-хостинге по адресу <a href="https://форумлюбви.рф">https://форумлюбви.рф</a>

Ветвь net6_0 - версия 1.0.1.0 для asp.net core mvc 6<br>

Определение номера версии:<br>
Номер состоит из четырёх неотрицательных целых чисел, разделённых точкой, и соответствует шаблону A.B.C.D, где A обозначает номер улучшения или исправления безопасности, B - номер улучшения или исправления функционала, С - номер улучшения или исправления производительности, D - номер прочих улучшений и исправлений (D = 0 - релиз, иначе - beta-версия), причём увеличение одного из чисел старших разрядов номера версии обнуляет все младшие разрады. К примеру, если обновляется функционал и добавляются прочие исправления, но не изменяется производительность, то инкрементируется номер обновления функционала, как более важный параметр.<br>

Код в папке FrameworkFree не привязан к фреймворку asp.net, поэтому может быть подключен к HttpListener, что должно повысить быстродействие, но хостинг при этом выйдет дороже.<br>

Код сторонних разработчиков помещен в namespace Inclusions.<br>
В программе использован алгоритм хэширования XXHash (<a href="https://github.com/shibox/XXHash">https://github.com/shibox/XXHash</a>).<br>

Методам, которые не могут вернуть данные, назначен постфикс Void.<br>
Методам, возвращаемый тип которых может принимать значение null, назначен постфикс Nullable.<br>
Методам in-memory хранилища, использующим внутреннюю блокировку lock, назначен постфикс Locked.<br>
Непротестированные методы бизнес-логики находятся в class Unstable и после тестирования будут перенесены в class Stable.<br>
Собственный функционал приложения находится в namespace Own.<br>
Методы бизнес-логики, которые выполняются последовательно и не требуют специальной блокировки, находятся в namespace Own.Sequential.<br>
Методы бизнес-логики, к которым осуществляется конкурентный доступ, и которые требуют использования блокировки, находятся в namespace Own.InRace.<br>
Методы доступа к данным дискового хранилища находятся в классе Slow.<br>
Методы доступа к данным in-memory хранилища и требующие оптимизации находятся в классе Fast.<br>
Имена членов данных in-memory хранилища имеют постфикс Static.<br>
Имена объектов блокировки имеют постфикс Locker.<br>

Производительность приложения и нагрузка на сервер регулируются заданием величины периода тактирования Constants.TimerPeriodMilliseconds в миллисекундах. Меньшая величина даёт большие производительность и нагрузку и наоборот.<br>

Не зависящий от фреймворка asp.net код взаимодействует с внешним окружением через класс FriendlyFire.<br>

Для корректной работы в базе данных должны храниться хотя бы одно личное сообщение и хотя бы один аккаунт (они изначально записываются скриптом установки базы данных).

Пакет Linux libgdiplus больше не требуется.

Примеры команды публикации: <br>
dotnet publish -r win-x86 -p:PublishSingleFile=false -p:PublishTrimmed=false --self-contained false -c Release
На хостинге: dotnet publish -r win10-x86 -p:PublishSingleFile=true -p:PublishTrimmed=false --self-contained false -c Release -p:PublishReadyToRun=true

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
Требуется установить базу данных скриптом MSSQLSetup.sql для SQL Server и задать строки подключения к серверам баз данных (можо использовать одинаковые) для переменных среды RELEASE (Production) и DEBUG (Test).

<br><b>This engine is still under development and strongly not recommended for production usage.</b>