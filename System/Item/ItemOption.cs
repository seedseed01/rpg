namespace rpg;

// 道具顯示
public class ItemOption
{
    public ItemNode Node { get; set; }
    public int Count { get; set; }

    public ItemOption(ItemNode node, int count)
    {
        Node = node;
        Count = count;
    }

    // 💡 搭配前面學到的：這會決定選單畫面上印出什麼字串！
    public override string ToString()
    {
        // 如果是取消選單，就不印出道具名稱
        if (Node.ItemNo == -1) return $"取消 (返回)";
        return $"{Node.ItemTitle} x{Count} - [grey]{Node.ItemContent}[/]";
    }
}