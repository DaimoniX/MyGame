using UnityEngine;

namespace Utils
{
    [RequireComponent(typeof(Camera))]
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] private float speed = 2;
        [SerializeField] private Transform target;

        private void FixedUpdate()
        {
            if(!target) return;
            Vector3 targetPos = target.position;
            targetPos.z = transform.position.z;
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.fixedDeltaTime * speed);
        }
    }
}