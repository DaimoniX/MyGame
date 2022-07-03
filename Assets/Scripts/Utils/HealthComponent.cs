using HealthSystem;
using UnityEngine;

namespace Utils
{
    public class HealthComponent : MonoBehaviour, IHealth, IDamageable, IHealable
    {
        [SerializeField] private Health health = new(10);
        [SerializeField] private int initialHealth = 10;

        private void Awake()
        {
            health.SetMaxHealth(initialHealth);
        }

        public void ApplyDamage(int value)
        {
            health.ApplyDamage(value);
        }

        public void ApplyHeal(int value)
        {
            health.ApplyHeal(value);
        }

        public Health GetHealth()
        {
            return health;
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            UnityEditor.Handles.color = GUI.color = Color.red;
            UnityEditor.Handles.Label(transform.position,
                $"Health: {health.CurrentHealth}/{health.MaxHealth} ({health.HealthPercent * 100}%)");
        }
#endif
    }
}