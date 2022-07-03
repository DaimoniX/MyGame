using HealthSystem;
using UnityEngine;

namespace Projectiles
{
    [RequireComponent(typeof(Rigidbody2D),typeof(Collider2D))]
    public class BasicProjectile : Projectile2D
    {
        [SerializeField] protected int damage = 1;
        [SerializeField] protected new Rigidbody2D rigidbody;
        [SerializeField] protected float speed = 1;
        [SerializeField] protected ParticleSystem hitParticles;
        protected float SpeedMultiplier = 1;
        private const float MaxLifetime = 20;

        public override void Destroy()
        {
            if (hitParticles)
                Instantiate(hitParticles, transform.position, transform.rotation);
            base.Destroy();
        }

        private void Start()
        {
            Invoke(nameof(Destroy), MaxLifetime);
        }

        public BasicProjectile SetSpeedMultiplier(float multiplier)
        {
            SpeedMultiplier = multiplier;
            return this;
        }
        
        public override void Launch(Vector2 direction)
        {
            rigidbody.velocity = direction * (speed * SpeedMultiplier);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if(col.TryGetComponent(out IDamageable damageable))
                damageable.ApplyDamage(damage);
            Destroy();
        }
    }
}