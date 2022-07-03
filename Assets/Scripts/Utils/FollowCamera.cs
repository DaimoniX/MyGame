using UnityEngine;

namespace Utils
{
    [RequireComponent(typeof(Camera))]
    public class FollowCamera : MonoBehaviour
    {
        private Transform _camera;
        [SerializeField] private float speed = 2;
        [SerializeField] private Transform target;

        private void Start()
        {
            _camera = GetComponent<Camera>().transform;
        }

        private void FixedUpdate()
        {
            Vector3 targetPos = target.position;
            targetPos.z = _camera.position.z;
            _camera.position = Vector3.Lerp(_camera.position, targetPos, Time.fixedDeltaTime * speed);
        }
    }
}