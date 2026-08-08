using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Spectre.Console;

namespace rpg;

public static class StorySystem
{
    // 讀取 JSON 並播放劇情
    public static async Task PlayStoryAsync(string jsonFileName)
    {
        string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "Scripts", jsonFileName);

        if (!File.Exists(filePath))
        {
            AnsiConsole.MarkupLine($"[red]錯誤：找不到劇情檔案 {jsonFileName}[/]");
            return;
        }

        // 1. 讀取並反序列化 JSON
        string jsonContent = File.ReadAllText(filePath);
        var dialogues = JsonSerializer.Deserialize<List<DialogueNode>>(jsonContent);

        if (dialogues == null) return;

        // 2. 逐句播放
        foreach (var node in dialogues)
        {
            AnsiConsole.Clear();
            
            // 繪製頂部對話外框
            var panel = new Panel(await TypewriterTextAsync(node.Text, speedMs: 30))
            {
                Header = new PanelHeader($"[bold {node.Color}] {node.Speaker} [/]"),
                Border = BoxBorder.Rounded,
                BorderStyle = new Style(Color.Cyan1),
                Width = 80,
                Height = 8
            };

            AnsiConsole.Write(new Align(panel, HorizontalAlignment.Left));
            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine("[grey] (按下 [/][bold yellow]Space / Enter[/][grey] 繼續...) [/]".PadLeft(45));

            // 3. 等待玩家按下 空白鍵 或 Enter 鍵
            WaitForKey();
        }

        AnsiConsole.Clear();
    }

    // 💡 逐字打字機效果 (Typewriter Effect)
    private static async Task<string> TypewriterTextAsync(string fullText, int speedMs)
    {
        // 簡單示範：如果你想有即時打字感，可以邊印邊 Delay；
        // 這裡直接模擬回傳或加上 Task.Delay 呈現沉浸感
        await Task.Delay(100); 
        return fullText;
    }

    // 💡 監聽按鍵：只接受 空白鍵 (Space) 或 Enter
    private static void WaitForKey()
    {
        while (true)
        {
            var key = Console.ReadKey(intercept: true).Key; // intercept: true 不會在畫面上印出按鍵字元
            if (key == ConsoleKey.Spacebar || key == ConsoleKey.Enter)
            {
                break;
            }
        }
    }
}