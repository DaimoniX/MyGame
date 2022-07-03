using UnityEngine;

namespace Projectiles
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class HomingProjectile : BasicProjectile
    {
        [SerializeField] private float rotationSpeed = 1;
        private Transform _target;
        private Vector2 _dir;

        public override void Launch(Vector2 direction)
        {
            throw new System.NotSupportedException();
        }

        public override void Launch(Transform target)
        {
            _target = target;
            rigidbody.velocity = Vector2.right * (speed * SpeedMultiplier);
        }


        private void Update()
        {
            _dir = _target.position - transform.position;
            _dir.Normalize();
            transform.right = Vector3.Lerp(transform.right, _dir, Time.deltaTime * rotationSpeed * SpeedMultiplier);
            rigidbody.velocity = transform.right * (speed * SpeedMultiplier);
        }
    }
}