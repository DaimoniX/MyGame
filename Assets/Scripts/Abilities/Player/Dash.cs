using System;
using Player;

namespace Abilities.Player
{
    public class Dash : PlayerAbility
    {
        private readonly float _dashSpeed;
        
        public Dash(Player2D player, float dashSpeed, float cooldown) : base(player, cooldown)
        {
            if (dashSpeed <= 0)
                throw new ArgumentException("Dash speed cannot be <= 0");
            _dashSpeed = dashSpeed;
        }

        protected override void OnActivation()
        {
            Player2D.Rigidbody.velocity = Player2D.Input * (Player2D.Movement.Speed * _dashSpeed);
        }
    }
}