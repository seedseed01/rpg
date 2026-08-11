using Spectre.Console;
using System.Text.Json;

namespace rpg;

public class TownWalk
{
    public void Walk()
    {
        AnsiConsole.Clear();
        AnsiConsole.MarkupLine($"{GameArt.TownArt}");
        AnsiConsole.WriteLine();

        int Rresult = Random.Shared.Next(0, 100);
        AnsiConsole.WriteLine();
        switch (Rresult)
        {
            case < 15:// 15%
                // 魔物情報
                if(Program.isShowing == true)
                {
                    TalkToPerson();
                }
                else
                {
                    AnsiConsole.MarkupLine("[yellow]探聽到魔物情報![/]");
                    Program.isShowing = true;    
                }
                break;
            case < 50: // 35%
                // 發現道具
                AnsiConsole.MarkupLine("[yellow]在村莊撿到道具![/]");
                GetItem();
                break;
            case < 85: // 35%
                // 村民對話
                TalkToPerson();
                break;
            case < 90: // 5%
                // 無事發生
                AnsiConsole.MarkupLine("[yellow]閒晃了一圈，無事發生。[/]");
                break;
            default: // 10%
                // 踩到陷阱
                AnsiConsole.MarkupLine("[yellow]踩到村民阿薛的陷阱...[/]");
                InTrap();
                break;                 
        }

        Console.ReadKey(true);        
    }

    private static void GetItem()
    {
        var itemArr = ItemDatabase.AllItems;

        if (itemArr != null && itemArr.Count > 0)
        {
            // 隨機抽一個索引 (Index)
            int randomIndex = Random.Shared.Next(itemArr.Count);
            ItemNode pickedItem = itemArr[randomIndex];

            // 提示玩家並加入背包
            AnsiConsole.MarkupLine($"[gold1]你在地上撿到了：[bold]{pickedItem.ItemTitle}[/][/]");
            
            // 呼叫你的背包系統加入道具
            Program.Inventory.AddItem(pickedItem.ItemNo);
        }
    }

    private static void TalkToPerson()
    {
        var personArr = EventDatabase.AllEvents;
        if (personArr != null && personArr.Count > 0)
        {
            // 隨機抽一個索引 (Index)
            int randomIndex = Random.Shared.Next(personArr.Count);
            EventNode personEvent = personArr[randomIndex];

            // 提示玩家遇到人物
            AnsiConsole.MarkupLine($"[gold1]你在路上遇到了：[bold]{personEvent.Name}[/][/]");
            Console.ReadKey(true);
            
            // 呼叫人物事件
            EventRun.ShowEvent(personEvent, Program.player, null);
        }
    }

    private static void InTrap()
    {
        int trapNum = Random.Shared.Next(0, 5);
        switch (trapNum)
        {
            case 0:
                AnsiConsole.MarkupLine("[yellow]掉進屎坑陷阱，臭到中毒...[/]");
                AnsiConsole.MarkupLine("[yellow]變成中毒狀態[/]");
                Program.player.Status = CurrentStatus.Poisoned;
                break;
            case 1:
                AnsiConsole.MarkupLine("[yellow]掉進大麻坑洞裡，嗨到神智不清[/]");
                AnsiConsole.MarkupLine("[yellow]變成迷幻狀態[/]");
                Program.player.Status = CurrentStatus.ManaDrain;
                break;
            case 2:
                AnsiConsole.MarkupLine("[yellow]掉進一個小藍洞，裡面滿滿的水母，被水母叮到麻痺[/]");
                AnsiConsole.MarkupLine("[yellow]變成麻痺狀態[/]");
                Program.player.Status = CurrentStatus.Paralyzed;
                break;
            case 3:
                AnsiConsole.MarkupLine("[yellow]掉進一個10公尺高的小洞，還好你命硬撐了下來，並靠毅力爬了出來，雙手肌肉感到疲勞[/]");
                AnsiConsole.MarkupLine("[yellow]變成無力狀態[/]");
                Program.player.Status = CurrentStatus.Weakened;
                break;
            case 4:
                AnsiConsole.MarkupLine("[yellow]掉進一個漩渦大洞，繞了50圈後被拋出洞外，你被晃到精神恍惚[/]");
                AnsiConsole.MarkupLine("[yellow]變成無神狀態[/]");
                Program.player.Status = CurrentStatus.Muddled;
                break;
            default:
                AnsiConsole.MarkupLine("[yellow]陷阱似乎沒挖好，無事發生。[/]");
                break;
        }
    }
}
