# TFP-AutoEvent

Это "небольшой" плагин который я набросал для ивентов. Он ещё в разработке, ибо мне ещё нужно накидать хотя-бы один ивент для примера.

## Установка

Соберите проект через Visual Studio, ссылаясь на библиотеки Exiled и пару-тройку библиотек в %DEDICATED SERVER FOLDER%/SCPSL_Data/Managed.
Дальше папка для ивентов будет создана автоматически в %appdata%\EXILED\Configs\TFP-AutoEvents\EventAssemblies

Я обещаю что скоро сделаю авто-билды с коммиитов когда я разберусь с багом с выдачей классов.\
Опять-же, soon™

## Создание ивента

Вы можете написать свой ивент, создав новую библиотеку класса и унаследовав интерфейс IEvent. После чего вам нужно имплементировать все методы/поля и написать код (удивительно)
Пример того есть в Events/FG.cs, но там нет никакой логики из-за бага выше.
