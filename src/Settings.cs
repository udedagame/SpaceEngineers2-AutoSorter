using System.Text.Json;

namespace AutoSorter;

public sealed class Settings
{
    public bool Enabled { get; set; } = true;
    public double IntervalSeconds { get; set; } = 2;
    public int MaxTransferAttempts { get; set; } = 64;
    public int MaxItemChecks { get; set; } = 256;
    public int SoftTimeBudgetMs { get; set; } = 10;
    public bool DryRun { get; set; }

    public void Validate()
    {
        if (!double.IsFinite(IntervalSeconds) || IntervalSeconds < 0.5 || IntervalSeconds > 60)
            throw new InvalidDataException("IntervalSeconds must be 0.5..60");
        if (MaxTransferAttempts < 1 || MaxTransferAttempts > 1024 || MaxItemChecks < 1 || MaxItemChecks > 4096
            || SoftTimeBudgetMs < 1 || SoftTimeBudgetMs > 100)
            throw new InvalidDataException("Invalid work budget in AutoSorter.json");
    }

    public static Settings Load()
    {
        var path = Path.Combine(Log.DirectoryPath, "AutoSorter.json");
        Settings settings;
        if (File.Exists(path))
            settings = JsonSerializer.Deserialize<Settings>(File.ReadAllText(path), new JsonSerializerOptions
            { PropertyNameCaseInsensitive = true, UnmappedMemberHandling = System.Text.Json.Serialization.JsonUnmappedMemberHandling.Disallow })
                ?? throw new InvalidDataException("Empty settings");
        else
        {
            settings = new Settings();
            File.WriteAllText(path, JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true }));
        }
        settings.Validate();
        return settings;
    }
}

internal static class Log
{
    internal static readonly string DirectoryPath = Path.GetDirectoryName(typeof(Log).Assembly.Location)!;
    private static readonly object Gate = new();
    internal static void Write(string message)
    {
        // Diagnostics must never take down the game (e.g. a read-only plugin directory).
        try
        {
            lock (Gate)
            {
                string path = Path.Combine(DirectoryPath, "AutoSorter.log");
                if (File.Exists(path) && new FileInfo(path).Length > 2 * 1024 * 1024)
                    File.Move(path, path + ".1", true);
                File.AppendAllText(path, $"{DateTimeOffset.Now:O} {message}{Environment.NewLine}");
            }
        }
        catch { }
    }
}
