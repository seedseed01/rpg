using System.Text.Json.Serialization;

namespace rpg;

// 道具結構
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum EventEffectType
{
    ExUp,        // 經驗提升
    HpUp,        // HP增加
    AtkUp,       // 攻擊加持
    DefUp,       // 防禦加持
    MatkUp,      // 魔法攻擊加持
    MdefUp,      // 魔法防禦加持
    SpeedUp,     // 速度加持

    HpDown,      // HP減少
    MpDown,      // MP減少
    MatkDown,    // 魔法攻擊減少
    MdefDown,    // 魔法防禦減少
    SpeedDown,   // 速度減少
    PointDown,   // 行動力減少
    GoldLoss,    // 金錢損失
}

public class EventNode
{
    public string Name { get; set; } = "";
    public string Talk { get; set; } = "";
    public string EventInfo { get; set; } = "";

    public EventEffectType EffectType { get; set; }
    public int Value { get; set; } = 0;
}