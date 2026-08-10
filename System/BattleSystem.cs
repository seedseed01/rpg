using Spectre.Console;

namespace rpg;

public class BattleSystem
{
    private Player player;
    private Monster monster;
    private bool isPlayerDefending = false;
    public static bool isPlayerRunning = false;
    private bool isPlayerRunningfalse = false;

    public BattleSystem(Player player, Monster monster)
    {
        this.player = player;
        this.monster = monster;
    }

    // 💡 開始戰鬥的主入口 (回傳 true 代表玩家獲勝)
    public bool StartBattle()
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new Rule("[bold red]戰鬥開始[/]").Centered());
        AnsiConsole.MarkupLine($"[bold yellow]{player.Name}[/] 遭遇了 [bold red]{monster.Name} (Lv.{monster.Level})[/]！\n");
        AnsiConsole.MarkupLine("[grey]按下任意鍵進入回合...[/]");
        Console.ReadKey(true);

        int round = 1;

        // 戰鬥主要迴圈：只要雙方都還有血量，就持續進行回合
        while (player.CurrentHP > 0 && monster.CurrentHP > 0)
        {
            AnsiConsole.Clear();
            RenderBattleScreen(round);

            // 2. 判斷先攻順序 (速度高者先手)
            bool playerFirst = player.Speed >= monster.Speed;

            if (playerFirst)
            {
                // 玩家先手
                ExecutePlayerTurn();                
                if (isPlayerRunning) break; // 玩家逃跑了，直接結束
                if (monster.CurrentHP <= 0) break; // 怪物被擊倒，直接結束

                AnsiConsole.Clear();
                RenderBattleScreen(round); // 行動後更新血條

                ExecuteMonsterTurn();
            }
            else
            {
                // 怪物先手
                AnsiConsole.MarkupLine($"[bold red]{monster.Name}速度較快，搶先發動攻擊！[/]");
                ExecuteMonsterTurn();
                if (player.CurrentHP <= 0) break; // 玩家被擊倒，直接結束

                AnsiConsole.Clear();
                RenderBattleScreen(round); // 行動後更新血條

                ExecutePlayerTurn();
                if (isPlayerRunning) break; // 玩家逃跑了，直接結束
            }

            round++;
            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine("[grey]--- 本回合結束，按下任意鍵進入下一回合 ---[/]");
            Console.ReadKey(true);
        }

        // 3. 戰鬥結果判定
        return ResolveBattleResult();
    }

    // 🖥️ 繪製戰鬥狀態介面
    private void RenderBattleScreen(int round)
    {
        AnsiConsole.Write(new Rule($"[bold yellow]--- 第 {round} 回合 ---[/]").Centered());

        var statusTable = new Table()
            .Border(TableBorder.Rounded)
            .AddColumn(new TableColumn($"[green]{player.Name} (Lv.{player.Level})[/]").Centered())
            .AddColumn(new TableColumn($"[red]{monster.Name} (Lv.{monster.Level})[/]").Centered())
            .AddRow(
                $"HP: [red]{player.CurrentHP}/{player.MaxHP}[/]\nMP: [blue]{player.CurrentMP}/{player.MaxMP}[/]",
                $"HP: [red]{monster.CurrentHP}/{monster.HP}[/]\nMP: [blue]{monster.MP}[/]"
            );

        AnsiConsole.Write(statusTable);
        AnsiConsole.WriteLine();
    }

    // 🗡️ 玩家回合邏輯
    private void ExecutePlayerTurn()
    {
        AnsiConsole.MarkupLine("[bold green]👉 輪到你的回合！[/]");

        // 重置回合狀態，輪到玩家時才重置上一回合的狀態
        isPlayerDefending = false; // 防禦狀態
        isPlayerRunningfalse = false; // 逃跑狀態

        if(player.Status == CurrentStatus.Paralyzed)
        {
            AnsiConsole.MarkupLine("你在麻痺中，有機率無法行動");
            Console.ReadKey(true);
            int r = Random.Shared.Next(0, 100);
            // 50%機率無法行動
            if(r > 50)
            {
                AnsiConsole.MarkupLine("你無法行動，因為麻痺");
                return;     
            }
            else
            {
                AnsiConsole.MarkupLine("這回合狀況還不錯，麻痺沒影響到你的行動");
            }
            Console.ReadKey(true);
        }

        var action = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[cyan]請選擇行動：[/]")
                .AddChoices(new[] {
                    "物理攻擊 (Physical)",
                    "元素魔法 (Magic, 耗 10 MP)",
                    "防禦 (Defense, 下回合傷害減半)",
                    "逃跑 (Run)"
                }));

        if (action.Contains("物理攻擊"))
        {
            // 1. 播放斬擊動畫！(畫面會彈出動畫視窗播放 0.5 秒)
            // AnimationHelper.PlayAsync(player.Name, monster.Name).Wait();

            AnsiConsole.MarkupLine($"[green]{player.Name}[/] 發動了物理攻擊！");
            int oldHP = monster.CurrentHP;
            int WeakenedAttack = player.Attack;
            // 呼叫怪物的 TakeDamage (傳入物理傷害與屬性)

            // 如果玩家無力，攻擊力降到7成
            if(player.Status == CurrentStatus.Weakened)
            {
                AnsiConsole.MarkupLine("你無力中，攻擊力會比預計的還低");
                WeakenedAttack = (int)(player.Attack * 0.7);   
            }
           
            monster.TakeDamage(WeakenedAttack, player.Type, isMagicAttack: false);

            int damageDealt = oldHP - monster.CurrentHP;
            AnsiConsole.MarkupLine($"對{monster.Name}造成了 [bold red]{damageDealt}[/] 點傷害！");
        }
        else if (action.Contains("元素魔法"))
        {
            if (player.CurrentMP < 10)
            {
                AnsiConsole.MarkupLine("[yellow]魔力 (MP) 不足！強制改為普通攻擊！[/]");
                monster.TakeDamage(player.Attack, player.Type, isMagicAttack: false);
            }
            else
            {
                player.CurrentMP -= 10;
                AnsiConsole.MarkupLine($"[green]{player.Name}[/] 吟唱魔法，發動 [bold cyan]{player.Type}[/] 屬性攻擊！");

                int oldHP = monster.CurrentHP;
                int WeakenedAttack = player.MagicAttack;
                if(player.Status == CurrentStatus.Muddled)
                {
                    AnsiConsole.MarkupLine("你無神中，魔法攻擊力會比預計的還低");
                    WeakenedAttack = (int)(player.MagicAttack * 0.7);  
                } 

                monster.TakeDamage(WeakenedAttack, player.Type, isMagicAttack: true);

                int damageDealt = oldHP - monster.CurrentHP;
                AnsiConsole.MarkupLine($"魔法造成了 [bold red]{damageDealt}[/] 點魔法傷害！");
            }
        }
        else if (action.Contains("防禦"))
        {
            isPlayerDefending = true;
            AnsiConsole.MarkupLine($"[green]{player.Name}[/] 架起了防禦姿態，準備抵擋下一擊！");
        }
        else if (action.Contains("逃跑"))
        {
            AnsiConsole.MarkupLine("[yellow]你倉皇逃跑了...[/]");
            Random random = new Random();
            // 有50%的逃跑機會
            if (random.Next(0, 100) < 50)
            {
                AnsiConsole.MarkupLine("[yellow]逃跑失敗，你破綻大開![/]");
                isPlayerRunningfalse = true;
            }
            else
            {                
                AnsiConsole.MarkupLine("[yellow]逃跑成功![/]");
                isPlayerRunning = true;
            }
        }

        if(player.Status != CurrentStatus.Normal)
        {
            switch (player.Status)
            {
                case CurrentStatus.Poisoned:
                    AnsiConsole.MarkupLine("你中毒了，血量在每回合降低...");
                    int countH = (int)(player.CurrentHP * 0.2);
                    AnsiConsole.MarkupLine($"血量減少 {countH} 點");
                    player.CurrentHP = Math.Max(0, player.CurrentHP - countH);
                    break;
                
                case CurrentStatus.ManaDrain:
                    AnsiConsole.MarkupLine("你在迷幻中，MP在每回合降低...");
                    int countM = (int)(player.CurrentMP * 0.2);
                    AnsiConsole.MarkupLine($"MP減少 {countM} 點");
                    player.CurrentMP = Math.Max(0, player.CurrentMP - countM);
                    break;
            }
        }

        Console.ReadKey(true);
    }

    // 怪物 AI 回合邏輯
    private void ExecuteMonsterTurn()
    {
        if (monster.CurrentHP <= 0) return;

        AnsiConsole.MarkupLine($"\n[bold red]{monster.Name}的回合！[/]");

        // 簡單 AI：預設物理攻擊，防禦狀態下傷害減半
        int rawDamage = monster.Attack;

        if (isPlayerDefending)
        {
            rawDamage /= 2;
            AnsiConsole.MarkupLine("[grey]（因為防禦姿態，受到的傷害減半！）[/]");
        }else if (isPlayerRunningfalse)
        {
            rawDamage *= 2;
            AnsiConsole.MarkupLine("[grey]（因破綻大開，受到的傷害加倍！）[/]");
        }

        int oldHP = player.CurrentHP;
        player.TakeDamage(rawDamage, monster.Type, isMagicAttack: false);

        int damageTaken = oldHP - player.CurrentHP;
        AnsiConsole.MarkupLine($"{monster.Name}對你造成了 [bold red]{damageTaken}[/] 點傷害！");
        Console.ReadKey(true);
    }

    // 🏆 勝負與獎勵發放
    private bool ResolveBattleResult()
    {
        AnsiConsole.WriteLine();
        AnsiConsole.Write(new Rule("[bold yellow]戰鬥結束[/]").Centered());

        if (isPlayerRunning)
        {
            AnsiConsole.MarkupLine($"[bold yellow]你逃離戰鬥了！[/]");
            Console.ReadKey(true);
            return false;
        }
        else if (player.CurrentHP > 0 && monster.CurrentHP <= 0)
        {
            AnsiConsole.MarkupLine($"[bold green]戰鬥勝利！你戰勝了{monster.Name}！[/]");

            // 計算與發放經驗值
            int expReward = monster.Level * 50;
            player.GainEXP(expReward);

            Console.ReadKey(true);
            return true;
        }
        else
        {
            AnsiConsole.MarkupLine($"[bold red]你被{monster.Name}擊倒了... 請再鍛鍊鍛鍊。[/]");
            Console.ReadKey(true);
            return false;
        }
    }
}