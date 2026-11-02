using AutoSorter;
int passed = 0;
void Check(bool value, string message) { if (!value) throw new Exception(message); passed++; }
ContainerRule Rule(params Category[] contents) => ContainerRule.FromContents(contents);
var ore = Rule(Category.Ore);
var ingots = Rule(Category.Ingots, Category.Ingots);
var components = Rule(Category.Components);
var mixed = Rule(Category.Ore, Category.Components);
var plain = Rule();
var excluded = ContainerRule.FromContents(new[] { Category.Ore }, "[NoSort]");
Check(ore.Keeps(Category.Ore), "Ore sample");
Check(ingots.Keeps(Category.Ingots), "Multiple ingot types");
Check(components.Keeps(Category.Components), "Component sample");
Check(mixed.Accepts == Category.None, "Mixed is general");
Check(plain.Accepts == Category.None, "Empty resets");
Check(Rule(Category.None).Accepts == Category.None, "Tools are general");
Check(Rule(Category.Ore, Category.None).Accepts == Category.None, "Ore plus tools is general");
Check(ContainerRule.FromContents(new[] { Category.Ore }, "[Ingots]").Keeps(Category.Ore), "Ignore old category tags");
Check(ContainerRule.FromContents(Array.Empty<Category>(), "[Ore]").Accepts == Category.None, "Old tag cannot assign empty cargo");
Check(!excluded.Keeps(Category.Ore), "Exclusion wins");
Check(ContainerRule.FromContents(new[] { Category.Ore }, "[НЕ СОРТИРОВАТЬ]").Excluded, "Russian exclusion");
Check(Routing.ShouldMove(mixed, ore, Category.Ore, false, true), "General supplies specialized cargo");
Check(!Routing.ShouldMove(mixed, ore, Category.Ore, false, false), "Blocked route");
Check(!Routing.ShouldMove(mixed, ore, Category.Ore, true, true), "Self transfer");
Check(!Routing.ShouldMove(excluded, ore, Category.Ore, false, true), "Excluded source");
Check(!Routing.ShouldMove(mixed, excluded, Category.Ore, false, true), "Excluded destination");
Check(!Routing.ShouldMove(ore, ore, Category.Ore, false, true), "No balancing");
Check(!Routing.ShouldMove(mixed, plain, Category.Ore, false, true), "Empty cargo never auto-assigned");
Check(!Routing.ShouldMove(mixed, mixed, Category.Ore, false, true), "No general destination");
Check(!Routing.ShouldMove(mixed, components, Category.None, false, true), "Tools stay");
foreach (var category in new[] { Category.Ore, Category.Ingots, Category.Components }) {
    Check(Rule(category).Keeps(category), "Seeding assigns");
    Check(Rule().Accepts == Category.None, "Removing last item resets");
    Check(Rule(Category.Ore, Category.Ingots, Category.Components).Accepts == Category.None, "Dumping a mixture is general");
}
var rules = new[] { plain, ore, ingots, components, mixed, excluded };
foreach (var from in rules)
foreach (var to in rules)
foreach (var kind in new[] { Category.Ore, Category.Ingots, Category.Components, Category.None })
    if (Routing.ShouldMove(from, to, kind, false, true)) {
        Check(Rule(to.Accepts, kind).Keeps(kind), "Transfer preserves destination category");
        foreach (var next in rules) Check(!Routing.ShouldMove(to, next, kind, false, true), "No oscillation");
    }
var visited = new HashSet<int>();
int cursor = 0;
for (int i = 0; i < 101; i++) { visited.Add(cursor); cursor = Routing.Next(cursor, 101); }
Check(visited.Count == 101 && cursor == 0, "Round robin coverage");
Check(Routing.Next(9, 0) == 0, "Empty list");
Console.WriteLine($"PASS: {passed} assertions. Policy tests only; live transfers require an in-game test.");
