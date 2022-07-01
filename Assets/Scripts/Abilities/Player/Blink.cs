using System;
using Player;
using UnityEngine;

namespace Abilities.Player
{
    public class Blink : PlayerAbility
    {
        private readonly float _blinkDistance;
        
        public Blink(Player2D player, float blinkDistance, float cooldown) : base(player, cooldown)
        {
            if (blinkDistance <= 0)
                throw new ArgumentException("Blink distance cannot be <= 0");
            _blinkDistance = blinkDistance;
        }
        
        protected override void OnActivation()
        {
            Player2D.Rigidbody.MovePosition((Vector2)Player2D.transform.position + Player2D.Input * _blinkDistance);
        }
    }
}