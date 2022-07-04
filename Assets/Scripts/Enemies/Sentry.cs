using Projectiles;
using UnityEngine;

namespace Enemies
{
    public class Sentry : MonoBehaviour
    {
        private float _timer = 0;
        [SerializeField] private Transform muzzle;
        [SerializeField] private float shootingInterval = 1;
        [SerializeField] private float aimingSpeed = 1;
        [SerializeField] private float projectileSpeedMultiplier = 1;
        [SerializeField] private float radius = 3;
        [SerializeField] private float maxAimingDistance = 0.15f;
        [SerializeField] private BasicProjectile projectile;
        private Transform _target;
        private Vector3 _dir;
        private CircleCollider2D _sensor;
        
        private void Start()
        {
            if(!transform.TryGetComponent(out _sensor))
                _sensor = gameObject.AddComponent<CircleCollider2D>();
            _sensor.radius = radius;
            _sensor.isTrigger = true;
        }

        private void Update()
        {
            if (_target)
            {
                _dir = _target.position - transform.position;
                _dir.Normalize();
                if (_timer < 0 && Vector3.Distance(transform.right, _dir) < maxAimingDistance)
                {
                    Instantiate(projectile, muzzle.position, muzzle.rotation).SetSpeedMultiplier(projectileSpeedMultiplier)
                        .Launch(_target);
                    _timer = shootingInterval;
                }

                transform.right = Vector3.RotateTowards(transform.right, _dir, Time.deltaTime * aimingSpeed,
                    Time.deltaTime * aimingSpeed);
                _timer -= Time.deltaTime;
            }
            else
                _timer = 0;
            _sensor.radius = radius;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!_target)
                _target = col.transform;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.transform == _target)
                _target = null;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, transform.right);
            Gizmos.DrawWireSphere(transform.position, radius);
            if (_target)
                Gizmos.DrawLine(transform.position, _target.position);
            UnityEditor.Handles.color = GUI.color = Color.red;
            UnityEditor.Handles.Label(transform.position,
                $"{_timer}");
        }
#endif
    }
}