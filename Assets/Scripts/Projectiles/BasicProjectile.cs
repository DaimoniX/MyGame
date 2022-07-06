using HealthSystem;
using Player;
using UnityEngine;

namespace Projectiles
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class BasicProjectile : Projectile2D
    {
        [SerializeField] protected int damage = 1;
        [SerializeField] protected new Rigidbody2D rigidbody;
        [SerializeField] protected float speed = 1;
        [SerializeField] protected ParticleSystem hitParticles;
        [SerializeField] protected AudioClip shootSound;
        [SerializeField] protected AudioClip hitSound;
        [Range(0, 0.3f)] [SerializeField] protected float spread = 0;

        protected float SpeedMultiplier = 1;
        private const float MaxLifetime = 20;

        public override void Destroy()
        {
            if (hitParticles)
                Instantiate(hitParticles, transform.position, transform.rotation);
            if (hitSound)
                AudioSource.PlayClipAtPoint(hitSound, transform.position, PlayerData.AudioVolume);
            base.Destroy();
        }

        protected virtual void TimeoutDestroy()
        {
            Destroy(gameObject);
        }

        private void Start()
        {
            if (shootSound)
                AudioSource.PlayClipAtPoint(shootSound, transform.position);
            Invoke(nameof(TimeoutDestroy), MaxLifetime);
        }

        public BasicProjectile SetSpeedMultiplier(float multiplier)
        {
            SpeedMultiplier = multiplier;
            return this;
        }

        public override void Launch(Vector2 direction)
        {
            if (spread > 0)
            {
                direction.x += Random.Range(-spread, spread);
                direction.y += Random.Range(-spread, spread);
                direction.Normalize();
            }

            rigidbody.velocity = direction * (speed * SpeedMultiplier);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.TryGetComponent(out IDamageable damageable))
                damageable.ApplyDamage(damage);
            Destroy();
        }
    }
}