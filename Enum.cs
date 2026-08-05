namespace rpg;

public enum CurrentStatus
{
    Normal,      // 一般
    Dead,        // 死亡
    Poisoned,    // 中毒 (逐漸扣 HP)
    ManaDrain,   // 迷幻 (逐漸扣 MP，亦可用 Psyched/ManaBleed)
    Paralyzed,   // 麻痺
    Asleep,      // 睡眠
    Weakened,    // 無力 (攻擊力降低)
    Muddled,     // 無神 (魔力/精神力降低，亦可用 Dazed/Enfeebled)
}

public enum CurrentType
{
    Normal,   // 一般
    Fire,     // 火
    Water,    // 水
    Earth,    // 土
    Wind,     // 風
    Dark,     // 黑暗
    Light,    // 光
}

public enum Personality
{
    Balanced,    // 平衡 (不加不減)
    Aggressive,  // 暴躁 (攻擊 +20%, 防禦 -10%)
    Cautious,    // 謹慎 (防禦 +20%, 攻擊 -10%)
    Focused,     // 專注 (魔攻 +20%, 魔防 -10%)
    Meditative,  // 冥想 (魔防 +20%, 魔攻 -10%)
    Swift        // 神行 (速度 +20%, 防禦 -10%, 魔防 -10%)
}