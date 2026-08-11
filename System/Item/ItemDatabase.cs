using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Reflection;

namespace rpg;

// 全道具清單
public static class ItemDatabase
{
    // 用一個靜態變數儲存載入進來的所有道具
    public static List<ItemNode> AllItems { get; private set; } = new();

    public static void Init()
    {
        string resourceName = "rpg.Assets.Scripts.item.json";

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

        // 讀取並填入 AllItems 變數中
        AllItems = JsonSerializer.Deserialize<List<ItemNode>>(jsonContent, options) ?? new List<ItemNode>();
    }
}