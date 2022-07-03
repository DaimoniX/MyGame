using System;
using Projectiles;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Player
{
    [Serializable]
    public class PlayerShooter2D
    {
        private float _timer = 0;
        public float ReloadPercent => 1 - _timer;
        [SerializeField] private Projectile2D projectile2D;
        [SerializeField] private Transform muzzle;
        [SerializeField] private float fireRate = 1;

        public void SetDirection(Vector2 direction)
        {
            muzzle.right = direction;
        }

        public void Update(float deltaTime)
        {
            _timer = Math.Max(_timer - deltaTime * fireRate, 0);
        }

        public void Shoot()
        {
            if (_timer > 0) return;
            Object.Instantiate(projectile2D, muzzle.position, muzzle.rotation).Launch(muzzle.right);
            _timer = 1;
        }
    }
}