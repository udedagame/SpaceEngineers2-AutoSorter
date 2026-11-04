using System.Reflection;
using System.Runtime.Loader;

string pluginDir = Path.GetFullPath(args[0]);
string gameDir = args[1];
AssemblyLoadContext.Default.Resolving += (_, name) =>
{
    foreach (string dir in new[] { pluginDir, gameDir })
    {
        string path = Path.Combine(dir, name.Name + ".dll");
        if (File.Exists(path)) return AssemblyLoadContext.Default.LoadFromAssemblyPath(path);
    }
    return null;
};
var assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(Path.Combine(pluginDir, "AutoSorter.dll"));
var type = assembly.GetType("AutoSorter.AutoSorterPlugin", true)!;
var instance = (IDisposable)Activator.CreateInstance(type)!;
foreach (string fieldName in new[] { "_inputHook", "_updateHook" })
{
    var hook = type.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Static)!.GetValue(null);
    if (hook == null) throw new Exception($"Hook missing: {fieldName}; inspect AutoSorter.log");
    Console.WriteLine($"PASS: installed {fieldName} against actual game DLLs");
}
var guard = assembly.GetType("AutoSorter.DepositProtection", true)!;
foreach (string fieldName in new[] { "_bulkHook", "_transferHook" }) {
 if(guard.GetField(fieldName, BindingFlags.Static | BindingFlags.NonPublic)!.GetValue(null) == null) throw new Exception("Missing " + fieldName);
 Console.WriteLine("PASS: installed " + fieldName + " against actual game DLLs");
}
var scope = guard.GetMethod("RunProtected", BindingFlags.NonPublic | BindingFlags.Static)!;
var field = guard.GetField("_protected", BindingFlags.NonPublic | BindingFlags.Static)!;
var outer = new HashSet<Guid> { Guid.NewGuid() };
var inner = new HashSet<Guid> { Guid.NewGuid() };
void Assert(bool value) { if(!value) throw new Exception("Guard scope assertion failed"); }
Action nested = () => { Assert(ReferenceEquals(field.GetValue(null),inner)); throw new InvalidOperationException("fixture"); };
Action body = () => {
 Assert(ReferenceEquals(field.GetValue(null),outer));
 try { scope.Invoke(null, new object[]{inner,nested}); } catch(TargetInvocationException e) when(e.InnerException is InvalidOperationException) {}
 Assert(ReferenceEquals(field.GetValue(null),outer));
 Task.Run(()=>Assert(field.GetValue(null)==null)).GetAwaiter().GetResult();
};
Assert(field.GetValue(null)==null);
scope.Invoke(null,new object[]{outer,body});
Assert(field.GetValue(null)==null);
Console.WriteLine("PASS: nested, exceptional and cross-thread guard scope isolation");
instance.Dispose();
foreach(string f in new[]{"_bulkHook","_transferHook"}) Assert(guard.GetField(f,BindingFlags.NonPublic|BindingFlags.Static)!.GetValue(null)==null);
Console.WriteLine("PASS: disposed plugin and all four hooks. No world loaded.");
