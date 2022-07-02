using HealthSystem;
using UnityEngine;

namespace Testing
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class DamageArea : MonoBehaviour
    {
        [SerializeField] private int damage = 1;
        private void OnTriggerEnter2D(Collider2D col)
        {
            if(col.TryGetComponent(out IHealth h))
                h.GetHealth().ApplyDamage(damage);
        }
    }
}