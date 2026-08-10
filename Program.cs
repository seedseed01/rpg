using System.Runtime.Versioning;
using Spectre.Console;

namespace rpg;

class Program
{
    public record MenuItem(int Id, string DisplayName);
    public static bool isShowing = false;
    public static int actionPoint = 5;
    public static int dayCount = 1;
    public static Inventory Inventory = new();
    public static Player player = null!;
    public static int gold = 100;
    
    [SupportedOSPlatform("windows")]
    static void Main()
    {
        Console.SetWindowSize(100, 30);
        int windowWidth = Console.WindowWidth;
        ItemDatabase.Init();
        EventDatabase.Init();

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

    // 遊戲主要選單
    private static void GameLoop(int windowWidth)
    {
        // 玩家選擇職業、個性、屬性
        AnsiConsole.Clear();
        StorySystem.PlayStoryAsync("intro.json").Wait();

        player = PlayerStart.SelectedPlayer();
        Monster monster = new Goblin(5);

        while (dayCount < 6)
        {
            AnsiConsole.Clear();
            AnsiConsole.MarkupLine($"[bold yellow]{GameArt.Day1Art}[/]");
            AnsiConsole.MarkupLine($"{GameArt.TownArt}");
            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine($"[bold #00FF00]行動點數剩餘: {actionPoint}[/]");
            AnsiConsole.MarkupLine($"[bold #FFD700]擁有錢幣: {gold}[/]");
            AnsiConsole.WriteLine();
            var gameMenuChoice = AnsiConsole.Prompt(
                new SelectionPrompt<MenuItem>()
                    .PageSize(6)
                    .UseConverter(item => CenterText(item.DisplayName, windowWidth))
                    .AddChoices(new[] {
                        new MenuItem(1, "檢視雙方狀態"),
                        new MenuItem(2, "鎮上閒晃"),
                        new MenuItem(3, "奇貨商人"),
                        new MenuItem(4, "使用道具"),
                        new MenuItem(5, "進入戰鬥"),
                        new MenuItem(6, "返回主選單")
                    }));

            switch (gameMenuChoice.Id)
            {
                case 1:
                    ShowBothStatus(player, monster);
                    break;
                case 2:
                    TownWalk();
                    break;
                case 3:
                    Trader();
                    break;
                case 4:
                    UseItem(player, monster);
                    break;
                case 5:
                    StartBattle(player, monster);                    
                    monster = new Goblin(player.Level > 5 ? player.Level : 5);
                    break;
                case 6:
                    // 離開後初始化數據
                    actionPoint = 5;
                    Inventory = new();
                    isShowing = false;
                    return;
            }
        }
    }

    public static void TownWalk()
    {
        if (actionPoint < 1)
        {
            AnsiConsole.MarkupLine("[yellow]已無行動力，請去面對魔物！[/]");
            Console.ReadKey(true);
            return;  
        }
        actionPoint--;
        TownWalk townWalk = new TownWalk();
        townWalk.Walk();
    }

    public static void Trader()
    {
        if (actionPoint < 1)
        {
            AnsiConsole.MarkupLine("[yellow]已無行動力，請去面對魔物！[/]");
            Console.ReadKey(true);
            return;  
        } 
        actionPoint--;
    }

    public static void UseItem(Player player, Monster? monster)
    {
        if (actionPoint < 1)
        {
            AnsiConsole.MarkupLine("[yellow]已無行動力，請去面對魔物！[/]");
            Console.ReadKey(true);
            return;  
        } 
  

        // 呼叫你的背包系統加入道具
       // 1. 呼叫背包選單，拿到玩家選的道具
        ItemNode? chosenItem = Inventory.ShowAndUseItemMenu();

        // 2. 如果玩家有選道具（沒有選取消）
        if (chosenItem != null)
        {
            // 套用道具效果（扣血/加能力等）
            Inventory.ApplyItemEffect(chosenItem, player);

            // 背包扣除 1 個該道具
            Inventory.UseItem(chosenItem.ItemNo);
            actionPoint--;
        }
    }

    public static void ShowBothStatus(Player player, Monster monster)
    {
        ShowStatusInfo showStatusInfo = new ShowStatusInfo(player, monster, isShowing);
        showStatusInfo.ShowInfo();

        AnsiConsole.MarkupLine("[grey]按下 [/][bold yellow]任意鍵[/][grey] 返回上一頁...[/]");
        Console.ReadKey(true);
    }


    public static void StartBattle(Player player, Monster monster)
    {
        // 建立戰鬥系統實例，並啟動戰鬥
        BattleSystem battle = new BattleSystem(player, monster);
        bool isVictory = battle.StartBattle();

        // 如果是逃跑出場，就不進行回血，並增加行動力1點
        if (BattleSystem.isPlayerRunning)
        {            
            if (actionPoint < 5) actionPoint++;
            return;  
        } 
        
        player.CurrentHP = player.MaxHP;
        player.CurrentMP = player.MaxMP;
        AnsiConsole.MarkupLine("[yellow]村裡醫生幫你治療傷勢...[/]");
        Console.ReadKey(true);
        
        if (isVictory)
        {
            // 勝利恢復狀態
            player.Status = CurrentStatus.Normal;
            actionPoint = 5;
        }
        else
        {
            if (actionPoint < 5) actionPoint++;
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