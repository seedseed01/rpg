using System.Runtime.Versioning;
using Spectre.Console;

namespace rpg;

class Program
{
    public record MenuItem(int Id, string DisplayName);
    public static bool isShowing = false;
    public static int actionPoint = 8;
    public static int dayCount = 1;
    public static Inventory Inventory = new();
    public static Player player = null!;
    public static Monster monster = null!;
    public static int gold = 100;
    public static int runningCount = 0; // 逃跑次數
    public static int battleCount = 0;  // 戰鬥次數
    public static int loseCount = 0;    // 失敗次數
    
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
        StorySystem.PlayStoryAsync().Wait();

        player = PlayerStart.SelectedPlayer();
        monster = new Goblin(5);

        while (dayCount < 6)
        {
            string t = dayCount switch
            {
                1 => GameArt.Day1Art,
                2 => GameArt.Day2Art,
                3 => GameArt.Day3Art,
                4 => GameArt.Day4Art,
                5 => GameArt.Day5Art,
                _ => GameArt.Day1Art
            };

            AnsiConsole.Clear();            
            AnsiConsole.MarkupLine($"{GameArt.TownArt}");
            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine($"[bold yellow]{t}[/]");
            AnsiConsole.MarkupLine($"[bold #00FF00]行動點數剩餘: {actionPoint}[/]");
            AnsiConsole.WriteLine();
            var gameMenuChoice = AnsiConsole.Prompt(
                new SelectionPrompt<MenuItem>()
                    .PageSize(6)
                    .UseConverter(item => CenterText(item.DisplayName, windowWidth))
                    .AddChoices(new[] {
                        new MenuItem(1, "檢視雙方狀態"),
                        new MenuItem(2, "鎮上閒晃"),
                        // new MenuItem(3, "奇貨商人"),
                        new MenuItem(4, "使用道具"),
                        new MenuItem(5, "進入戰鬥"),
                        new MenuItem(6, "返回主選單")
                    }));

            switch (gameMenuChoice.Id)
            {
                case 1:
                    ShowBothStatus();
                    break;
                case 2:
                    TownWalk();
                    break;
                // case 3:
                //     Trader();
                //     break;
                case 4:
                    UseItem();
                    break;
                case 5:
                    StartBattle();
                    break;
                case 6:
                    var d = AnsiConsole.Prompt(
                        new SelectionPrompt<MenuItem>()
                            .Title("[cyan]確定返回主選單? 會導致所有紀錄重來[/]")
                            .UseConverter(item => item.DisplayName)
                            .PageSize(5)
                            .AddChoices(new[] {
                                new MenuItem(1, "否"),
                                new MenuItem(2, "是"),
                            }));

                    if (d.Id == 1){
                        break;
                    }
                    else
                    {
                        // 離開後初始化數據
                        actionPoint = 8;
                        Inventory = new();
                        isShowing = false;
                        return;
                    }
            }
        }

        AnsiConsole.Clear();
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("[bold #AFFF00]結局 [/]");
        string story = "因為你的努力，村莊幸免於難!\n\n大陸上幾乎九成的村莊覆滅，剩餘的人決定將這些勇敢挺身而出的英雄們集結在一起，向魔王討還失去的一切!\n";
        var panel = new Panel(story)
        {
            Border = BoxBorder.DoubleVertical,
            BorderStyle = new Style(Color.Cyan1),
            Width = 60
        };

        AnsiConsole.Write(new Align(panel, HorizontalAlignment.Left));
        Console.ReadKey(true);
        AnsiConsole.MarkupLine("[bold #AFFF00]統計此次遊戲數據: [/]");
        AnsiConsole.MarkupLine("--------------------------------------------------------------------------------");
        AnsiConsole.MarkupLine($"[bold #FFD787]戰鬥次數: {battleCount}[/]");
        AnsiConsole.MarkupLine($"[bold #FFD787]逃跑次數: {runningCount}[/]");
        AnsiConsole.MarkupLine($"[bold #FFD787]失敗次數: {loseCount}[/]");
        AnsiConsole.MarkupLine("--------------------------------------------------------------------------------");
        AnsiConsole.MarkupLine($"[bold #FF5F00]最終能力數據: [/]");
        AnsiConsole.MarkupLine($"[bold #FF5F00]名稱: {player.Name}[/]");
        AnsiConsole.MarkupLine($"[bold #FF5F00]狀態: {player.Status.ToChinese()}[/]");
        AnsiConsole.MarkupLine($"[bold #FF5F00]屬性: {player.Type.ToChinese()}[/]");
        AnsiConsole.MarkupLine($"[bold #FF5F00]性格: {player.Nature.ToChinese()}[/]");
        AnsiConsole.MarkupLine($"[bold #FF5F00]等級: {player.Level}[/]");
        AnsiConsole.MarkupLine($"[bold #FF5F00]HP: {player.MaxHP}[/]");
        AnsiConsole.MarkupLine($"[bold #FF5F00]MP: {player.MaxMP}[/]");
        AnsiConsole.MarkupLine($"[bold #FF5F00]攻擊力: {player.Attack}[/]");
        AnsiConsole.MarkupLine($"[bold #FF5F00]防禦力: {player.Defense}[/]");
        AnsiConsole.MarkupLine($"[bold #FF5F00]魔法攻擊力: {player.MagicAttack}[/]");
        AnsiConsole.MarkupLine($"[bold #FF5F00]魔法防禦力: {player.MagicDefense}[/]");
        AnsiConsole.MarkupLine($"[bold #FF5F00]速度: {player.Speed}[/]");
        Console.ReadKey(true);
    }

    public static void TownWalk()
    {
        if (actionPoint < 1)
        {
            AnsiConsole.MarkupLine("[yellow]已無行動力，請去面對魔物！[/]");
            AnsiConsole.MarkupLine("[yellow]進入戰鬥後，會回復點數！[/]");
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
            AnsiConsole.MarkupLine("[yellow]進入戰鬥後，會回復點數！[/]");
            Console.ReadKey(true);
            return;  
        } 
        actionPoint--;
    }

    public static void UseItem()
    {
        if (actionPoint < 1)
        {
            AnsiConsole.MarkupLine("[yellow]已無行動力，請去面對魔物！[/]");
            AnsiConsole.MarkupLine("[yellow]進入戰鬥後，會回復點數！[/]");
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
            Inventory.ApplyItemEffect(chosenItem, player, monster);

            // 背包扣除 1 個該道具
            Inventory.UseItem(chosenItem.ItemNo);
            actionPoint--;
        }
    }

    public static void ShowBothStatus()
    {
        ShowStatusInfo showStatusInfo = new ShowStatusInfo(player, monster, isShowing);
        showStatusInfo.ShowInfo();

        AnsiConsole.MarkupLine("[grey]按下 [/][bold yellow]任意鍵[/][grey] 返回上一頁...[/]");
        Console.ReadKey(true);
    }


    public static void StartBattle()
    {
        battleCount++;
        // 建立戰鬥系統實例，並啟動戰鬥
        BattleSystem battle = new BattleSystem(player, monster);
        bool isVictory = battle.StartBattle();

        // 如果是逃跑出場，就不進行回血，並增加行動力1點
        if (BattleSystem.isPlayerRunning)
        {
            runningCount++;
            if (actionPoint < 8) actionPoint += 8;
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
            actionPoint = 8;
            isShowing = false;
            dayCount++;
            
            monster = dayCount switch
            {
                2 => new GuestTree(player.Level > 10 ? player.Level : 10),
                3 => new Stone(player.Level > 20 ? player.Level : 20),
                4 => new Bird(player.Level > 30 ? player.Level : 30),
                5 => new DarkHero(player.Level > 40 ? player.Level : 40),
                _ => new Goblin(player.Level > 5 ? player.Level : 5)
            };
        }
        else
        {
            loseCount++;
            if (actionPoint < 8) actionPoint += 8;
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