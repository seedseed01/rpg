using Spectre.Console;

namespace rpg;

public class Bird : Monster
{
    private bool wing = true;
    private int wingCount = 2;
    // 預設建構子：只需要傳入等級，其他基礎數值自動帶入種族設定
    public Bird(int level) 
        : base(
            name: "絨絨鳥",
            hp: 300,
            mp: 500,
            attack: 85,
            defense: 50,
            speed: 50,
            magicAttack: 85,
            magicDefense: 60,
            level: level,
            status: CurrentStatus.Normal,
            type: CurrentType.Water,
            nature: Personality.Swift
        )
    {
    }

    // 💡 核心：計算傷害的方法 (傳入攻擊者的屬性來算相剋)
    public override void TakeDamage(int rawDamage, CurrentType attackerElement, bool isMagicAttack = false)
    {
        if (isMagicAttack)
        {
            if (wing)
            {
                AnsiConsole.Console.WriteLine("絨絨鳥的絨毛可以吸收魔法攻擊");
                int getHp = (int)(rawDamage * 0.2);
                AnsiConsole.Console.WriteLine($"絨絨鳥吸收了魔法攻擊，回復了 {getHp} 點 HP");
                if ((CurrentHP + getHp) > HP) CurrentHP = HP;
                return;
            }
            else
            {
                AnsiConsole.Console.WriteLine("絨絨鳥目前無絨毛防身，魔法攻擊效果顯著");
                rawDamage = (int)(rawDamage * 1.5);
            }
        }

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

        if(wingCount < 2)
        {
            wingCount++;
            if (wingCount == 2)
            {
                wing = true;
                AnsiConsole.Console.WriteLine("絨絨鳥的絨毛又再生了!!");
            }
        } 

        var allTypes = Enum.GetValues<CurrentType>();
        CurrentType newType = allTypes[Random.Shared.Next(allTypes.Length)];
        Type = newType;
        AnsiConsole.MarkupLine($"[bold magenta]{Name}[/] 身體發出微光，屬性隨機轉變成了 [yellow]{Type}[/]！");

        int atkLogic = Random.Shared.Next(0, 10);
        if(CurrentHP < 100 && CurrentMP >= 100 && wing)
        {            
            AnsiConsole.Console.WriteLine("絨絨鳥: 喲喲伊喲喲伊伊伊喲");
            AnsiConsole.Console.WriteLine("絨絨鳥感到危機，吸收絨毛提升自己能力");
            AnsiConsole.Console.WriteLine("絨絨鳥全能力提升，但暫無絨毛防身");

            CurrentHP += 100;
            Attack += 50;
            MagicAttack += 50;
            Defense += 50;
            MagicDefense += 50;
            Speed += 50;
            CurrentMP -= 100;
            wingCount = 0;
            wing = false;

            damage = 0;
            isMagicAttack = false;
        }
        else if (atkLogic < 2 && wing && CurrentMP >= 50)
        {
            AnsiConsole.Console.WriteLine("絨絨鳥丟出絨毛炸彈，威力驚人，但使得絨絨鳥暫時無絨毛防身");
            CurrentMP -= 50;
            wingCount = 0;
            wing = false;
            
            damage = MagicAttack * 2;
            isMagicAttack = true;
        }
        else if (atkLogic < 7 && CurrentMP >= 20)
        {
            AnsiConsole.Console.WriteLine("絨絨鳥煽動翅膀，颳起魔力風箭！");
            CurrentMP -= 20;

            damage = MagicAttack;
            isMagicAttack = true;
        }
        else
        {
            AnsiConsole.Console.WriteLine("絨絨鳥俯身衝鋒，使出尖嘴攻擊");
            damage = Attack;
            isMagicAttack = false;
        }


        return (damage, isMagicAttack);
    }
}
