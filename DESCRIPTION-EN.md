# AutoSorter — Automatic Cargo Sorting for Space Engineers 2

**Drop in a sample. Let AutoSorter find the matching cargo.**

AutoSorter sorts ore, ingots/materials, and components between your connected cargo containers without requiring category tags or renamed blocks.

- Put ore in a container to make it an ore destination.
- Put an ingot/material in another to make it a material destination.
- Put a component in a third to make it a component destination.
- Mixed containers supply those destinations. Empty containers wait for a sample.

Container roles follow their current contents. Empty a container to reset it, or add a different category to make it general storage. If a mixed container is left with only one category after sorting, it becomes a destination for that category on the next pass.

AutoSorter checks conveyor connections, direction, and item filters. Correctly stored items stay in place. An optional `[NoSort]` tag excludes a container completely. Character and production inventories are not sorted.

**Standalone:** BuildPlanner is not required. Classification uses internal item types and does not depend on your interface language. English installation instructions are included.

**Distribution:** this is a manually installed plugin loaded through the game's `-plugins:` launch option, not a Steam Workshop mod. Version 0.2.0 targets local worlds and vanilla cargo containers owned by the local player. Remote and dedicated servers are not supported.

**Experimental release:** build, policy tests, and isolated hook loading passed. In-game transfers have not yet been validated. Start with a test world or a copy of your save. Game updates may require plugin updates.

See `README.md` for setup, settings, and the first-run checklist.
