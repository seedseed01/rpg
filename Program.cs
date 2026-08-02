using Spectre.Console;

namespace RPG;

class Program
{
    private static string? _name;

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
        
        // 2. 用表格做一個角色狀態欄
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

        // 3. 建立支援【方向鍵 ↑ ↓ 選擇】的互動選單
        var action = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title($"[cyan]前面出現了一隻哥布林！{_name}要怎麼做？[/]")
                .PageSize(5)
                .AddChoices(new[] {
                    "⚔️ 揮劍攻擊",
                    "🛡️ 舉盾防禦",
                    "🎒 打開背包",
                    "🏃 嘗試逃跑"
                }));

        // 4. 印出選擇結果
        AnsiConsole.MarkupLine($"\n你選擇了：[bold yellow]{action}[/]");
    }
}
