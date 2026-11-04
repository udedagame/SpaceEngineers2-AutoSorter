# AutoSorter 0.2.3 for Space Engineers 2

[Русская инструкция](README-RU.md)

Fixes cargo destination selection and docked-ship unloading. Build and automated checks are available; live world transfers still require in-game validation.

## Behavior

Cargo containing only ore, only materials, or only components becomes a destination for that category. Mixed cargo supplies the matching destinations. Tools and weapons are protected.

Matching seeded destinations are tried first, largest mass capacity first. Mass-full destinations do not consume transfer attempts. Remaining cargo can overflow into empty containers on a grid with a reachable seeded destination for that category. Larger empty containers are tried first. A successful overflow transfer assigns the destination immediately for the rest of the pass, preventing different categories from mixing. Subsequent passes derive its role from its contents. Mixed inventories never receive automatic overflow. Unmovable items remain at the source.

Correctly sorted containers on the same grid do not drain or balance one another. Empty grids without a reachable category seed are not automatically assigned.

Across different grids, only mobile-to-static transfers are allowed. A station never feeds a docked ship. Ship-to-ship and separate station-to-station automatic transfers are blocked. A single-category ship cargo can unload into station storage, including ore-only cargo.

A station is identified by its stored GridPhysicsComponent.GridMotionType (Static), not speed or parking brake. A mobile space base is not treated as a static station; transfers between two mobile grids are disabled. Sorting within either grid continues normally.

Use `[NoSort]` or `[Не сортировать]` to protect a container, including ship reserves. Excluded containers are not sources, destinations, or category seeds. Legacy category name tags do not assign a category.

Only functional vanilla cargo containers owned by the local player participate. Actual-item conveyor reachability and direction are checked. Character and production inventories are excluded. Local worlds with in-process server simulation are supported; remote/dedicated servers are not.

## Install/update

Close the game. Extract the entire AutoSorter folder into `D:\SE2Mods\AutoSorter`, replacing the previous files. Preserve a customized AutoSorter.json; no new mandatory settings were added. Keep your existing plugin launch options, including BuildPlanner if present. Standalone example:

```text
"-plugins:D:\SE2Mods\AutoSorter\AutoSorter.dll"
```

Check the version in AutoSorter.log and test on a world copy. Verify a full small seeded store plus empty large overflow, multiple seeded stores, ore-only docked ship unloading, station-to-ship protection, two docked ships, NoSort, conveyor filters, full destinations, and conservation of item totals.

## Settings and build

AutoSorter.json is read at launch. Enabled toggles sorting; IntervalSeconds defaults to 2. MaxTransferAttempts and MaxItemChecks bound work per pass. SoftTimeBudgetMs is a soft routing budget, not a hard bound on scanning the entire world. DryRun logs candidates without transfers and does not predict exact capacity.

```powershell
dotnet build AutoSorter.csproj -c Release -p:GameDir="D:\SteamLibrary\steamapps\common\SpaceEngineers2\Game2"
dotnet run --project tests/AutoSorter.Tests.csproj -c Release
```

Exceptions disable sorting for the world session and are logged. Reload after addressing the cause. No save conversion or custom blocks are required; saved item movements remain part of the world.

Plugin loading was informed by Vito Pistelli's BuildPlanner (MIT). Original license notices are preserved. Game DLLs are not distributed.


## Quick-deposit tool protection (0.2.3)

Protects the vanilla bulk-deposit interaction used while aiming at a container (Alt + right mouse button). The game checks EssentialItems, which may omit modded tools. AutoSorter additionally excludes registered hand tools and weapons, including AreaTools, only during that synchronous bulk operation. Explicit manual inventory transfers remain available. No game or mod files are changed. Enabled=false or DryRun=true disables this guard. BuildPlanner Alt+N is a separate operation and requires BuildPlanner's own tool-protection fix.
