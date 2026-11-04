using Keen.Game2.Simulation.WorldObjects.Items;
using Keen.VRage.Library.Mathematics;

namespace AutoSorter;

internal static class InventoryTransfer
{
    // An explicit target slot prevents the game's auto-merge path from combining
    // stacks with different metadata. Check slot capacity BEFORE removing items.
    internal static bool TryTarget(ReadOnlySpan<ItemStack> items, int? maxStacks, ItemStack stack, out int target)
    {
        if (stack.Definition.IsStackable)
            for (int i = 0; i < items.Length; i++)
                if (items[i].Definition == stack.Definition &&
                    (items[i].MetaData?.Equals(stack.MetaData) ?? stack.MetaData == null))
                { target = i; return true; }
        target = items.Length;
        return !maxStacks.HasValue || items.Length < maxStacks.Value;
    }

    internal static FixedPoint MoveStack(InventoryComponent source, InventoryComponent destination, int index)
    {
        if (ReferenceEquals(source, destination)) return 0;
        var stack = source.Items[index];
        if (!TryTarget(destination.Items, destination.Definition.MaxStacks, stack, out int target)) return 0;
        var beforeSource = source.CountItem(stack.Definition);
        var beforeDestination = destination.CountItem(stack.Definition);
        // Never use TransferByDef here: mixed battery metadata can make that API
        // remove fewer items than it inserts. Each non-stackable item moves alone.
        var requested = stack.Definition.IsStackable ? stack.Amount : (FixedPoint)1;
        var moved = source.TransferByIdx(destination, index, target, requested, true);
        if (beforeSource - source.CountItem(stack.Definition) != moved ||
            destination.CountItem(stack.Definition) - beforeDestination != moved)
            throw new InvalidOperationException("Inventory conservation check failed; sorting stopped. Item=" + stack.Definition.Guid);
        return moved;
    }
}
