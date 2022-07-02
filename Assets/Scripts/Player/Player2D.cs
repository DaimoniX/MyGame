using Abilities.Player;
using HealthSystem;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Player2D : MonoBehaviour, IHealth
    {
        private PlayerAbility _ability;
        public PlayerMovement2D Movement { private set; get; }
        public Rigidbody2D Rigidbody { private set; get; }
        private Vector2 _input;
        public Vector2 Input => _input;
        [SerializeField]
        private Health health = new(20);

        private void Start()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
            _input = Vector2.zero;
            Movement = new PlayerMovement2D(Rigidbody)
            {
                Speed = 4
            };
            _ability = new Dash(this, 6, 1);
        }

        private void Update()
        {
            // Get user input
            _input.x = UnityEngine.Input.GetAxisRaw("Horizontal");
            _input.y = UnityEngine.Input.GetAxisRaw("Vertical");
            _input.Normalize();
            // Move player
            Movement.Move(_input);
            // Update and perform ability
            if (UnityEngine.Input.GetKey(KeyCode.F))
                _ability.Activate();
            _ability.Update(Time.deltaTime);
        }

        public void SetAbility(PlayerAbility ability)
        {
            _ability = ability;
        }

#if UNITY_EDITOR
        private readonly Vector3 _debugOffset = new(-1, 1, 0);
        private void OnDrawGizmos()
        {
            if (_ability == null) return;
            UnityEditor.Handles.color = GUI.color = Color.red;
            UnityEditor.Handles.Label(transform.position + _debugOffset,
                $"Health: {health.CurrentHealth}/{health.MaxHealth} ({health.HealthPercent * 100}%)\n" +
                $"{_ability}\n" +
                $"CD: {_ability.Cooldown} T: {_ability.Timer}\n" +
                $"Cooldown: {(1f - _ability.CooldownPercent) * 100f}%\n" +
                $"{(_ability.IsReady ? "Ready" : "Not Ready")}");
        }
#endif
        public Health GetHealth()
        {
            return health;
        }
    }
}