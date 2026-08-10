using Spectre.Console;
using System.Text.Json;

namespace rpg;

// 執行事件
public static class EventRun
{
    public static void ShowEvent(EventNode eventInfo, Player player, Monster? monster = null)
    {
        var panel = new Panel(eventInfo.Talk)
        {
            Header = new PanelHeader($"[bold yellow] {eventInfo.Name} [/]"),
            Border = BoxBorder.Rounded,
            BorderStyle = new Style(Color.Cyan1),
            Width = 60
        };

        AnsiConsole.Write(new Align(panel, HorizontalAlignment.Left));
        AnsiConsole.WriteLine();
        Console.ReadKey(true);


        switch (eventInfo.EffectType)
        {
            case EventEffectType.ExUp:
                player.EXP += eventInfo.Value;
                AnsiConsole.MarkupLine($"[green]{eventInfo.EventInfo}[/]");
                AnsiConsole.MarkupLine($"[bold green]經驗值提升了 {eventInfo.Value} 點！[/]");
                break;
            
            case EventEffectType.HpDown:
                player.CurrentHP = Math.Max(0, player.CurrentHP - eventInfo.Value);
                AnsiConsole.MarkupLine($"[red]{eventInfo.EventInfo}[/]");
                AnsiConsole.MarkupLine($"[bold red]降低了 {eventInfo.Value} 點 HP！[/]");
                break;
            
            case EventEffectType.MpDown:
                player.CurrentMP = Math.Max(0, player.CurrentMP - eventInfo.Value);
                AnsiConsole.MarkupLine($"[red]{eventInfo.EventInfo}[/]");
                AnsiConsole.MarkupLine($"[bold red]降低了 {eventInfo.Value} 點 MP！[/]");
                break;
            
            case EventEffectType.AtkUp:
                player.Attack += eventInfo.Value;
                AnsiConsole.MarkupLine($"[green]{eventInfo.EventInfo}[/]");
                AnsiConsole.MarkupLine($"[bold green]攻擊力提升了 {eventInfo.Value} 點！[/]");
                break;            
            
            case EventEffectType.GoldLoss:
                Program.gold = Math.Max(0, Program.gold - eventInfo.Value);
                AnsiConsole.MarkupLine($"[red]{eventInfo.EventInfo}[/]");
                AnsiConsole.MarkupLine($"[bold red]扣除了 {eventInfo.Value} 枚錢幣！[/]");              
                break;  

            case EventEffectType.MatkUp:
                player.MagicAttack += eventInfo.Value;
                AnsiConsole.MarkupLine($"[green]{eventInfo.EventInfo}[/]");
                AnsiConsole.MarkupLine($"[bold green]魔法攻擊力提升了 {eventInfo.Value} 點！[/]");
                break; 
            
            case EventEffectType.PointDown:
                Program.actionPoint = Math.Max(0, Program.actionPoint - eventInfo.Value);
                AnsiConsole.MarkupLine($"[red]{eventInfo.EventInfo}[/]");
                AnsiConsole.MarkupLine($"[bold red]行動力一次減少 {eventInfo.Value} 點！[/]");
                break;

            case EventEffectType.SpeedUp:
                player.Speed += eventInfo.Value;
                AnsiConsole.MarkupLine($"[green]{eventInfo.EventInfo}[/]");
                AnsiConsole.MarkupLine($"[bold green]速度提升了 {eventInfo.Value} 點！[/]");
                break;    
        }
    }
}