# AutoSorter 0.2.4 — Battery duplication fix / Исправление дублирования батарей

## Русский
- Исправлен перенос батарей и других предметов с индивидуальными данными: AutoSorter переносит конкретную ячейку, сохраняя заряд каждого предмета.
- Убрано использование TransferByDef в автоматической сортировке. На установленных игровых библиотеках старый вызов воспроизводимо превращал 4 батареи с разными данными в 7 после одного переноса.
- Перед переносом проверяется лимит ячеек получателя; заполненный контейнер не должен приводить к удалению исходного предмета. Стеки с разными данными не объединяются.
- После каждого переноса проверяется изменение количества в обоих контейнерах. При нарушении сортировка останавливается для текущего мира и записывает ошибку; это диагностика, не автоматическое восстановление предметов.
- Сохранены исправления стыковки, переполнения и защита инструментов из 0.2.3.

Закройте игру, сохраните свой AutoSorter.json и замените файлы папки AutoSorter содержимым AutoSorter-0.2.4.zip. Параметры запуска не меняются. Уже созданные дубликаты не удаляются автоматически.

## English
- Replace definition-wide sorting with per-slot transfers, preserving each battery's charge metadata.
- Reproduced the old TransferByDef duplication with installed game DLLs: four distinct-metadata batteries became seven after one call.
- Preflight destination slot capacity, preserve metadata when selecting merge slots, and verify source/destination quantity deltas after every automatic move.
- Retain 0.2.3 docking, overflow and tool protections. No new settings or dependencies.
- Close the game and replace the AutoSorter folder from the binary archive; preserve customized AutoSorter.json. Existing duplicate items are not removed.

## Validation / Проверка
Release build and automated checks against installed game libraries. Live-world validation remains pending: this is a pre-release. See VALIDATION.md for exact scope. Test the update on a world copy before using the main save.

# 0.2.3

Protect registered tools and weapons during vanilla quick deposit (Alt + RMB). Preserve explicit manual transfers. Includes 0.2.2 routing fixes.

# AutoSorter 0.2.2 — routing fixes / исправления маршрутизации

RU: Запрещён перенос с базы на пристыкованный корабль. Разрешена выгрузка однотипного груза корабля на статическую базу. Добавлено переполнение в пустые контейнеры той же конструкции с доступным складом-образцом. Приоритет более вместительных приёмников; заполненные по массе пропускаются. Защита от смешивания категорий при назначении пустого контейнера. NoSort, владение, инструменты и конвейерные ограничения сохранены.

EN: Block station-to-ship and peer-grid transfers. Allow single-category ship unloading to static stations. Add empty overflow on a grid with reachable seeded storage. Prefer larger destinations within each priority group, skip mass-full stores, and claim overflow categories immediately. Preserve NoSort, ownership, protected tools and conveyor checks.

Limit: mobile space bases are not static stations. Live-world verification is still required. See README-RU.md / README.md.


# Changelog / История изменений

## 0.2.1 — 2026-11-03

### English

- Added an explicit exclusion for registered hand tools and weapons, including modded tools such as AreaTools. Items listed in the loaded game's `ToolsConfiguration.ItemPrefabs` or `GunItems` are protected even if their item definition uses ore, material, or component flags.
- Protected items do not establish a sorting destination and are excluded from automatic transfers between cargo containers. Normal manual inventory transfers remain available.
- Container roles still follow their current contents. A container with tools plus materials is general storage: materials may be sorted out; protected tools stay in place.
- No new settings or launch arguments are required. BuildPlanner remains optional.

**Alt+N is a separate feature of BuildPlanner.** AutoSorter 0.2.1 does not change backpack deposit. For the reported Alt+N problem, use the separately supplied, unofficial `BuildPlanner-1.1.0-tools-fix.1.zip`. Do not load the original and patched BuildPlanner simultaneously. This is not an upstream BuildPlanner release.

Validation: AutoSorter Release build succeeded, 115 routing-policy assertions passed, and isolated installation/removal of both hooks against installed game DLLs passed. The separate BuildPlanner fix passed 28 checks against its actual patched filter and game types, including three AreaTools item identifiers and deliberately incorrect category flags. These 28 checks validate the BuildPlanner patch, not live AutoSorter transfers. Live testing of both updates is still pending. NuGet's online vulnerability audit was unavailable (NU1900).

Update: close the game, back up any customized `AutoSorter.json`, and replace the AutoSorter files with the new archive. Keep the existing launch argument. Retain pre-release status while live validation is pending.

### Русский

- Добавлено явное исключение зарегистрированных инструментов и оружия, включая модовые инструменты AreaTools. Предметы из загруженных игровых списков `ToolsConfiguration.ItemPrefabs` и `GunItems` защищены, даже если в их определениях указаны категории руды, материалов или компонентов.
- Защищённые предметы не назначают категорию склада и не переносятся автоматически между грузовыми контейнерами. Обычное ручное перемещение остаётся доступным.
- Назначение контейнеров по-прежнему определяется содержимым. Контейнер с инструментами и материалами считается общим: материалы могут быть рассортированы, инструменты остаются.
- Новых настроек и параметров запуска не требуется. BuildPlanner остаётся необязательным.

**Alt+N — отдельная функция BuildPlanner.** AutoSorter 0.2.1 не меняет выгрузку рюкзака. Для исправления описанной проблемы с Alt+N предназначен отдельный неофициальный архив `BuildPlanner-1.1.0-tools-fix.1.zip`. Не загружайте оригинальный и исправленный BuildPlanner одновременно. Это не релиз от автора BuildPlanner.

Проверка: AutoSorter собран в Release, прошли 115 проверок правил и отдельная проверка установки/снятия обоих перехватчиков с DLL игры. Отдельный патч BuildPlanner прошёл 28 проверок настоящего исправленного фильтра с игровыми типами, включая три идентификатора AreaTools и заведомо неправильные флаги категорий. Эти 28 проверок относятся к патчу BuildPlanner, а не к живым переносам AutoSorter. Игровые тесты обоих обновлений ещё требуются. Онлайн-аудит зависимостей NuGet был недоступен (NU1900).

Обновление: закройте игру, сохраните копию изменённого `AutoSorter.json` и замените файлы AutoSorter новым архивом. Параметр запуска прежний. Пока игровые тесты не завершены, оставьте статус pre-release.

## 0.2.0

English: introduced content-based container roles without renaming: ore, ingots/materials, components, and general mixed storage. Emptying a container clears its role. Optional NoSort exclusion; English and Russian instructions.

Русский: назначение контейнеров по содержимому без переименования: руда, слитки/материалы, компоненты и общий смешанный склад. Опустошение сбрасывает назначение. Необязательное исключение NoSort; инструкции на английском и русском.



