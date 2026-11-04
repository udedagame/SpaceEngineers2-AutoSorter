using System.Reflection;
using System.Runtime.Loader;
using System.Collections;
using System.Runtime.CompilerServices;
string pluginDir = Path.GetFullPath(args[0]), gameDir = args[1];
AssemblyLoadContext.Default.Resolving += (_, name) => {
    foreach(var dir in new[]{pluginDir,gameDir}) { var path=Path.Combine(dir,name.Name+".dll"); if(File.Exists(path)) return AssemblyLoadContext.Default.LoadFromAssemblyPath(path); }
    return null;
};
var plugin=AssemblyLoadContext.Default.LoadFromAssemblyPath(Path.Combine(pluginDir,"AutoSorter.dll"));
var sim=AssemblyLoadContext.Default.LoadFromAssemblyPath(Path.Combine(gameDir,"Game2.Simulation.dll"));
var itemType=sim.GetType("Keen.Game2.Simulation.WorldObjects.Items.ItemDefinition")!;
var configType=sim.GetType("Keen.Game2.Simulation.WorldObjects.Characters.ToolsConfiguration")!;
object Item(Guid id,string category) { var item=Activator.CreateInstance(itemType)!; for(Type? t=itemType;t!=null;t=t.BaseType) { var f=t.GetFields(BindingFlags.Instance|BindingFlags.NonPublic|BindingFlags.Public|BindingFlags.DeclaredOnly).FirstOrDefault(x=>x.FieldType==typeof(Guid)); if(f!=null) { f.SetValue(item,id); break; } } var p=itemType.GetProperty("Type")!; p.SetValue(item,Enum.Parse(p.PropertyType,category)); if(!Equals(itemType.GetProperty("Guid")!.GetValue(item),id)) throw new Exception("GUID fixture failed"); return item; }
object Config(object? tool, bool gun=false) {
    var config=Activator.CreateInstance(configType)!;
    foreach(var name in new[]{"ItemPrefabs","GunItems"}) {
        var p=configType.GetProperty(name)!; var types=p.PropertyType.GetGenericArguments();
        var dict=(IDictionary)Activator.CreateInstance(typeof(Dictionary<,>).MakeGenericType(types))!;
        if(tool!=null && ((name=="GunItems")==gun)) dict.Add(RuntimeHelpers.GetUninitializedObject(types[0]),tool);
        p.SetValue(config,Activator.CreateInstance(p.PropertyType,new object[]{dict}));
    }
    return config;
}
var method=plugin.GetType("AutoSorter.DepositProtection")!.GetMethod("GetProtectedItems",BindingFlags.NonPublic|BindingFlags.Static)!;
int count=0;
void Check(object item,object config,bool expected,string name) { var result=((HashSet<Guid>)method.Invoke(null,new[]{config})!).Contains((Guid)itemType.GetProperty("Guid")!.GetValue(item)!); if(result!=expected) throw new Exception(name); count++; }
var modIds=new[]{"70cb80c8-9745-5fbb-9c63-a3012dc30bf1","733c73b4-08f5-53dc-b1ab-8254d0f70bcc","52886e12-4740-5323-8f26-5c4251c9532f"};
foreach(var id in modIds) foreach(var category in new[]{"Component","Material","Ore","PresetAll"}) {
    var tool=Item(Guid.Parse(id),category);
    Check(tool,Config(tool),true,"Registered AreaTools item must be kept, regardless of flags");
    Check(tool,Config(tool,true),true,"Registered gun must be kept");
}
foreach(var category in new[]{"Component","Material","Ore"}) {var item=Item(Guid.NewGuid(),category); Check(item,Config(null),false,"Ordinary build input still deposited");}
var ordinaryTool=Item(Guid.NewGuid(),"Item"); Check(ordinaryTool,Config(null),false,"Ordinary tool stays");
Console.WriteLine($"PASS: {count} assertions against the actual AutoSorter protected-tool registry and game types. No world loaded.");



