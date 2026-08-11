using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Reflection;

namespace rpg;

// 全事件清單
public static class EventDatabase
{
    // 用一個靜態變數儲存載入進來的所有事件
    public static List<EventNode> AllEvents { get; private set; } = new();

    public static void Init()
    {
        string resourceName = "rpg.Assets.Scripts.person.json";

        var assembly = Assembly.GetExecutingAssembly();
        using Stream? stream = assembly.GetManifestResourceStream(resourceName);

        if (stream == null)
        {
            Console.WriteLine($"[錯誤] 找不到嵌入資源：{resourceName}");
            // 💡 偵錯小幫手：若找不到名稱，可以印出所有已嵌入的資源名稱來比對
            // foreach (var name in assembly.GetManifestResourceNames()) Console.WriteLine(name);
            return;
        }

        using StreamReader reader = new StreamReader(stream);
        string jsonContent = reader.ReadToEnd();

        // 記得加上 JsonStringEnumConverter 避免 Enum 轉換失敗！
        var options = new JsonSerializerOptions
        {
            Converters = { new JsonStringEnumConverter() }
        };

        AllEvents = JsonSerializer.Deserialize<List<EventNode>>(jsonContent, options) ?? new List<EventNode>();
    }
}