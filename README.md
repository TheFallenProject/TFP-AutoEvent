# TFP-AutoEvent

Это "небольшой" плагин который я набросал для ивентов. Я думаю в будущем как-то сделать интеграцию с MapEditorReborn, но пока мне лень.
Также есть баг с сменой класса игрока, который при смене класса через ReferenceHub решает крашнуть процесс сервера ссылаясь на Memory Access Violation в логах Local Admin. Мейби я с этим разберусь, мейби нет.
Если кто-то хочет помочь, то вот ошибка которая вызывает краш сервера:
```
[STDOUT]   ERROR: SymGetSymFromAddr64, GetLastError: 'Attempt to access invalid address.' (Address: 00007FFB3DEB812D)
[STDOUT] 0x00007FFB3DEB812D (UnityPlayer) (function-name not available)
[STDOUT]   ERROR: SymGetSymFromAddr64, GetLastError: 'Attempt to access invalid address.' (Address: 00007FFB3DCFAA9C)
[STDOUT] 0x00007FFB3DCFAA9C (UnityPlayer) (function-name not available)
[STDOUT]   ERROR: SymGetSymFromAddr64, GetLastError: 'Attempt to access invalid address.' (Address: 00007FFB3DF5CCD6)
[STDOUT] 0x00007FFB3DF5CCD6 (UnityPlayer) (function-name not available)
[STDOUT]   ERROR: SymGetSymFromAddr64, GetLastError: 'Attempt to access invalid address.' (Address: 00007FFB3DB644F2)
[STDOUT] 0x00007FFB3DB644F2 (UnityPlayer) (function-name not available)
[STDOUT]   ERROR: SymGetSymFromAddr64, GetLastError: 'Attempt to access invalid address.' (Address: 00007FFB3D95F1AA)
```
Мейби это как-то связано с CedMod, но я тут хз

## Установка

Соберите проект через Visual Studio, ссылаясь на библиотеки Exiled и пару-тройку библиотек в %DEDICATED SERVER FOLDER%/SCPSL_Data/Managed.
Дальше папка для ивентов будет создана автоматически в %appdata%\EXILED\Configs\TFP-AutoEvents\EventAssemblies

Я обещаю что скоро сделаю авто-билды с коммиитов когда я разберусь с багом с выдачей классов.\
Опять-же, soon™

## Создание ивента

Вы можете написать свой ивент, создав новую библиотеку класса и унаследовав интерфейс IEvent. После чего вам нужно имплементировать все методы/поля и написать код (удивительно)
Пример того есть в Events/FG.cs, но там нет никакой логики из-за бага выше.
