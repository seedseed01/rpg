using Spectre.Console;

namespace rpg;

class Program
{
    private static string? _name;
    public record MenuItem(int Id, string DisplayName);

    static void Main(string[] args)
    {
        // 清空畫面
        AnsiConsole.Clear();

        // 1. 印出漂亮的標題
        AnsiConsole.Write(
            new FigletText("RPG GAME")
                .Color(Color.Red));

        var name = AnsiConsole.Ask<string>("輸入您的名字 [green]name[/]?");
        _name = name;

        while (true)
        {
            // 選項:角色狀態、戰鬥畫面、退出
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<MenuItem>()
                    .Title("[cyan]請選擇一個選項[/]")
                    .PageSize(5)
                    .UseConverter(item => item.DisplayName)
                    .AddChoices(new[] {
                        new MenuItem(1, "1. 角色狀態"),
                        new MenuItem(2, "2. 戰鬥畫面"),
                        new MenuItem(3, "3. 退出")
                    }));
            
            // 清空畫面
            AnsiConsole.Clear();
    
            switch (choice.Id)
            {
                case 1:
                    ShowInfo();
                    break;
                case 2:
                    // Battle();
                    // break;
                case 3:
                    AnsiConsole.MarkupLine("[yellow]感謝使用！[/]");
                    return;                    
                default:
                    AnsiConsole.MarkupLine("[red]選擇錯誤！[/]");
                    break;
            }
        }
    }

    public static void ShowInfo()
    {
        // 用表格做一個角色狀態欄
        var statusTable = new Table()
            .Border(TableBorder.Rounded)
            .AddColumn("[yellow]屬性[/]")
            .AddColumn("[yellow]數值[/]");

        statusTable.AddRow("職業", "⚔️ 劍士");
        statusTable.AddRow("血量 (HP)", "[red]85 / 100[/]");
        statusTable.AddRow("魔力 (MP)", "[blue]30 / 50[/]");
        statusTable.AddRow("持有金幣", "[gold1]120 G[/]");

        // 印出帶外框的面板
        AnsiConsole.Write(
            new Panel(statusTable)
                .Header("[bold green] 玩家狀態 [/]")
                .Expand());

        AnsiConsole.WriteLine(); // 留一行空行
    }
}
