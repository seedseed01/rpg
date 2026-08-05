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

    public int Attack { get; set; } = 20;
    public int Defense { get; set; } = 20;
    public int Speed { get; set; } = 20;
    public int MagicAttack { get; set; } = 20;
    public int MagicDefense { get; set; } = 20;

    public Player(string name, int hp, int mp, int attack, int defense, int speed, int magicAttack, int magicDefense, int level,
                CurrentStatus status, CurrentType type, Personality nature)
    {
        Name = name;
        Level = level;        
        Status = status;
        Type = type;
        Nature = nature;

        var (atkM, defM, matkM, mdefM, spdM) = GetNatureModifiers(Nature);

        // 3. 賦值給最終屬性，有性格加成
        MaxHP = hp;
        CurrentHP = MaxHP;
        MaxMP = mp;
        Attack = (int)(attack * atkM);
        Defense = (int)(defense * defM);
        MagicAttack = (int)(magicAttack * matkM);
        MagicDefense = (int)(magicDefense * mdefM);
        Speed = (int)(speed * spdM);
    }

    // 💡 把性格加成抽取成私有方法，避免重複程式碼
    private (double atk, double def, double matk, double mdef, double spd) GetNatureModifiers(Personality nature)
    {
        return nature switch
        {
            Personality.Aggressive => (1.2, 0.9, 1.0, 1.0, 1.0),
            Personality.Cautious   => (0.9, 1.2, 1.0, 1.0, 1.0),
            Personality.Focused    => (1.0, 1.0, 1.2, 0.9, 1.0),
            Personality.Meditative => (1.0, 1.0, 0.9, 1.2, 1.0),
            Personality.Swift      => (1.0, 0.9, 1.0, 0.9, 1.2),
            _                     => (1.0, 1.0, 1.0, 1.0, 1.0)
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
    private void LevelUp()
    {
        Level++;

        var (atkM, defM, matkM, mdefM, spdM) = GetNatureModifiers(Nature);

        // 設定升級成長數值（可根據職業微調）
        int hpGain = 20;
        int mpGain = 10;
        int atkGain = (int)(10 * atkM);
        int defGain = (int)(10 * defM);
        int matkGain = (int)(10 * matkM);
        int mdefGain = (int)(10 * mdefM);
        int spdGain = (int)(10 * spdM);

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
