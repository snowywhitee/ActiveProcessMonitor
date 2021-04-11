# Active Process Monitor

отображает список текущих процессов

*Прочитать версию на другом языке: [English](README.md)*

<details open="open">
  <summary>Содержание</summary>
  <ol>
    <li>
      <a href="#about-the-project">Про проект</a>
      <ul>
        <li><a href="#implemented-functionality">Реализованный Функционал</a></li>
        <li><a href="#built-with">Используемые пакеты/дополнения</a></li>
        <li><a href="#structure">Структура Проекта</a></li>
      </ul>
    </li>
    <li><a href="#usage">Использование</a></li>
  </ol>
</details>



## Про проект

ТЗ на проект:

**Необходимо написать программу "process manager" на языке C#, которая состоит из двух частей:**

* UI приложение (WPF, Windows Forms, Avalonia, ..), которое показывает список запущенныхпроцессов. Однако, само приложение не должно считать этот список. Вместо этого оно запускает второе приложение (сервис) и вычитывает этот список процессов из него.
* Сервис (headless cosole application) которое запускается из основного процесса и постоянно мониторит список активных процессов в системе. Когда он меняется, сервис отправляет данные в главное приложение, чтобы оно обновило UI.

Протокол между этими двумя процессами может быть люой: stdout, text file, named pipes, protobuf, ..

### Реализованный Функционал

Для UI использовалась Avalonia, чтобы сделать приложение кросс-платформенным.
Для обменя информацией между процессами использовались Named pipes.

Программа имитирует Activity Monitor (OS X) - динамически обновляет таблицу с процессами.

* Сервис не отправляет список процессов целиком, только изменения в нём.
* ObservableCollectionsEx (Из dynamic binding) используется для динамического обновления таблицы без дополнительного кода.
* Вся информация о процессах берется из System.Diagnostics.Process.GetProcesses().


### Используемые пакеты/дополнения

* [AvaloniaUI](https://github.com/AvaloniaUI/Avalonia) Для UI


### Структура Проекта

Решение состоит из двух частей: UI (ActiveProcessMonitor), и service (ProcessObserver).
Для обмена информацией между процесами используются NamedPipeClientStream и NamedPipeServerStream из System.IO.Pipes соответственно в статических классах PipeServer и PipeClient. Они отвечают за отправку/получение изменений в списке, поэтому MainWindowView не инициализируется, пока оба процесса не подключатся к pipe.


## Использование

Вид UI:

![alt text](screenshots/Home.png "Home/Index")
![alt text](screenshots/Encrypt.png "Action/Encrypt")
![alt text](screenshots/Decrypt.png "Action/Decrypt")
![alt text](screenshots/Text.png "Action/Text")
![alt text](screenshots/Result.png "Action/Result")
![alt text](screenshots/Download.png "Action/Download")

