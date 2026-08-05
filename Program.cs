using System.Runtime.Versioning;
using Spectre.Console;

namespace rpg;

class Program
{
    public record MenuItem(int Id, string DisplayName);

    [SupportedOSPlatform("windows")]
    static void Main()
    {
        Console.SetWindowSize(120, 30);
        int windowWidth = Console.WindowWidth;        

        // 初始化玩家與怪物實例（作為遊戲狀態）
        Player player = new Player(
            name: "AAAAAA",
            hp: 100,
            mp: 100,
            attack: 20,
            defense: 20,
            magicAttack: 10,
            magicDefense: 10,
            speed: 10,
            level: 1,
            status: CurrentStatus.Normal,
            type: CurrentType.Earth,
            nature: Personality.Focused);

        Monster goblin = new Goblin(5);

        // 主選單迴圈
        while (true)
        {
            AnsiConsole.Clear();
            AnsiConsole.WriteLine();

            // 1. 印出標題
            AnsiConsole.Write(
                new FigletText("RPG GAME")
                    .Centered()
                    .Color(Color.Red));

            AnsiConsole.WriteLine();

            // 主選單
            var mainMenuChoice = AnsiConsole.Prompt(
                new SelectionPrompt<MenuItem>()
                    .PageSize(5)
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
            GameLoop(player, goblin, windowWidth);
        }
    }

    // 遊戲主要選單 (雙方狀態 / 進入戰鬥)
    private static void GameLoop(Player player, Monster monster, int windowWidth)
    {
        while (true)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule("[bold cyan]冒險者大廳[/]").Centered());
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
                    monster = new Goblin(player.Level);
                    break;
                case 3:
                    return; // 回到主選單
            }
        }
    }

    // 3. 呈現敵我兩方資訊
    public static void ShowBothStatus(Player player, Monster monster)
    {
        ShowStatusInfo showStatusInfo = new ShowStatusInfo(player, monster);
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
            // 如果玩家戰敗，血量重置/復活，方便繼續測試
            player.CurrentHP = player.MaxHP;
            player.CurrentMP = player.MaxMP;
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