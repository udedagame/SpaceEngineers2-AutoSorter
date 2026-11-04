# AutoSorter 0.2.3 — routing fixes / исправления маршрутизации

RU: Запрещён перенос с базы на пристыкованный корабль. Разрешена выгрузка однотипного груза корабля на статическую базу. Добавлено переполнение в пустые контейнеры той же конструкции с доступным складом-образцом. Приоритет более вместительных приёмников; заполненные по массе пропускаются. Защита от смешивания категорий при назначении пустого контейнера. NoSort, владение, инструменты и конвейерные ограничения сохранены.

EN: Block station-to-ship and peer-grid transfers. Allow single-category ship unloading to static stations. Add empty overflow on a grid with reachable seeded storage. Prefer larger destinations within each priority group, skip mass-full stores, and claim overflow categories immediately. Preserve NoSort, ownership, protected tools and conveyor checks.

Limit: mobile space bases are not static stations. Live-world verification is still required. See README-RU.md / README.md.


## 0.2.3: Alt + ПКМ / Alt + right mouse button

RU: Добавлена защита зарегистрированных инструментов и оружия, включая AreaTools, при штатной массовой выгрузке в контейнер. Ручное перемещение инструментов через инвентарь доступно. Все исправления складов и стыковки из 0.2.2 включены.

EN: Registered tools and weapons, including AreaTools, are now protected during vanilla bulk deposit into a container. Explicit manual inventory transfers still work. Includes all 0.2.2 storage and docking fixes.

Download the plugin ZIP for installation; the separate source ZIP contains the complete buildable source. Local-world pre-release: real in-game transfers remain unverified.
