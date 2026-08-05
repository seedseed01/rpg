namespace rpg;

public class Goblin : Monster
{
    // 預設建構子：只需要傳入等級，其他基礎數值自動帶入哥布林的種族設定
    public Goblin(int level) 
        : base(
            name: "哥布林",
            hp: 30,
            mp: 10,
            attack: 20,
            defense: 15,
            speed: 15,
            magicAttack: 5,
            magicDefense: 5,
            level: level,
            status: CurrentStatus.Normal,
            type: CurrentType.Earth,
            nature: Personality.Aggressive
        )
    {
    }    
}
