using HealthSystem;
using UnityEngine;
using UnityEngine.Serialization;

namespace Utils
{
    public class HealthComponent : MonoBehaviour, IHealth, IDamageable, IHealable
    {
        [SerializeField] private Health health = new();
        [FormerlySerializedAs("initialHealth")] [SerializeField] private int maxHealth = 10;

        private void Awake()
        {
            health.SetMaxHealth(maxHealth);
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

        public void SpawnObject(GameObject gameObject)
        {
            Instantiate(gameObject, transform.position, transform.rotation);
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