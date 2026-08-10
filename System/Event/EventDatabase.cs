using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace rpg;

// 全事件清單
public static class EventDatabase
{
    // 用一個靜態變數儲存載入進來的所有事件
    public static List<EventNode> AllEvents { get; private set; } = new();

    public static void Init()
    {
        string jsonFileName = "person.json";
        string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "Scripts", jsonFileName);

        if (!File.Exists(filePath))
        {
            Console.WriteLine($"[錯誤] 找不到事件資料庫檔案：{filePath}");
            Console.ReadKey(true);
            return;
        }

        string jsonContent = File.ReadAllText(filePath);

        // 記得加上 JsonStringEnumConverter 避免 Enum 轉換失敗！
        var options = new JsonSerializerOptions
        {
            Converters = { new JsonStringEnumConverter() }
        };

        AllEvents = JsonSerializer.Deserialize<List<EventNode>>(jsonContent, options) ?? new List<EventNode>();
    }
}