using Spectre.Console;

namespace rpg;

public class DarkHero : Monster
{
    private bool shieldLeft = false;
    private bool shieldRight = false;
    private int shieldDown = 2;
    private bool shieldSword = false;

    // 預設建構子：只需要傳入等級，其他基礎數值自動帶入種族設定
    public DarkHero(int level) 
        : base(
            name: "死靈英雄",
            hp: 1000,
            mp: 1000,
            attack: 100,
            defense: 100,
            speed: 50,
            magicAttack: 100,
            magicDefense: 100,
            level: level,
            status: CurrentStatus.Normal,
            type: CurrentType.Dark,
            nature: Personality.Meditative
        )
    {
    }

    // 💡 核心：計算傷害的方法 (傳入攻擊者的屬性來算相剋)
    public override void TakeDamage(int rawDamage, CurrentType attackerElement, bool isMagicAttack = false)
    {
        if (shieldSword)
        {
            int reDamage = (int)(rawDamage * 0.2);
            AnsiConsole.Console.WriteLine("你的攻擊部分威力被死靈英雄身周的劍氣反彈");
            AnsiConsole.Console.WriteLine($"對你自己也造成了{reDamage}點傷害！");
            Program.player.CurrentHP -= reDamage;
            return;
        }

        if (isMagicAttack)
        {
            if (shieldRight)
            {
                AnsiConsole.Console.WriteLine("死靈英雄的右手盔甲大盾抵禦魔法攻擊");
                CurrentHP -= 0;
                return;
            }
        }
        else
        {
            if (shieldLeft)
            {
                AnsiConsole.Console.WriteLine("死靈英雄的左手盔甲大盾抵禦物理攻擊");
                CurrentHP -= 0;               
                return;
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

        AnsiConsole.Console.WriteLine("死靈英雄掛在脖子上的項鍊發出亮光，祝福籠罩全身！");
        int plusT = Random.Shared.Next(0, 4);
        switch (plusT)
        {
            case 0:
                AnsiConsole.Console.WriteLine("死靈英雄攻擊力上升！");
                Attack = (int)(Attack *1.1);
                break;
            case 1:
                AnsiConsole.Console.WriteLine("死靈英雄防禦力上升！");
                Defense = (int)(Defense *1.1);
                break;
            case 2:
                AnsiConsole.Console.WriteLine("死靈英雄魔攻力上升！");
                MagicAttack = (int)(MagicAttack *1.1);
                break;
            case 3:
                AnsiConsole.Console.WriteLine("死靈英雄魔防力上升！");
                MagicDefense = (int)(MagicDefense *1.1);
                break;
            default:
                AnsiConsole.Console.WriteLine("死靈英雄速度上升！");
                Speed = (int)(Speed *1.1);
                break;
        }

        if(shieldDown < 2)
        {
            shieldDown++;
            if (shieldDown == 2)
            {
                AnsiConsole.Console.WriteLine("死靈英雄大盾效果消失！");
                shieldLeft = false;
                shieldRight = false;
            }
        }

        int atkLogic = Random.Shared.Next(0, 10);
        if(CurrentHP < 100 && CurrentMP >= 100 && shieldSword == false)
        {            
            AnsiConsole.Console.WriteLine("死靈英雄手中長劍爆出藍光！");
            AnsiConsole.Console.WriteLine("不但劍氣向你飛來，並且也覆蓋死靈英雄全身");            
            CurrentMP -= 100;
            shieldSword = true;

            damage = Attack *2;
            isMagicAttack = true;
        }
        else if (atkLogic < 2 && CurrentMP >= 50 && shieldDown == 2)
        {
            int shieldCount = Random.Shared.Next(0, 3);
            if (shieldCount == 0)
            {
                AnsiConsole.Console.WriteLine("死靈英雄左手盔甲張啟大盾");
                AnsiConsole.Console.WriteLine("暫時絕對防禦物理攻擊");
                shieldLeft = true;
            }
            else if (shieldCount == 1)
            {
                AnsiConsole.Console.WriteLine("死靈英雄右手盔甲張啟大盾");
                AnsiConsole.Console.WriteLine("暫時絕對防禦魔法攻擊");
                shieldRight = true;
            }
            else
            {
                AnsiConsole.Console.WriteLine("死靈英雄盔甲張起失敗!");
            }
            CurrentMP -= 50;
            shieldDown = 0;

            damage = 0;
            isMagicAttack = true;
        }
        else if (atkLogic < 7 && CurrentMP >= 20)
        {
            AnsiConsole.Console.WriteLine("死靈英雄放出元素攻擊");
            CurrentMP -= 20;

            damage = MagicAttack;
            isMagicAttack = true;
        }
        else
        {
            AnsiConsole.Console.WriteLine("死靈英雄揮劍劈斬");
            damage = Attack;
            isMagicAttack = false;
        }


        return (damage, isMagicAttack);
    }
}
