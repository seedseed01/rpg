namespace rpg;

public class Monster
{
    public int HP { get; set; }
    public int CurrentHP { get; set; }
    public int MP { get; set; }
    public int Attack { get; set; }
    public int Defense { get; set; }
    public int Speed { get; set; }
    public int MagicAttack { get; set; }
    public int MagicDefense { get; set; }
    public int Level { get; set; }
    public CurrentStatus Status { get; set; }
    public CurrentType Type { get; set; }
    public Personality Nature { get; set; }

    public Monster(int hp, int mp, int attack, int defense, int speed, int magicAttack, int magicDefense, int level,
                CurrentStatus status, CurrentType type, Personality nature)
    {
        Level = level;
        Nature = nature;
        Status = status;
        Type = type;

        double rawAttack = (attack + level) * 1.5;
        double rawDefense = (defense + level) * 1.5;
        double rawMagicAttack = (magicAttack + level) * 1.5;
        double rawMagicDefense = (magicDefense + level) * 1.5;
        double rawSpeed = speed + level;
        
        double atkMulti = 1.0, defMulti = 1.0, matkMulti = 1.0, mdefMulti = 1.0, spdMulti = 1.0;

        switch (nature)
        {
            case Personality.Aggressive: // 暴躁 (物攻 +20%, 物防 -10%)
                atkMulti = 1.2;
                defMulti = 0.9;
                break;
            case Personality.Cautious:   // 謹慎 (物防 +20%, 物攻 -10%)
                defMulti = 1.2;
                atkMulti = 0.9;
                break;
            case Personality.Focused:    // 專注 (魔攻 +20%, 魔防 -10%)
                matkMulti = 1.2;
                mdefMulti = 0.9;
                break;
            case Personality.Meditative: // 冥想 (魔防 +20%, 魔攻 -10%)
                mdefMulti = 1.2;
                matkMulti = 0.9;
                break;
            case Personality.Swift:      // 神行 (速度 +20%, 物防 -10%, 魔防 -10%)
                spdMulti = 1.2;
                defMulti = 0.9;
                mdefMulti = 0.9;
                break;
        }

        // 3. 賦值給最終屬性
        HP = hp + level * 5;
        CurrentHP = HP;
        MP = mp + level * 5;
        Attack = (int)(rawAttack * atkMulti);
        Defense = (int)(rawDefense * defMulti);
        MagicAttack = (int)(rawMagicAttack * matkMulti);
        MagicDefense = (int)(rawMagicDefense * mdefMulti);
        Speed = (int)(rawSpeed * spdMulti);
    }

    // 💡 核心：計算傷害的方法 (傳入攻擊者的屬性來算相剋)
    public void TakeDamage(int rawDamage, CurrentType attackerElement, bool isMagicAttack = false)
    {
        // 1. 決定要用哪種防禦力來抵擋
        int defenseToUse = isMagicAttack ? MagicDefense : Defense;

        // 2. 計算屬性相剋倍率
        double elementModifier = GetElementModifier(attackerElement, this.Type);

        // 3. 算出最終傷害
        int finalDamage = (int)((rawDamage - defenseToUse) * elementModifier);

        if (finalDamage < 1) finalDamage = 1; // 至少造成 1 點傷害

        CurrentHP -= finalDamage;
        if (CurrentHP < 0) CurrentHP = 0;
    }

    // 屬性相剋倍率計算
    private double GetElementModifier(CurrentType attacker, CurrentType defender)
    {
        if (attacker == CurrentType.Water && defender == CurrentType.Fire) return 1.8; // 水克火 1.8倍
        if (attacker == CurrentType.Fire && defender == CurrentType.Wind) return 1.8; // 火克風 1.8倍
        if (attacker == CurrentType.Wind && defender == CurrentType.Earth) return 1.8; // 風克土 1.8倍
        if (attacker == CurrentType.Earth && defender == CurrentType.Water) return 1.8; // 土克水 1.8倍
        if (attacker == CurrentType.Light && defender == CurrentType.Dark) return 1.8; // 光克黑暗 1.8倍
        if (attacker == CurrentType.Dark && defender == CurrentType.Light) return 1.8; // 黑暗克光 1.8倍

        // 屬性相反的話減弱 0.6倍
        if (attacker == CurrentType.Water && defender == CurrentType.Earth) return 0.6;
        if (attacker == CurrentType.Fire && defender == CurrentType.Water) return 0.6;
        if (attacker == CurrentType.Wind && defender == CurrentType.Fire) return 0.6;
        if (attacker == CurrentType.Earth && defender == CurrentType.Wind) return 0.6;
        
        return 1.0;
    }
}
