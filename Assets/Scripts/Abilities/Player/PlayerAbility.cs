using Player;

namespace Abilities.Player
{
    public abstract class PlayerAbility : Ability
    {
        protected readonly Player2D Player2D; 
        
        protected PlayerAbility(Player2D player2D, float cooldown) : base(cooldown)
        {
            Player2D = player2D;
        }
    }
}