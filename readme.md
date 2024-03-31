
# Serilog_Example

В этом решении показано, как правильно пользоваться логгером в проектах различного типа,
оперируя абстракциями, определёнными в составе сборки `Microsoft.Extensions.Logging.Abstractions`,
при этом "под капотом" используя реализацию от `Serilog`.

В каждом проекте находится свой собственный файл `readme.md` с подробной информацией о проекте.

Перед запуском проектов данного решения обязательно установите на своём локальном компьютере
приложение [Seq](https://datalust.co/seq) - оно предоставляет удобный Web-интерфейс для работы
со структурированным логом (приложения данного решения будут писать логи в т.ч. и в **Seq**).

Каждый проект пишет логи сразу в несколько приёмников:

  * Вывод в консоль
  * Вывод в файл. Например, в проекте **WebApplication1** показано, как настраивать _пользовательский_
	формат сообщений, сохраняемых в файл, а в проекте **WorkerService1** показано, как сохранять
	в файле сообщения в компактном JSON-формате.
  * Запись в журнал Windows
  * Отправка структурированного лога в локальный [Seq](https://datalust.co/seq) - этот лог можно
	просматривать в браузере по адресу <http://localhost:5341/#/events>. Присутствует гибкая возможость
	фильтрации, сортировки и т.п.

Если в любом из проектов данного решения (за исключением **ConsoleApp1**) закомментировать строку кода,
регистрирующую `Serilog` (эта строка будет разной, в зависимости от типа проекта), то по умолчанию будет
работать реализация от Microsoft. Эта реализация будет писать логи только в консоль и в журнал Windows.

## Проекты в составе решения

  * **WebApplication1** - Web-сервис. Приложение создано на основе шаблона `ASP.NET Core Web API`. Платформа: `.NET 8`.
  * **WorkerService1** - Worker-сервис. Приложение создано на основе шаблона `Worker Service`. Платформа: `.NET 8`.
  * **ConsoleApp1** - Консольное приложение, созданное на основе шаблона `Console Application`. Платформа: `.Net Framework 4.6.2`.
