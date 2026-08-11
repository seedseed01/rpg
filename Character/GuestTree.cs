using Spectre.Console;

namespace rpg;

public class GuestTree : Monster
{
    // 預設建構子：只需要傳入等級，其他基礎數值自動帶入種族設定
    public GuestTree(int level)
        : base(
            name: "幽靈大樹",
            hp: 150,
            mp: 150,
            attack: 20,
            defense: 30,
            speed: 20,
            magicAttack: 40,
            magicDefense: 60,
            level: level,
            status: CurrentStatus.Normal,
            type: CurrentType.Fire,
            nature: Personality.Focused
        )
    {
    }

    // 💡 核心：計算傷害的方法 (傳入攻擊者的屬性來算相剋)
    public override void TakeDamage(int rawDamage, CurrentType attackerElement, bool isMagicAttack = false)
    {
        // 1. 決定要用哪種防禦力來抵擋
        if (!isMagicAttack)
        {
            // 不怕物理攻擊，0傷害 (如果魔力不足，就能用物理攻擊)
            if (CurrentMP >= 20)
            {
                AnsiConsole.MarkupLine("[yellow]物理攻擊對幽靈大樹無效，此次攻擊無法造成傷害！[/]");
                CurrentHP -= 0;
                return;
            }
            else
            {
                AnsiConsole.MarkupLine("[yellow]幽靈大樹的魔力不足以維持狀態，物理攻擊開始生效！[/]");
            }
        }

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

        if (CurrentHP < 20)
        {
            AnsiConsole.Console.WriteLine("幽靈大樹陷入狂暴，魔力大增中!");
            AnsiConsole.Console.WriteLine("幽靈大樹釋放大鬼火飛濺!");
            MagicAttack += 10;
            CurrentMP -= 20;

            damage = MagicAttack;
            isMagicAttack = true;
        }
        else if (CurrentMP >= 20)
        {
            AnsiConsole.Console.WriteLine("幽靈大樹釋放鬼火飛濺!");
            CurrentMP -= 20;

            damage = MagicAttack;
            isMagicAttack = true;
        }
        else
        {
            AnsiConsole.Console.WriteLine("幽靈大樹揮舞樹枝攻擊!");
            damage = Attack;
            isMagicAttack = false;
        }

        return (damage, isMagicAttack);
    }
}
