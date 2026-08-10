using Spectre.Console;
using System.Text.Json;

namespace rpg;

// 道具管理
public class Inventory
{
    public Dictionary<int, int> Items { get; set; } = new();

    // 獲得道具
    public void AddItem(int itemNo, int count = 1)
    {
        if (Items.ContainsKey(itemNo))
            Items[itemNo] += count;
        else
            Items[itemNo] = count;
    }

    // 消耗道具
    public bool UseItem(int itemNo)
    {
        if (Items.ContainsKey(itemNo) && Items[itemNo] > 0)
        {
            Items[itemNo]--;
            if (Items[itemNo] == 0) Items.Remove(itemNo); // 數量歸零則移除
            return true;
        }
        return false;
    }

    // 顯示道具清單
    // 顯示背包並讓玩家選擇使用道具
    public ItemNode? ShowAndUseItemMenu()
    {
        // 1. 檢查背包是否為空
        if (Items.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]你的背包空空如也！[/]");
            Console.ReadKey(true);
            return null;
        }

        // 2. 將字典裡的 ItemNo 拿去對照 allItems，組合成選單選項
        var options = new List<ItemOption>();

        foreach (var kvp in Items)
        {
            int itemNo = kvp.Key;
            int count = kvp.Value;

            // 從完整的道具資料庫搜尋對應的 ItemNode
            ItemNode? targetNode = ItemDatabase.AllItems.FirstOrDefault(x => x.ItemNo == itemNo);

            if (targetNode != null)
            {
                options.Add(new ItemOption(targetNode, count));
            }
        }

        // 加上一個「離開/取消」選項
        var cancelOption = new ItemOption(new ItemNode { ItemNo = -1, ItemTitle = "取消 (返回)" }, 0);
        options.Add(cancelOption);

        // 3. 跳出 Spectre.Console 選擇選單
        var selectedOption = AnsiConsole.Prompt(
            new SelectionPrompt<ItemOption>()
                .Title("[cyan]請選擇要使用的道具：[/]")
                .PageSize(8)
                .AddChoices(options));

        // 4. 如果選擇取消，回傳 null
        if (selectedOption.Node.ItemNo == -1)
        {
            return null;
        }

        // 5. 回傳玩家選中的道具物件！
        return selectedOption.Node;
    }

    public static void ApplyItemEffect(ItemNode item, Player player, Monster? monster = null)
    {
        switch (item.EffectType)
        {
            case ItemEffectType.AtkBoost:
                player.Attack += item.Value;
                AnsiConsole.MarkupLine($"[green]攻擊力提升了 {item.Value} 點！[/]");
                break;
            
            case ItemEffectType.DefBoost:
                player.Defense += item.Value;
                AnsiConsole.MarkupLine($"[green]防禦力提升了 {item.Value} 點！[/]");
                Console.ReadKey(true);
                break;
            
            case ItemEffectType.MatkBoost:
                player.MagicAttack += item.Value;
                AnsiConsole.MarkupLine($"[green]魔法攻擊力提升了 {item.Value} 點！[/]");
                Console.ReadKey(true);
                break;
            
            case ItemEffectType.MdefBoost:
                player.MagicDefense += item.Value;
                AnsiConsole.MarkupLine($"[green]魔法防禦力提升了 {item.Value} 點！[/]");
                Console.ReadKey(true);
                break;
            
            case ItemEffectType.SpeedBoost:
                player.Speed += item.Value;
                AnsiConsole.MarkupLine($"[green]速度提升了 {item.Value} 點！[/]");
                Console.ReadKey(true);
                break;

            case ItemEffectType.HealHp:
                player.MaxHP += item.Value;
                player.CurrentHP += item.Value;
                AnsiConsole.MarkupLine($"[green]提升了 {item.Value} 點 HP！[/]");
                Console.ReadKey(true);
                break;
            
            case ItemEffectType.HealMp:
                player.MaxMP += item.Value;
                player.CurrentMP += item.Value;                
                AnsiConsole.MarkupLine($"[green]提升了 {item.Value} 點 MP！[/]");
                Console.ReadKey(true);
                break;
            
            case ItemEffectType.ChangeType:
                Random random = new Random();
                int Rresult = random.Next(0, 6);
                player.Type = Rresult switch
                {
                    0 => CurrentType.Normal,
                    1 => CurrentType.Fire,
                    2 => CurrentType.Water,
                    3 => CurrentType.Earth,
                    4 => CurrentType.Wind,
                    5 => CurrentType.Dark,
                    6 => CurrentType.Light,
                    _ => CurrentType.Normal
                };
                AnsiConsole.MarkupLine($"[green]隨機改變屬性！[/]");
                Console.ReadKey(true);
                break;
            
            case ItemEffectType.CureStatus:
                player.Status = CurrentStatus.Normal;
                AnsiConsole.MarkupLine($"[green]解除了異常！[/]");
                Console.ReadKey(true);
                break;

            case ItemEffectType.LevelUp:
                for(int i = 0; i < item.Value; i++)
                {
                    player.LevelUp();
                    Console.ReadKey(true);
                }
                break;

            case ItemEffectType.LevelDown:
                if (monster != null)
                {
                    monster.Level = Math.Max(1, monster.Level - item.Value);                    
                    AnsiConsole.MarkupLine($"[purple]魔物等級降低了 {item.Value} 等！[/]");
                    Console.ReadKey(true);
                }
                break;

            case ItemEffectType.LookAll:
                Program.isShowing = true;
                AnsiConsole.MarkupLine($"[yellow]知曉魔物！[/]");
                Console.ReadKey(true);
                break;
        }
    }
}