using System;
using UnityEngine;

namespace Player
{
    [Serializable]
    public class PlayerMovement2D
    {
        private Rigidbody2D _rigidbody2D;
        [SerializeField] private float speed;

        public float Speed
        {
            set => speed = Math.Clamp(value, 0, 100);
            get => speed;
        }

        public void SetRigidbody(Rigidbody2D rigidbody2D)
        {
            _rigidbody2D = rigidbody2D;
        }

        public void Move(Vector2 input)
        {
            _rigidbody2D.velocity = Vector2.Lerp(_rigidbody2D.velocity, input * Speed, Time.deltaTime * Speed * 2);
        }
    }
}