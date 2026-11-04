using System.Reflection;
using Keen.Game2.Game.SessionComponents;
using Keen.Game2.Simulation.WorldObjects.Characters;
using Keen.Game2.Simulation.WorldObjects.Items;
using Keen.VRage.DCS.Components;
using Keen.VRage.Library.Definitions;
using Keen.VRage.Library.Mathematics;
using Keen.VRage.Library.Utils;
using Keen.VRage.Multiplayer.Replications.FailureStrategies;
using MonoMod.RuntimeDetour;

namespace AutoSorter;

// Restrict the guard to the synchronous vanilla bulk-deposit call. Manual inventory
// transfers and other plugins retain their normal behavior outside this scope.
internal static class DepositProtection
{
    private static Hook? _bulkHook, _transferHook;
    [ThreadStatic] private static HashSet<Guid>? _protected;
    private delegate void BulkOriginal(object self, SkipOnReplicationFail<Entity> from, SkipOnReplicationFail<Entity> to);
    private delegate FixedPoint TransferOriginal(InventoryComponent self, InventoryComponent to, int index, int? targetIndex, FixedPoint? amount, bool partial);

    internal static HashSet<Guid> GetProtectedItems(ToolsConfiguration tools)
    {
        var result = new HashSet<Guid>();
        foreach (var item in tools.ItemPrefabs.Values) result.Add(item.Guid);
        foreach (var item in tools.GunItems.Values) result.Add(item.Guid);
        return result;
    }

    internal static void Install(Func<bool> enabled)
    {
        var server = typeof(WorldSessionComponent).Assembly.GetType("Keen.Game2.Game.SessionComponents.InProcessGameServerSessionComponent", true)!;
        var bulk = server.GetMethod("TryDepositAllNonEssentialItemsImpl", BindingFlags.NonPublic | BindingFlags.Instance,
            null, new[] { typeof(SkipOnReplicationFail<Entity>), typeof(SkipOnReplicationFail<Entity>) }, null)
            ?? throw new MissingMethodException(server.FullName, "TryDepositAllNonEssentialItemsImpl");
        var transfer = typeof(InventoryComponent).GetMethod(nameof(InventoryComponent.TransferByIdx),
            new[] { typeof(InventoryComponent), typeof(int), typeof(int?), typeof(FixedPoint?), typeof(bool) })!;
        try
        {
            _transferHook = new Hook(transfer, (TransferOriginal orig, InventoryComponent self, InventoryComponent to,
                int index, int? targetIndex, FixedPoint? amount, bool partial) =>
            {
                if (_protected?.Contains(self.GetItemDefinition(index).Guid) == true) return (FixedPoint)0;
                return orig(self, to, index, targetIndex, amount, partial);
            });
            _bulkHook = new Hook(bulk, (BulkOriginal orig, object self, SkipOnReplicationFail<Entity> from, SkipOnReplicationFail<Entity> to) =>
            {
                if (!enabled()) { orig(self, from, to); return; }
                HashSet<Guid> protectedItems;
                try
                {
                    protectedItems = GetProtectedItems(Singleton<DefinitionManager>.Instance.GetConfiguration<ToolsConfiguration>());
                }
                catch (Exception e)
                {
                    // Do not deposit unfiltered if tool discovery failed.
                    Log.Write("Quick deposit cancelled: cannot identify protected tools: " + e);
                    return;
                }
                RunProtected(protectedItems, () => orig(self, from, to));
            });
            Log.Write("Vanilla quick-deposit tool protection installed.");
        }
        catch { Dispose(); throw; }
    }

    internal static void RunProtected(HashSet<Guid> items, Action deposit)
    {
        var previous = _protected;
        try { _protected = items; deposit(); }
        finally { _protected = previous; }
    }

    internal static void Dispose()
    {
        _bulkHook?.Dispose(); _bulkHook = null;
        _transferHook?.Dispose(); _transferHook = null;
    }
}
