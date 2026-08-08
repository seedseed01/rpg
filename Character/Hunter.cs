namespace rpg;

public class Hunter : Player
{
    public Hunter (int level, CurrentType type, Personality nature) 
        : base(
            name: "獵人",
            hp: 100,
            mp: 50,
            attack: 30,
            defense: 30,
            speed: 30,
            magicAttack: 15,
            magicDefense: 15,
            level: level,
            status: CurrentStatus.Normal,
            type: type,
            nature: nature
        )
    {
    }    
}
