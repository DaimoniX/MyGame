namespace HealthSystem
{
    public interface IHealth : IDamageable, IHealable
    {
        public Health GetHealth();
    }
}