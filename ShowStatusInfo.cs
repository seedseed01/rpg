using Spectre.Console;

namespace rpg;

public class ShowStatusInfo
{
    private Player player;
    private Monster monster;

    public ShowStatusInfo(Player player, Monster monster)
    {
        this.player = player;
        this.monster = monster;
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

        // 使用並排兩欄 Table 把雙方面板放進去
        var layoutTable = new Table()
            .Border(TableBorder.None)
            .AddColumn(new TableColumn("[green]=== 玩家資訊 ===[/]").Centered())
            .AddColumn(new TableColumn("[red]=== 遭遇怪物 ===[/]").Centered())
            .AddRow(playerTable, monsterTable);

        AnsiConsole.Write(layoutTable);
        AnsiConsole.WriteLine();
    }

}