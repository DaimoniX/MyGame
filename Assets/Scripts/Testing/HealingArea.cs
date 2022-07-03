using HealthSystem;
using UnityEngine;

namespace Testing
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class HealingArea : MonoBehaviour
    {
        [SerializeField] private int heal = 1;
        private void OnTriggerEnter2D(Collider2D col)
        {
            if(col.TryGetComponent(out IHealable h))
                h.ApplyHeal(heal);
        }
    }
}