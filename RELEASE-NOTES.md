# AutoSorter 0.2.1 — Experimental / Экспериментальная версия

**English** · Русская версия ниже ↓

## English

### What's new in 0.2.1

Registered hand tools and weapons, including modded AreaTools, are now explicitly excluded from automatic cargo transfers even if a mod assigns ore/material/component flags. Manual transfers remain available. No new settings are required.

**Alt+N backpack deposit belongs to BuildPlanner.** Fixing that operation requires the separate, unofficial **BuildPlanner-1.1.0-tools-fix.1.zip**; updating AutoSorter alone does not change Alt+N. BuildPlanner is not required for AutoSorter itself. Do not load original and patched BuildPlanner at the same time.


### Automatic cargo sorting without renaming containers

AutoSorter is a standalone plugin for Space Engineers 2. It sorts ore, ingots/materials, and components between your connected cargo containers. **BuildPlanner is not required.** Classification uses internal item types and does not depend on the game's interface language.

### How to use

1. Put only ore in a container to make it an ore destination.
2. Put only ingots/materials in another to make it a material destination.
3. Put only components in a third to make it a component destination.
4. Mixed cargo in other containers will be sorted into matching destinations, normally every two seconds.

Empty containers wait for a sample. Emptying a container clears its assignment. Mixing categories makes it general storage, from which sortable items may be removed. If only one category remains after sorting, that container becomes a destination for it on the next pass. Correctly stored items are not balanced between containers of the same category.

Optional: add `[NoSort]` to a container's name to exclude it completely. Conveyor direction and item filters are checked. Character and production inventories are not sorted.

### Installation

1. Download **AutoSorter-0.2.1.zip** from **Assets** below. Use this plugin archive rather than GitHub's automatically generated “Source code” downloads.
2. Close the game and extract the entire **AutoSorter** folder to a permanent writable location, for example `D:\SE2Mods\AutoSorter`. Keep all bundled DLLs together.
3. In Steam, open **Space Engineers 2 → Properties → General → Launch Options** and enter:

```text
"-plugins:D:\SE2Mods\AutoSorter\AutoSorter.dll"
```

Use your actual folder path. If you also use BuildPlanner, use one combined argument:

```text
"-plugins:D:\SE2Mods\BuildPlanner\BuildPlanner.dll;D:\SE2Mods\AutoSorter\AutoSorter.dll"
```

If you already have a `-plugins:` argument, append the path with `;` inside that argument. Do not add a second `-plugins:` argument. Preserve unrelated launch options.

4. Start the game and load a test world. Connect the containers with conveyors and place your samples.
5. Check `AutoSorter.log` next to the DLL for `AutoSorter 0.2.1 ready` and `Input host captured`.

### Settings, updating and removal

Edit **AutoSorter.json** next to the DLL and restart the game. Set `Enabled` to `false` to disable sorting or `DryRun` to `true` to inspect candidate destinations without moving items. Full settings are explained in the included **README.md**.

To update, close the game and replace the plugin files. Back up your JSON settings first if customized. To uninstall, remove AutoSorter's path from the launch argument. Item movements already saved by the game remain in your world.

### Release status

**Experimental pre-release.** Build, 115 policy assertions, and isolated hook installation against the developer's installed game DLLs passed. Actual transfers in a running world have not yet been validated. Start with a test world or a copy of your save.

This version targets loaded vanilla cargo containers owned by the local player in local worlds. Remote and dedicated servers are not supported. Game updates may require a plugin update. This is a manually installed plugin; it is not installed through Steam Workshop.

When reporting an issue, attach **AutoSorter.log** and include your game version, plugin version, and what happened.

---

## Русский

### Что нового в 0.2.1

Зарегистрированные инструменты и оружие, включая модовые AreaTools, теперь явно исключены из автоматических переносов между контейнерами, даже если мод назначил им категории руды, материалов или компонентов. Ручное перемещение доступно. Новых настроек не требуется.

**Выгрузку рюкзака по Alt+N выполняет BuildPlanner.** Для неё нужен отдельный неофициальный архив **BuildPlanner-1.1.0-tools-fix.1.zip**; одно обновление AutoSorter не меняет Alt+N. Сам AutoSorter не требует BuildPlanner. Не загружайте оригинальный и исправленный BuildPlanner одновременно.


### Автоматическая сортировка без переименования контейнеров

AutoSorter — самостоятельный плагин для Space Engineers 2. Раскладывает руду, слитки/материалы и компоненты по подключённым грузовым контейнерам. **BuildPlanner не требуется.** Категории определяются по внутренним типам предметов и не зависят от языка интерфейса игры.

### Как пользоваться

1. Положите только руду в контейнер — он станет складом руды.
2. Положите только слитки/материалы в другой — он станет складом материалов.
3. Положите только компоненты в третий — он станет складом компонентов.
4. Смешанный груз в остальных контейнерах будет разбираться по подходящим складам, обычно каждые две секунды.

Пустой контейнер ждёт образец. Если убрать последний предмет, назначение сбрасывается. Смесь категорий делает контейнер общим: из него сортировщик может забирать вещи. Если после сортировки остаётся одна категория, на следующем проходе контейнер становится складом этой категории. Уже правильно лежащие вещи не перекладываются между однотипными складами.

По желанию добавьте `[NoSort]` или `[Не сортировать]` в название контейнера, чтобы полностью исключить его. Проверяются направление конвейеров и фильтры предметов. Инвентари персонажа и производства не сортируются.

### Установка

1. Скачайте **AutoSorter-0.2.1.zip** в разделе **Assets** ниже. Для установки нужен именно архив плагина, а не автоматически созданные GitHub архивы «Source code».
2. Закройте игру и распакуйте всю папку **AutoSorter** в постоянное место с правом записи, например `D:\SE2Mods\AutoSorter`. Все DLL должны лежать вместе.
3. В Steam откройте **Space Engineers 2 → Свойства → Общие → Параметры запуска** и укажите:

```text
"-plugins:D:\SE2Mods\AutoSorter\AutoSorter.dll"
```

Используйте свой фактический путь. Если нужен также BuildPlanner, укажите один общий параметр:

```text
"-plugins:D:\SE2Mods\BuildPlanner\BuildPlanner.dll;D:\SE2Mods\AutoSorter\AutoSorter.dll"
```

Если `-plugins:` уже есть, добавьте путь через `;` внутри существующего параметра. Второй `-plugins:` добавлять не нужно. Остальные параметры запуска сохраните.

4. Запустите игру и загрузите тестовый мир. Соедините контейнеры конвейерами и положите образцы.
5. Проверьте файл `AutoSorter.log` рядом с DLL: там должны появиться `AutoSorter 0.2.1 ready` и `Input host captured`.

### Настройки, обновление и удаление

Настройки находятся в **AutoSorter.json** рядом с DLL. После изменения перезапустите игру. `Enabled: false` отключает сортировку; `DryRun: true` включает поиск подходящих целей без переноса вещей. Полное описание настроек есть в приложенном **README-RU.md**.

Для обновления закройте игру и замените файлы плагина. Если меняли JSON, сначала сохраните копию настроек. Для удаления уберите путь AutoSorter из параметра запуска. Уже сохранённые игрой перемещения предметов останутся в мире.

### Статус выпуска

**Экспериментальная предварительная версия.** Сборка, 115 проверок правил и отдельная проверка установки перехватчиков с DLL установленной игры прошли. Реальные переносы в запущенном мире пока не проверены. Начните с тестового мира или копии сохранения.

Версия рассчитана на загруженные ванильные грузовые контейнеры владельца текущего игрока в локальных мирах. Удалённые и выделенные серверы не поддерживаются. После обновлений игры может потребоваться обновление плагина. Установка выполняется вручную; это не мод из Steam Workshop.

При сообщении об ошибке приложите **AutoSorter.log**, укажите версии игры и плагина и опишите, что произошло.

---

**Credits / Благодарности:** [BuildPlanner by Vito Pistelli](https://github.com/viligante8/SpaceEngineers2BuildPlanner) — reference for plugin loading and inventory API usage / ориентир по загрузке плагина и работе с API инвентарей. License notices are included / уведомления о лицензиях приложены.


