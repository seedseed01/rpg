using Spectre.Console;

namespace rpg;

public class Goblin : Monster
{
    // 預設建構子：只需要傳入等級，其他基礎數值自動帶入哥布林的種族設定
    public Goblin(int level) 
        : base(
            name: "哥布林",
            hp: 50,
            mp: 30,
            attack: 25,
            defense: 20,
            speed: 14,
            magicAttack: 7,
            magicDefense: 3,
            level: level,
            status: CurrentStatus.Normal,
            type: CurrentType.Wind,
            nature: Personality.Aggressive
        )
    {
    }
    public override (int damage, bool isMagicAttack) SkillAttack()
    {
        if(CurrentHP < 20)
        {
            AnsiConsole.Console.WriteLine("哥布林陷入狂暴，攻擊力比平時更危險了!");
            Attack += 10;
        }
        else
        {
            AnsiConsole.Console.WriteLine("哥布林揮舞狼牙棒攻擊!");    
        }

        int damage = Attack;
        bool isMagicAttack = false;
        return (damage, isMagicAttack);
    }
}
