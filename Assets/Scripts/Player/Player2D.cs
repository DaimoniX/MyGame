using Abilities.Player;
using HealthSystem;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Player2D : MonoBehaviour, IHealth
    {
        private PlayerAbility _ability;
        public PlayerAbility Ability => _ability;
        [SerializeField] private PlayerMovement2D movement = new();
        public PlayerMovement2D Movement => movement;
        [SerializeField] private PlayerShooter2D shooter = new();
        public PlayerShooter2D Shooter => shooter;
        public Rigidbody2D Rigidbody { private set; get; }
        private Vector2 _input;
        public Vector2 Input => _input;
        [Header("Health")]
        [SerializeField] private int maxHealth = 10;
        [SerializeField] private Health health = new();
        private Camera _camera;
        private Vector2 _aimingDirection;

        private void Start()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
            _input = Vector2.zero;
            _aimingDirection = Vector2.zero;
            movement.SetRigidbody(Rigidbody);
            _ability = new Dash(this, 6, 1);
            _camera = Camera.main;
            health.SetMaxHealth(maxHealth);
        }

        private void Update()
        {
            // Get user input
            _input.x = UnityEngine.Input.GetAxisRaw("Horizontal");
            _input.y = UnityEngine.Input.GetAxisRaw("Vertical");
            _input.Normalize();
            // Set aiming direction
            _aimingDirection = _camera.ScreenToWorldPoint(UnityEngine.Input.mousePosition) - transform.position;
            _aimingDirection.Normalize();
            // Move player
            movement.Move(_input);
            // Update and perform ability
            if (UnityEngine.Input.GetButton("Jump"))
                _ability.Activate();
            _ability.Update(Time.deltaTime);
            // Shoot
            shooter.SetDirection(_aimingDirection);
            if (UnityEngine.Input.GetButton("Fire1"))
                shooter.Shoot();
            shooter.Update(Time.deltaTime);
        }

        public void SetAbility(PlayerAbility ability)
        {
            _ability = ability;
        }

        public void SpawnParticles(ParticleSystem particleSystem)
        {
            Instantiate(particleSystem, transform.position, transform.rotation);
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }

        #region IHealth

        public Health GetHealth()
        {
            return health;
        }

        public void ApplyDamage(int value)
        {
            health.ApplyDamage(value);
        }

        public void ApplyHeal(int value)
        {
            health.ApplyHeal(value);
        }

        #endregion

        #region DEBUG

#if UNITY_EDITOR
        private readonly Vector3 _debugOffset = new(-1, 1, 0);
        private void OnDrawGizmos()
        {
            if (_ability == null) return;
            Gizmos.DrawRay(transform.position, _aimingDirection);
            UnityEditor.Handles.color = GUI.color = Color.red;
            UnityEditor.Handles.Label(transform.position + _debugOffset,
                $"Health: {health.CurrentHealth}/{health.MaxHealth} ({health.HealthPercent * 100}%)\n" +
                $"Reload: {(int)(shooter.ReloadPercent * 100)}%\n" +
                $"{_ability}\n" +
                $"CD: {_ability.Cooldown} T: {_ability.Timer}\n" +
                $"Cooldown: {(1f - _ability.CooldownPercent) * 100f}%\n" +
                $"Ability {(_ability.IsReady ? "ready" : "not ready")}");
        }
#endif

        #endregion
    }
}