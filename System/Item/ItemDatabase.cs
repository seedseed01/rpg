using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace rpg;

// 全道具清單
public static class ItemDatabase
{
    // 用一個靜態變數儲存載入進來的所有道具
    public static List<ItemNode> AllItems { get; private set; } = new();

    public static void Init()
    {
        string jsonFileName = "item.json";
        string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "Scripts", jsonFileName);

        if (!File.Exists(filePath))
        {
            Console.WriteLine($"[錯誤] 找不到道具資料庫檔案：{filePath}");
            return;
        }

        string jsonContent = File.ReadAllText(filePath);

        // 記得加上 JsonStringEnumConverter 避免 Enum 轉換失敗！
        var options = new JsonSerializerOptions
        {
            Converters = { new JsonStringEnumConverter() }
        };

        // 讀取並填入 AllItems 變數中
        AllItems = JsonSerializer.Deserialize<List<ItemNode>>(jsonContent, options) ?? new List<ItemNode>();
    }
}