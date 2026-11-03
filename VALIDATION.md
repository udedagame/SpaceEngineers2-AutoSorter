# Validation — AutoSorter 0.2.1

Content-based assignment replaces category name tags. Empty or mixed inventories are general sources; inventories containing exclusively ore, materials or components are destinations for that category. No persistent assignment. Optional NoSort exclusion remains.

Regression tests cover assignment from contents, repeated same-category stacks, empty/reset/reseed, mixtures and tools, ignoring old category tags, exclusions, no self-transfer, blocked paths, destination stability under transfers, no oscillation, and round-robin coverage.

Release build and isolated load check use the installed Space Engineers 2 DLLs. The isolated check installs and disposes both MonoMod hooks; it does not load a world.

Live game validation is still pending: discovery, transfers and quantity conservation, full targets, conveyor filters, pause/reload and coexistence with BuildPlanner. See README-RU.md for the test sequence. No game files or saves were changed.

Final packaging succeeded with 115 assertions and 0 errors. NuGet emitted NU1900: vulnerability data could not be fetched; the online dependency audit was not completed. Isolated hook loading passed.


0.2.1 adds tool-registry exclusion in AutoSorter. The separate BuildPlanner Alt+N patch passed 28 actual-filter checks; these are not AutoSorter gameplay tests. See CHANGELOG.md.
