using UnityEngine;

namespace Projectiles
{
    public abstract class Projectile2D : MonoBehaviour
    {
        public abstract void Launch(Vector2 direction);

        public virtual void Launch(Transform target)
        {
            Launch((target.position - transform.position).normalized);
        }

        public virtual void Destroy()
        {
            Destroy(gameObject);
        }
    }
}