namespace rpg;

public class Scholar : Player
{
    public Scholar (int level, CurrentType type, Personality nature) 
        : base(
            name: "學者",
            hp: 80,
            mp: 70,
            attack: 15,
            defense: 20,
            speed: 15,
            magicAttack: 50,
            magicDefense: 30,
            level: level,
            status: CurrentStatus.Normal,
            type: type,
            nature: nature
        )
    {
    }    
}
