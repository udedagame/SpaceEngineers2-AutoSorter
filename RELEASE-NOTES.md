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
