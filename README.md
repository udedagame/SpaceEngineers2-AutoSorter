# AutoSorter 0.2.0 for Space Engineers 2

[Русская инструкция](README-RU.md)

Automatically sort cargo by what is already inside each container. No category tags or block renaming required. AutoSorter is a standalone **plugin**, not a Steam Workshop mod. BuildPlanner is not required.

**Experimental build:** compilation, 115 policy assertions, and isolated hook installation against the developer's installed game DLLs passed. Transfers in a running world have not yet been tested. Start with a test world or a copy of your save.

## How it works

Connect your cargo containers with conveyors and put a sample into each destination container:

| Current contents | Container role |
|---|---|
| Ore only, including multiple ore types | Accepts ore |
| Ingots/materials only (the game's `Material` category) | Accepts ingots/materials |
| Components only | Accepts components |
| Empty, mixed categories, or other items present | General storage: a source for sorting |

For example, put an iron ingot in one container, some ore in another, and mixed cargo in a third. AutoSorter attempts to move the mixed cargo into the matching destinations every two seconds. A component sample creates a component destination.

Assignments are recalculated from current contents; they are not saved permanently:

- Empty a container to clear its assignment. Add a different sample to change its role.
- Add another category to make a container general storage. General storage can hold anything, but sortable items may be removed from it.
- If sorting leaves only one category in a general container, it becomes a destination for that category on the next pass.
- Empty containers are not filled automatically. Put a sample in first.
- Correctly stored items stay where they are. Containers of the same category do not drain or balance each other.
- If a destination fills up, another matching reachable destination is tried. Items without a suitable destination stay at their source.

Optional exclusion: add `[NoSort]` to a container's name to leave it completely untouched. `[Не сортировать]` also works. Exclusion tags are case insensitive. Old `[Ore]`, `[Ingots]`, and `[Components]` name tags do not assign categories in version 0.2.0.

## Language and scope

Classification uses internal item types, not translated names. It does not depend on the game's interface language, including English and Russian. Logs and configuration keys are in English. Live gameplay in different languages has not been separately tested.

Only loaded vanilla cargo containers owned by the local player and connected to a conveyor system are considered. Conveyor direction and per-item filters are checked before transfer. Character and production inventories are not sorted. Tools and categories other than ore, materials, and components are left in place.

This version targets a local world with server simulation in the game process. Remote servers and dedicated servers are not supported. Custom modded cargo types may not be recognized. Game updates may break the internal API hooks.

## Installation

1. Close the game. Extract the **entire AutoSorter folder** from `AutoSorter-0.2.0.zip` to a permanent writable location, for example `D:\SE2Mods\AutoSorter`. Keep every bundled DLL together.
2. In Steam, right-click **Space Engineers 2 → Properties → General → Launch Options**.
3. To run AutoSorter on its own, enter:

```text
"-plugins:D:\SE2Mods\AutoSorter\AutoSorter.dll"
```

Use your actual installation path. If you also use BuildPlanner, combine the paths in **one** launch argument:

```text
"-plugins:D:\SE2Mods\BuildPlanner\BuildPlanner.dll;D:\SE2Mods\AutoSorter\AutoSorter.dll"
```

If you already have a `-plugins:` argument, append AutoSorter's path to it with a semicolon. Do not add a second `-plugins:` argument: the loader reads the first one. Keep any unrelated launch options.

4. Start the game. Look for `AutoSorter 0.2.0 ready` and `Input host captured` in `AutoSorter.log` next to the DLL.
5. Load your test world, connect cargo containers, and place the samples. The log reports eligible inventories and destinations every 30 seconds. `moved` counts successful transfer operations, not individual items.

To update, close the game and replace the files in the AutoSorter folder with the new archive. Back up your `AutoSorter.json` first if you customized it. There is no in-game settings panel in this version.

## Settings

Edit `AutoSorter.json` next to the DLL and restart the game:

| Setting | Default | Meaning |
|---|---|---|
| `Enabled` | `true` | Enable sorting |
| `IntervalSeconds` | `2` | Seconds between passes; range 0.5–60 |
| `MaxTransferAttempts` | `64` | Attempts per pass; range 1–1024 |
| `MaxItemChecks` | `256` | Source/item-type pairs checked per pass; range 1–4096 |
| `SoftTimeBudgetMs` | `10` | Soft routing budget; range 1–100 ms |
| `DryRun` | `false` | Find candidate destinations without moving items |

The time budget is not a strict frame-time limit: discovering inventories and an individual engine call can take additional time. `dryRunCandidates` counts possible destinations, not predicted transfer amounts. A round-robin cursor limits repeated preference for the first work items.

Sorting skips paused, saving, or unloading worlds. A sorting exception disables it for that world session and records the error; reload the world after resolving the issue. The log rotates at approximately 2 MB and retains one previous file.

## First in-game test

1. Connect two sample containers (ore and ingots) and a third with mixed ore and ingots. Check both routing and total quantities before and after.
2. Seed a component container and check component transfers from mixed storage.
3. Empty a destination and verify that it stops receiving items. Seed it with a different category and verify its new role.
4. Mix categories in a destination. Check that it becomes a source. Check that tools remain in a mixed inventory.
5. Test `[NoSort]`, a full destination plus a second free destination, disconnected conveyors, and blocking/one-way sorters.
6. Check pause, world reload, and coexistence with BuildPlanner if installed.

Keep `AutoSorter.log` when reporting a problem. Include the plugin and game versions and a description of the conveyor layout.

## Uninstall

Remove AutoSorter's path from the `-plugins:` argument. No save conversion is needed. The plugin adds no blocks and does not modify game files. Item movements already saved by the game remain part of the world.

## Building from source

Requires .NET SDK 9 and your own installed copy of Space Engineers 2. From the source directory:

```powershell
dotnet build AutoSorter.csproj -c Release -p:GameDir="D:\SteamLibrary\steamapps\common\SpaceEngineers2\Game2"
dotnet run --project tests/AutoSorter.Tests.csproj -c Release
```

The policy tests do not replace in-game validation of inventories, capacity, conveyors, or saves. See `VALIDATION.md` for the verification limits.

## Credits and licenses

Developed using [BuildPlanner by Vito Pistelli](https://github.com/viligante8/SpaceEngineers2BuildPlanner) as a reference for plugin loading and inventory API usage. BuildPlanner is MIT licensed; its notice is included. AutoSorter and bundled third-party notices are included in `LICENSE`, `LICENSE-BuildPlanner.txt`, and `THIRD-PARTY-NOTICES.txt`. Game DLLs and decompiled game source are not distributed in this package.
