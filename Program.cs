using Newtonsoft.Json;
using System.Text.Json;

const string OutputDir = "migrated";
string[] files = Directory.GetFiles(".");
Directory.CreateDirectory(OutputDir);

JsonSerializerSettings settings = new()
{
    TypeNameHandling = TypeNameHandling.All
};
JsonSerializerOptions options = new()
{
    WriteIndented = true,
};

foreach (var file in files)
{
    if (file.EndsWith(".json") )
    {
        await MigrateFile(file);
        Console.WriteLine($"{file} migrated.");
    }
}

Console.WriteLine("\nDone.");
Console.ReadKey(true);

async Task MigrateFile(string file)
{
    string content = await File.ReadAllTextAsync(file);
    Dictionary<string, object?>? dict = JsonConvert.DeserializeObject<Dictionary<string, object?>>(content, settings)
        ?? throw new Exception($"Failed to convert file [{file}]");
    content = System.Text.Json.JsonSerializer.Serialize(dict, options);
    await File.WriteAllTextAsync(Path.Combine(OutputDir, file), content);
}
