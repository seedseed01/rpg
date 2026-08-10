using System.Text.Json.Serialization;

namespace rpg;

// 道具結構
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum EventEffectType
{
    ExUp,        // 經驗提升
    HpDown,      // HP減少
    MpDown,      // MP減少
    AtkUp,       // 攻擊加持
    GoldLoss,    // 金錢損失
    MatkUp,      // 魔法攻擊加持
    PointDown,   // 行動力減少
    SpeedUp,     // 速度加持
}

public class EventNode
{
    public string Name { get; set; } = "";
    public string Talk { get; set; } = "";
    public string EventInfo { get; set; } = "";

    public EventEffectType EffectType { get; set; }
    public int Value { get; set; } = 0;
}