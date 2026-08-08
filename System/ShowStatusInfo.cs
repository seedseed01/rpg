using Spectre.Console;

namespace rpg;

public class ShowStatusInfo
{
    private Player player;
    private Monster monster;
    private bool isShowing = false;

    public ShowStatusInfo(Player player, Monster monster, bool isShowing)
    {
        this.player = player;
        this.monster = monster;
        this.isShowing = isShowing;
    }


    public void ShowInfo()
    {
        AnsiConsole.Clear();

        // 建立玩家狀態表
        var playerTable = new Table()
            .Border(TableBorder.Rounded)
            .AddColumn("[yellow]屬性[/]")
            .AddColumn("[yellow]數值[/]")
            .AddRow("名稱", player.Name)
            .AddRow("等級", $"[yellow]Lv.{player.Level}[/]")
            .AddRow("血量 (HP)", $"[red]{player.CurrentHP} / {player.MaxHP}[/]")
            .AddRow("魔力 (MP)", $"[blue]{player.CurrentMP} / {player.MaxMP}[/]")
            .AddRow("狀態", $"[green]{player.Status.ToChinese()}[/]")
            .AddRow("屬性", $"[yellow]{player.Type.ToChinese()}[/]")
            .AddRow("性格", $"[yellow]{player.Nature.ToChinese()}[/]")
            .AddRow("攻擊 (AT)", $"{player.Attack}")
            .AddRow("防禦 (DE)", $"{player.Defense}")
            .AddRow("魔攻 (MA)", $"{player.MagicAttack}")
            .AddRow("魔防 (MD)", $"{player.MagicDefense}")
            .AddRow("速度 (SP)", $"{player.Speed}");

        // 建立怪物狀態表
        var monsterTable = new Table()
            .Border(TableBorder.Rounded)
            .AddColumn("[red]屬性[/]")
            .AddColumn("[red]數值[/]")
            .AddRow("名稱", monster.Name)
            .AddRow("等級", $"[yellow]Lv.{monster.Level}[/]")
            .AddRow("血量 (HP)", $"[red]{monster.CurrentHP} / {monster.HP}[/]")
            .AddRow("魔力 (MP)", $"[blue]{monster.MP}[/] ")
            .AddRow("狀態", $"[green]{monster.Status.ToChinese()}[/]")
            .AddRow("屬性", $"[yellow]{monster.Type.ToChinese()}[/]")
            .AddRow("性格", $"[yellow]{monster.Nature.ToChinese()}[/]")
            .AddRow("攻擊 (AT)", $"{monster.Attack}")
            .AddRow("防禦 (DE)", $"{monster.Defense}")
            .AddRow("魔攻 (MA)", $"{monster.MagicAttack}")
            .AddRow("魔防 (MD)", $"{monster.MagicDefense}")
            .AddRow("速度 (SP)", $"{monster.Speed}");

        var unknownTable = new Table()
            .Border(TableBorder.Rounded)
            .AddColumn("[red]屬性[/]")
            .AddColumn("[red]數值[/]")
            .AddRow("名稱", monster.Name)
            .AddRow("等級", $"[yellow]Lv.**[/]")
            .AddRow("血量 (HP)", $"[red]*** / ***[/]")
            .AddRow("魔力 (MP)", $"[blue]*** / ***[/] ")
            .AddRow("狀態", $"[green]**[/]")
            .AddRow("屬性", $"[yellow]**[/]")
            .AddRow("性格", $"[yellow]**[/]")
            .AddRow("攻擊 (AT)", $"***")
            .AddRow("防禦 (DE)", $"***")
            .AddRow("魔攻 (MA)", $"***")
            .AddRow("魔防 (MD)", $"***")
            .AddRow("速度 (SP)", $"***");

        // 使用並排兩欄 Table 把雙方面板放進去
        var layoutTable = new Table()
            .Border(TableBorder.None)
            .AddColumn(new TableColumn("[green]=== 玩家資訊 ===[/]").Centered())
            .AddColumn(new TableColumn("[red]=== 敵人資訊 ===[/]").Centered())
            .AddRow(playerTable, isShowing ? monsterTable : unknownTable);

        AnsiConsole.Write(layoutTable);
        AnsiConsole.WriteLine();
    }

}