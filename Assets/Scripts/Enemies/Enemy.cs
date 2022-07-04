using System;
using UnityEngine;
using Utils;

namespace Enemies
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Enemy : HealthComponent
    {
        private Transform _target;
        private Rigidbody2D _rb2d;
        public bool DestroyOnOutOfBounds => destroyOnOutOfBounds;
        public int ScoreValue => scoreValue;
        [SerializeField] private float speed = 2;
        [SerializeField] private float rotationSpeed = 1;
        [SerializeField] private int scoreValue = 1;
        [SerializeField] private bool destroyOnOutOfBounds = true;
        [SerializeField] private Transform hull;

        private void Start()
        {
            _rb2d = GetComponent<Rigidbody2D>();
        }

        public void SetTarget(Transform target)
        {
            _target = target;
        }

        private Vector2 DirectionToTarget()
        {
            return (_target.position - transform.position).normalized;
        }

        private void Update()
        {
            if (!_target) return;
            Vector2 direction = Vector3.RotateTowards(_rb2d.velocity.normalized, DirectionToTarget(), Time.deltaTime * rotationSpeed,
                Time.deltaTime * rotationSpeed);
            _rb2d.velocity = direction * speed;
            if (hull)
                hull.right = _rb2d.velocity.normalized;
        }
    }
}