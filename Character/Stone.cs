using Spectre.Console;

namespace rpg;

public class Stone : Monster
{
    // 預設建構子：只需要傳入等級，其他基礎數值自動帶入種族設定
    public Stone(int level) 
        : base(
            name: "石像鬼",
            hp: 200,
            mp: 30,
            attack: 70,
            defense: 70,
            speed: 10,
            magicAttack: 50,
            magicDefense: 60,
            level: level,
            status: CurrentStatus.Normal,
            type: CurrentType.Earth,
            nature: Personality.Cautious
        )
    {
    }

    // 💡 核心：計算傷害的方法 (傳入攻擊者的屬性來算相剋)
    public override void TakeDamage(int rawDamage, CurrentType attackerElement, bool isMagicAttack = false)
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

    public override (int damage, bool isMagicAttack) SkillAttack()
    {
        int damage;
        bool isMagicAttack;

        int atkLogic = Random.Shared.Next(0, 10);
        if(CurrentHP < 100 && CurrentMP >= 80)
        {            
            AnsiConsole.Console.WriteLine("石像鬼: 吼吼啊阿阿溝吼!啊達!");
            AnsiConsole.Console.WriteLine("石像鬼感到危機，使出了技能，防禦上升");
            Defense += 30;
            MagicDefense += 30;
            CurrentMP -= 80;
            damage = 0;
            isMagicAttack = false;
        }
        else if (atkLogic < 3)
        {
            AnsiConsole.Console.WriteLine("石像鬼的捨身衝撞!");
            AnsiConsole.Console.WriteLine("石像鬼自身也小損傷");
            CurrentHP -= 20;
            damage = Attack + 20;
            isMagicAttack = false;
        }
        else
        {
            AnsiConsole.Console.WriteLine("石像鬼揮舞石頭攻擊!");
            damage = Attack;
            isMagicAttack = false;
        }


        return (damage, isMagicAttack);
    }
}
