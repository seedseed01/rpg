namespace rpg;

public class Thief : Player
{
    public Thief (int level, CurrentType type, Personality nature) 
        : base(
            name: "扒手",
            hp: 80,
            mp: 70,
            attack: 20,
            defense: 15,
            speed: 35,
            magicAttack: 20,
            magicDefense: 50,
            level: level,
            status: CurrentStatus.Normal,
            type: type,
            nature: nature
        )
    {
    }    
}
