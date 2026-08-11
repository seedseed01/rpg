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
                AnsiConsole.MarkupLine($"[bold #AFFF00]{eventInfo.EventInfo}[/]");
                AnsiConsole.MarkupLine($"[bold green]經驗值提升了 {eventInfo.Value} 點！[/]");
                player.GainEXP(eventInfo.Value);
                break;
            
            case EventEffectType.HpUp:
                player.MaxHP += eventInfo.Value;
                player.CurrentHP += eventInfo.Value;
                AnsiConsole.MarkupLine($"[bold #AFFF00]{eventInfo.EventInfo}[/]");
                AnsiConsole.MarkupLine($"[bold green]增加了 {eventInfo.Value} 點 HP！[/]");                
                break;

            case EventEffectType.HpDown:
                player.CurrentHP = Math.Max(0, player.CurrentHP - eventInfo.Value);
                AnsiConsole.MarkupLine($"[bold #AFFF00]{eventInfo.EventInfo}[/]");
                AnsiConsole.MarkupLine($"[bold red]降低了 {eventInfo.Value} 點 HP！[/]");
                break;
            
            case EventEffectType.MpDown:
                player.CurrentMP = Math.Max(0, player.CurrentMP - eventInfo.Value);
                AnsiConsole.MarkupLine($"[bold #AFFF00]{eventInfo.EventInfo}[/]");
                AnsiConsole.MarkupLine($"[bold red]降低了 {eventInfo.Value} 點 MP！[/]");
                break;
            
            case EventEffectType.AtkUp:
                player.Attack += eventInfo.Value;
                AnsiConsole.MarkupLine($"[bold #AFFF00]{eventInfo.EventInfo}[/]");
                AnsiConsole.MarkupLine($"[bold green]攻擊力提升了 {eventInfo.Value} 點！[/]");
                break;

            case EventEffectType.DefUp:
                player.Defense += eventInfo.Value;
                AnsiConsole.MarkupLine($"[bold #AFFF00]{eventInfo.EventInfo}[/]");
                AnsiConsole.MarkupLine($"[bold green]防禦力提升了 {eventInfo.Value} 點！[/]");
                break; 
            
            case EventEffectType.GoldLoss:
                Program.gold = Math.Max(0, Program.gold - eventInfo.Value);
                AnsiConsole.MarkupLine($"[bold #AFFF00]{eventInfo.EventInfo}[/]");
                AnsiConsole.MarkupLine($"[bold red]扣除了 {eventInfo.Value} 枚錢幣！[/]");              
                break;  

            case EventEffectType.MatkUp:
                player.MagicAttack += eventInfo.Value;
                AnsiConsole.MarkupLine($"[bold #AFFF00]{eventInfo.EventInfo}[/]");
                AnsiConsole.MarkupLine($"[bold green]魔法攻擊力提升了 {eventInfo.Value} 點！[/]");
                break;

            case EventEffectType.MdefUp:
                player.MagicDefense += eventInfo.Value;
                AnsiConsole.MarkupLine($"[bold #AFFF00]{eventInfo.EventInfo}[/]");
                AnsiConsole.MarkupLine($"[bold green]魔法防禦力提升了 {eventInfo.Value} 點！[/]");
                break;
            
            case EventEffectType.MatkDown:
                player.MagicAttack = Math.Max(0, player.MagicAttack - eventInfo.Value);
                AnsiConsole.MarkupLine($"[bold #AFFF00]{eventInfo.EventInfo}[/]");
                AnsiConsole.MarkupLine($"[bold red]魔法攻擊力降低了 {eventInfo.Value} 點！[/]");
                break;
            
            case EventEffectType.MdefDown:
                player.MagicDefense = Math.Max(0, player.MagicDefense - eventInfo.Value);
                AnsiConsole.MarkupLine($"[bold #AFFF00]{eventInfo.EventInfo}[/]");
                AnsiConsole.MarkupLine($"[bold red]魔法防禦力降低了 {eventInfo.Value} 點！[/]");
                break;
            
            case EventEffectType.PointDown:
                Program.actionPoint = Math.Max(0, Program.actionPoint - eventInfo.Value);
                AnsiConsole.MarkupLine($"[bold #AFFF00]{eventInfo.EventInfo}[/]");
                AnsiConsole.MarkupLine($"[bold red]行動力一次減少 {eventInfo.Value} 點！[/]");
                break;

            case EventEffectType.SpeedUp:
                player.Speed += eventInfo.Value;
                AnsiConsole.MarkupLine($"[bold #AFFF00]{eventInfo.EventInfo}[/]");
                AnsiConsole.MarkupLine($"[bold green]速度提升了 {eventInfo.Value} 點！[/]");
                break;
            
            case EventEffectType.SpeedDown:
                player.Speed = Math.Max(0, player.Speed - eventInfo.Value);
                AnsiConsole.MarkupLine($"[bold #AFFF00]{eventInfo.EventInfo}[/]");
                AnsiConsole.MarkupLine($"[bold red]速度一次減少 {eventInfo.Value} 點！[/]");
                break;
        }
    }
}