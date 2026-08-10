namespace rpg;

public class Player
{
    public string Name { get; set; }

    public int Level { get; set; } = 1;
    public int EXP { get; set; } = 0;
    public int MaxEXP => Level * 100; // 升級所需經驗值公式 (例如：Lv.1要100，Lv.2要200)

    public CurrentStatus Status { get; set; }
    public CurrentType Type { get; set; }
    public Personality Nature { get; set; }

    public int CurrentHP { get; set; }
    public int CurrentMP { get; set; }

    public int MaxHP { get; set; }
    public int MaxMP { get; set; }

    public int Attack { get; set; }
    public int Defense { get; set; }
    public int Speed { get; set; }
    public int MagicAttack { get; set; }
    public int MagicDefense { get; set; }

    public Player(string name, int hp, int mp, int attack, int defense, int magicAttack, int magicDefense, int speed, int level,
                CurrentStatus status, CurrentType type, Personality nature)
    {
        Name = name;
        Level = level;
        Status = status;
        Type = type;
        Nature = nature;

        var (atkM, defM, matkM, mdefM, spdM) = GetNatureModifiers(Nature);

        MaxHP = hp + (level - 1) * 10;
        CurrentHP = MaxHP;
        MaxMP = mp + (level - 1) * 10;
        CurrentMP = MaxMP;
        Attack = attack + (level - 1) * 5 * atkM;
        Defense = defense + (level - 1) * 5 * defM;
        MagicAttack = magicAttack + (level - 1) * 5 * matkM;
        MagicDefense = magicDefense + (level - 1) * 5 * mdefM;
        Speed = speed + (level - 1) * 5 * spdM;
    }

    // 性格加成計算
    private (int atk, int def, int matk, int mdef, int spd) GetNatureModifiers(Personality nature)
    {
        return nature switch
        {
            Personality.Aggressive => (3, 1, 2, 2, 2),
            Personality.Cautious => (1, 3, 2, 2, 2),
            Personality.Focused => (2, 2, 3, 1, 2),
            Personality.Meditative => (2, 2, 1, 3, 2),
            Personality.Swift => (2, 1, 2, 1, 3),
            _ => (2, 2, 2, 2, 2)
        };
    }

    // 💡 獲得經驗值的方法
    public void GainEXP(int amount)
    {
        EXP += amount;
        Console.WriteLine($"{Name} 獲得了 {amount} 點經驗值！");

        // 判斷是否滿足升級條件 (用 while 防止一口氣升很多級)
        while (EXP >= MaxEXP)
        {
            EXP -= MaxEXP; // 扣除升級所需經驗值，保留溢出的經驗
            LevelUp();
        }
    }

    // 💡 升級邏輯：成長公式寫在這裡！
    public void LevelUp()
    {
        Level++;

        var (atkM, defM, matkM, mdefM, spdM) = GetNatureModifiers(Nature);

        // 設定升級成長數值（可根據職業微調）
        int hpGain = 10;
        int mpGain = 10;
        int atkGain = 5 * atkM;
        int defGain = 5 * defM;
        int matkGain = 5 * matkM;
        int mdefGain = 5 * mdefM;
        int spdGain = 5 * spdM;

        // 提升最大面板
        MaxHP += hpGain;
        MaxMP += mpGain;
        Attack += atkGain;
        Defense += defGain;
        MagicAttack += matkGain;
        MagicDefense += mdefGain;
        Speed += spdGain;

        // 升級福利：血量魔量補滿！
        CurrentHP = MaxHP;
        CurrentMP = MaxMP;

        Console.WriteLine($"\n🎉 恭喜！{Name} 升到了 Lv.{Level}！");
        Console.WriteLine($"HP+{hpGain} | MP+{mpGain} | 攻擊+{atkGain} | 防禦+{defGain} | 魔攻+{matkGain} | 魔防+{mdefGain} | 速度+{spdGain}\n");
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
