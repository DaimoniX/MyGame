using System;
using UnityEngine;

namespace Player
{
    public class PlayerMovement2D
    {
        private readonly Rigidbody2D _rigidbody2D;

        private float _speed;
        public float Speed
        {
            set => _speed = Math.Clamp(value, 0, 100);
            get => _speed;
        }

        public PlayerMovement2D(Rigidbody2D rigidbody2D)
        {
            _rigidbody2D = rigidbody2D;
            Speed = 1;
        }

        public void Move(Vector2 input)
        {
            _rigidbody2D.velocity = Vector2.Lerp(_rigidbody2D.velocity, input * Speed, Time.deltaTime * Speed * 2);
        }
    }
}