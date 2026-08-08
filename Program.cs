using System.Runtime.Versioning;
using Spectre.Console;

namespace rpg;

class Program
{
    public record MenuItem(int Id, string DisplayName);
    private static bool isShowing = false;
    
    [SupportedOSPlatform("windows")]
    static void Main()
    {
        Console.SetWindowSize(100, 30);
        int windowWidth = Console.WindowWidth;

        Goblin goblin = new Goblin(5);

        // 主選單迴圈
        while (true)
        {
            AnsiConsole.Clear();
            AnsiConsole.WriteLine();

            // 印出標題
            AnsiConsole.MarkupLine($"[bold red]{GameArt.TitleArt}[/]");
            AnsiConsole.WriteLine();

            // 主選單
            var mainMenuChoice = AnsiConsole.Prompt(
                new SelectionPrompt<MenuItem>()
                    .PageSize(3)
                    .UseConverter(item => CenterText(item.DisplayName, windowWidth))
                    .AddChoices(new[] {
                        new MenuItem(1, "開始遊戲"),
                        new MenuItem(2, "退出")
                    }));

            if (mainMenuChoice.Id == 2)
            {
                AnsiConsole.Clear();
                break; // 結束程式
            }

            // 2. 選擇「開始遊戲」後進入次選單迴圈
            GameLoop(windowWidth);
        }
    }

    // 遊戲主要選單 (雙方狀態 / 進入戰鬥)
    private static void GameLoop(int windowWidth)
    {
        // 玩家選擇職業、個性、屬性
        AnsiConsole.Clear();
        StorySystem.PlayStoryAsync("intro.json").Wait();

        Player player = PlayerStart.SelectedPlayer();

        Monster monster = new Goblin(5);

        while (true)
        {
            AnsiConsole.Clear();
            AnsiConsole.MarkupLine($"[bold yellow]{GameArt.Day1Art}[/]");
            AnsiConsole.MarkupLine($"{GameArt.TownArt}");
            AnsiConsole.WriteLine();

            var gameMenuChoice = AnsiConsole.Prompt(
                new SelectionPrompt<MenuItem>()
                    .PageSize(5)
                    .UseConverter(item => CenterText(item.DisplayName, windowWidth))
                    .AddChoices(new[] {
                        new MenuItem(1, "檢視雙方狀態"),
                        new MenuItem(2, "進入戰鬥"),
                        new MenuItem(3, "返回主選單")
                    }));

            switch (gameMenuChoice.Id)
            {
                case 1:
                    // 3. 執行雙方狀態並可按任一鍵返回
                    ShowBothStatus(player, monster);
                    break;
                case 2:
                    // 4. 進入戰鬥程序
                    StartBattle(player, monster);
                    
                    monster = new Goblin(player.Level > 5 ? player.Level : 5);
                    break;
                case 3:
                    return; // 回到主選單
            }
        }
    }

    // 3. 呈現敵我兩方資訊
    public static void ShowBothStatus(Player player, Monster monster)
    {
        ShowStatusInfo showStatusInfo = new ShowStatusInfo(player, monster, isShowing);
        showStatusInfo.ShowInfo();

        // 提示按任一鍵回上一頁
        AnsiConsole.MarkupLine("[grey]按下 [/][bold yellow]任意鍵[/][grey] 返回上一頁...[/]");
        Console.ReadKey(true);
    }

    // 4. 進入戰鬥程序
    public static void StartBattle(Player player, Monster monster)
    {
        // 建立戰鬥系統實例，並啟動戰鬥
        BattleSystem battle = new BattleSystem(player, monster);
        bool isVictory = battle.StartBattle();

        if (!isVictory)
        {
            player.CurrentHP = player.MaxHP;
            player.CurrentMP = player.MaxMP;
            AnsiConsole.MarkupLine("[yellow]血條重置...[/]");
            Console.ReadKey(true);
        }
    }

    // 計算字串在 Terminal 上的「顯示寬度」（中文算 2 寬度）
    private static int GetDisplayWidth(string text)
    {
        int width = 0;
        foreach (char c in text)
        {
            // Unicode 範圍判斷：CJK 常用漢字與全形符號佔 2 個寬度
            if (c >= 0x4e00 && c <= 0x9fff || c >= 0xff00 && c <= 0xffef)
                width += 2;
            else
                width += 1;
        }
        return width;
    }

    // 置中選單 Helper
    private static string CenterText(string text, int width)
    {
        int displayWidth = GetDisplayWidth(text);
        int padding = Math.Max(0, (width - displayWidth) / 2);
        return text.PadLeft(padding + text.Length);
    }
}