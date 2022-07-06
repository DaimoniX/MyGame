using System;
using System.Collections.Generic;
using System.Linq;
using Projectiles;
using UnityEngine;
using UnityEngine.Serialization;

namespace Enemies
{
    public class Sentry : MonoBehaviour
    {
        private float _timer = 0;
        [FormerlySerializedAs("muzzle")] [SerializeField] private Transform[] muzzles;
        [SerializeField] private float shootingInterval = 1;
        [Range(0, 10)] [SerializeField] private float aimingSpeed = 1;
        [SerializeField] private float projectileSpeedMultiplier = 1;
        [Range(0, 100)] [SerializeField] private float radius = 3;
        [Range(0, 360)] [SerializeField] private int shootingArc = 360;
        [SerializeField] private BasicProjectile projectile;
        private Transform _target;
        private readonly List<Transform> _possibleTargets = new();
        private Vector3 _dir;
        private CircleCollider2D _sensor;

        private void Start()
        {
            if (!transform.TryGetComponent(out _sensor))
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

                if (_timer < 0 && Vector3.Angle(transform.right, _dir) < shootingArc)
                {
                    foreach (var muzzle in muzzles)
                    {
                        Instantiate(projectile, muzzle.position, muzzle.rotation)
                            .SetSpeedMultiplier(projectileSpeedMultiplier)
                            .Launch(_target);
                    }
                    _timer = shootingInterval;
                }

                transform.right = Vector3.RotateTowards(transform.right, _dir, Time.deltaTime * aimingSpeed,
                    Time.deltaTime * aimingSpeed);
                _timer -= Time.deltaTime;
            }
            else if (_possibleTargets.Count > 0)
            {
                _target = _possibleTargets.First();
            }
            else
            {
                _timer = 0;
            }

            _sensor.radius = radius;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!_target)
                _target = col.transform;
            else
                _possibleTargets.Add(col.transform);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.transform.Equals(_target))
                _target = null;
            if (_possibleTargets.Contains(other.transform))
                _possibleTargets.Remove(other.transform);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            var position = transform.position;
            Gizmos.color = Color.red;
            Gizmos.DrawRay(position, transform.right * radius);
            if (aimingSpeed > 0)
                Gizmos.DrawWireSphere(position, radius * 0.99f);
            if (_target)
                Gizmos.DrawLine(position, _target.position);
            Gizmos.color = UnityEditor.Handles.color = GUI.color = Color.magenta;
            UnityEditor.Handles.Label(position,
                $"{_timer}");
            if(Application.isPlaying) return;
            Vector3 arcStart = new Vector3(position.x + radius * Mathf.Cos(shootingArc / 2f * Mathf.Deg2Rad),
                -(position.y + radius * Mathf.Sin(shootingArc / 2f * Mathf.Deg2Rad)), position.z);
            Gizmos.DrawRay(position, arcStart);
            UnityEditor.Handles.DrawWireArc(position, transform.forward, arcStart, shootingArc, radius);
            arcStart.y *= -1;
            Gizmos.DrawRay(position, arcStart);
        }
#endif
    }
}