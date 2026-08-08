namespace rpg;

public class Goblin : Monster
{
    // 預設建構子：只需要傳入等級，其他基礎數值自動帶入哥布林的種族設定
    public Goblin(int level) 
        : base(
            name: "哥布林",
            hp: 50,
            mp: 30,
            attack: 28,
            defense: 21,
            speed: 18,
            magicAttack: 9,
            magicDefense: 10,
            level: level,
            status: CurrentStatus.Normal,
            type: CurrentType.Earth,
            nature: Personality.Aggressive
        )
    {
    }    
}
