# Validation — AutoSorter 0.2.3

2026-11-04

- Starting source: local AutoSorter 0.2.1 source; its binary SHA-256 matched installed D:\SE2Mods\AutoSorter\AutoSorter.dll: 5453911ECDA84DDB68DAF149F85D7C0D982B3A4EB30F82AD5CD35CB4FC637070.
- Release build against installed Space Engineers 2 game assemblies: successful, no C# warnings/errors.
- NuGet emitted NU1900 because its vulnerability-data endpoint was unreachable. Existing cached dependencies were used; no dependency versions changed. Vulnerability audit was not completed.
- Standalone regression suite: 251 assertions passed, covering classification, exclusions, same-grid storage, empty overflow policy, category claims, station-to-ship blocking, mobile-to-static unloading, peer-grid isolation, and a simulated full-store/overflow scenario with conserved item count.
- Hook smoke check: both input and session-update hooks installed and removed against actual installed game DLLs.
- No live world loaded or modified. Capacity behavior of real inventories, connector topology, static-station detection in an actual docked world, conveyor filters and multiplayer are NOT validated by the standalone tests or hook smoke check.

A saved GridMotionType is used instead of transient body motion/parking state. Mobile space bases remain mobile; cross-grid transfer between mobile grids is intentionally disabled. Live-world checklist is in README-RU.md and README.md.


0.2.3 additional validation:
- Local installed AreaTools registry: three registered item GUIDs found in ToolsConfiguration_AreaTools.partialdef. No mod files are bundled.
- Confirmed UseConveyorAction.DepositNonEssentialItems calls vanilla InProcessGameServerSessionComponent bulk deposit; the original bulk loop checks EssentialItems and calls TransferByIdx synchronously.
- 28 assertions against the built protected-tool registry and actual game types passed, including all three AreaTools GUIDs with ore/material/component/all flags and ordinary unregistered materials.
- All four hooks installed successfully against installed game DLLs; plugin disposal removed them.
- Guard scope tests passed for nested scopes, exception cleanup, cross-thread isolation and no active scope outside bulk deposit.
- The hook smoke test does not execute actual inventory transfers or prove UI input behavior in a running world. In-game validation still required.
