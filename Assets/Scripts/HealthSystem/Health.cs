using System;
using UnityEngine.Events;

namespace HealthSystem
{
    [Serializable]
    public class Health : IHealth
    {
        public int MaxHealth { get; private set; }
        public int CurrentHealth { get; private set; }
        public float HealthPercent => (float)CurrentHealth / MaxHealth;
        public UnityEvent<int> onDamage  = new();
        public UnityEvent<int> onHeal  = new();
        public UnityEvent onDeath = new();

        public Health(int maxHealth)
        {
            if (maxHealth <= 0)
                throw new ArgumentException("Max health cannot be <= 0");
            CurrentHealth = MaxHealth = maxHealth;
        }

        public void SetMaxHealth(int value)
        {
            if(value <= 0)
                throw new ArgumentException("Max health cannot be <= 0");
            CurrentHealth = MaxHealth = value;
        }
        
        public void SetHealth(int value)
        {
            CurrentHealth = Math.Clamp(value, 0, MaxHealth);
        }

        public void ApplyDamage(int value)
        {
            SetHealth(CurrentHealth - value);
            onDamage.Invoke(value);
            if(CurrentHealth == 0)
                onDeath.Invoke();
        }

        public void ApplyHeal(int value)
        {
            SetHealth(CurrentHealth + value);
            onHeal.Invoke(value);
        }

        public Health GetHealth()
        {
            return this;
        }
    }
}