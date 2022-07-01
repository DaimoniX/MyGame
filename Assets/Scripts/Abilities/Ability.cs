namespace Abilities
{
    public abstract class Ability
    {
        public float Timer { private set; get; }
        public readonly float Cooldown;

        public bool IsReady => Timer <= 0;
        public float CooldownPercent => Timer / Cooldown;

        protected Ability(float cooldown)
        {
            Cooldown = cooldown;
            Timer = cooldown;
        }

        public void Update(float deltaTime)
        {
            if(!IsReady)
                Timer -= deltaTime;
            else
                Timer = 0;
        }

        public void Activate()
        {
            if(!IsReady)
                return;
            OnActivation();
            Timer = Cooldown;
        }

        protected abstract void OnActivation();
    }
}