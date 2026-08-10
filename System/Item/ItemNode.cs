using System.Text.Json.Serialization;

namespace rpg;

// 道具結構
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ItemEffectType
{
    AtkBoost,     // 加攻擊
    DefBoost,     // 加防禦
    MatkBoost,    // 加魔攻
    MdefBoost,    // 加魔防
    SpeedBoost,   // 加速度
    HealHp,       // 增加 HP
    HealMp,       // 增加 MP
    ChangeType,   // 改變屬性
    CureStatus,   // 解除異常
    LevelUp,      // 玩家升級
    LevelDown,    // 敵人降級
    LookAll       // 知曉魔物
}

public class ItemNode
{
    public int ItemNo { get; set; } = 0;
    public string ItemTitle { get; set; } = "";
    public string ItemContent { get; set; } = "";

    public ItemEffectType EffectType { get; set; }
    public int Value { get; set; } = 0;
}