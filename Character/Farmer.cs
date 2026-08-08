namespace rpg;

public class Farmer : Player
{
    public Farmer (int level, CurrentType type, Personality nature) 
        : base(
            name: "農夫",
            hp: 100,
            mp: 50,
            attack: 25,
            defense: 40,
            speed: 10,
            magicAttack: 10,
            magicDefense: 10,
            level: level,
            status: CurrentStatus.Normal,
            type: type,
            nature: nature
        )
    {
    }    
}
