# Validation — AutoSorter 0.2.4

2026-11-04. Based on published 0.2.3 source (main e0d0ea0d79cc856960acd275549403a961604ce4).

- Original game TransferByDef reproduced creation of extra items: four batteries with distinct ResourceContainerItemMetaData became seven total after one call.
- TransferCheck executes actual installed game inventory-data methods and the new destination-slot selection helper on synthetic inventories. 117 assertions: four batteries stay four, charge metadata preserved, 100 round trips, full mass, full slots, partial ore moves, merge into existing stack at slot limit, and distinct metadata never merged.
- No copied/decompiled game implementation is included in the source or binary distribution. Tests require the user's installed game DLLs.
- Runtime wrapper still uses the game's TransferByIdx inventory-filter checks. After each automatic transfer, source and destination quantity deltas must both match the returned amount; otherwise sorting stops for the session. This diagnostic does not roll back an unexpected engine or third-party failure.
- No world loaded or modified. Conveyor topology, live UI interaction, other-plugin interaction and real save behavior remain pending. Release is marked pre-release.
- NuGet online vulnerability audit disabled for offline cached build; dependency versions unchanged.

Reproduce with a .NET 9 SDK:
```
dotnet build AutoSorter.csproj -c Release -p:NuGetAudit=false
dotnet run --project tests -c Release -p:NuGetAudit=false
dotnet run --project tools/TransferCheck -c Release -p:NuGetAudit=false -- "D:\SteamLibrary\steamapps\common\SpaceEngineers2\Game2"
dotnet run --project tools/LoadCheck -c Release -p:NuGetAudit=false -- bin/Release/net9.0 "D:\SteamLibrary\steamapps\common\SpaceEngineers2\Game2"
dotnet run --project tools/ToolProtectionCheck -c Release -p:NuGetAudit=false -- bin/Release/net9.0 "D:\SteamLibrary\steamapps\common\SpaceEngineers2\Game2"
```

Verified results: Release build 0 warnings / 0 errors; 251 routing assertions; 117 actual-game inventory-data assertions; all four hooks installed/disposed and nested/exception/thread guard scope checks passed.

