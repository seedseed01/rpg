namespace rpg;

public class Goblin : Monster
{
    // 預設建構子：只需要傳入等級，其他基礎數值自動帶入哥布林的種族設定
    public Goblin(int level) 
        : base(
            name: "哥布林",
            hp: 50,
            mp: 30,
            attack: 15,
            defense: 10,
            speed: 14,
            magicAttack: 7,
            magicDefense: 3,
            level: level,
            status: CurrentStatus.Normal,
            type: CurrentType.Earth,
            nature: Personality.Aggressive
        )
    {
    }    
}
