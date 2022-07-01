using System;
using Abilities.Player;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Player2D : MonoBehaviour
    {
        private PlayerAbility _ability;
        public Rigidbody2D Rigidbody { private set; get; }
        private Vector2 _input;
        public Vector2 Input => _input;
        private float _speed;

        private void Start()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
            _input = Vector2.zero;
            _speed = 4;
            _ability = new Blink(this, 3 ,1);
        }

        private void Update()
        {
            _ability.Update(Time.deltaTime);
            _input.x = UnityEngine.Input.GetAxisRaw("Horizontal");
            _input.y = UnityEngine.Input.GetAxisRaw("Vertical");
            _input.Normalize();
            Rigidbody.velocity = Vector2.Lerp(Rigidbody.velocity, _input * _speed, Time.deltaTime * _speed);
            if (UnityEngine.Input.GetKey(KeyCode.F))
                _ability.Activate();
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (_ability != null)
            {
                UnityEditor.Handles.color = GUI.color = Color.red;
                UnityEditor.Handles.Label(transform.position,
                    $"{_ability}\nCD: {_ability.Cooldown} T: {_ability.Timer}\nCooldown: {(1f - _ability.CooldownPercent) * 100f}%\n{(_ability.IsReady ? "Ready" : "Not Ready")}");
            }
        }
#endif
    }
}