namespace rpg;

public class Woodsman : Player
{
    public Woodsman (int level, CurrentType type, Personality nature) 
        : base(
            name: "樵夫",
            hp: 100,
            mp: 50,
            attack: 40,
            defense: 25,
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
