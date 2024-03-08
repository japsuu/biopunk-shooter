using Singletons;
using UnityEngine;

namespace Camera
{
    /// <summary>
    /// Makes the camera follow the player, slightly zoom in if the player is shooting, and move a bit towards the mouse position.
    /// </summary>
    public class CameraController : SingletonBehaviour<CameraController>
    {
        [SerializeField]
        private UnityEngine.Camera _camera;
        
        [SerializeField]
        private Transform _player;
        
        [SerializeField]
        private float _maxDistanceFromPlayer = 10f;
        
        public UnityEngine.Camera Camera => _camera;


        private void Update()
        {
            // Move the midpoint to between the player and the mouse position.
            Vector3 playerPosition = _player.position;
            Vector3 mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 targetPos = (playerPosition + mousePosition) / 2f;
            
            // Clamp the target position to a certain distance from the player.
            Vector3 direction = targetPos - playerPosition;
            if (direction.magnitude > _maxDistanceFromPlayer)
            {
                direction = direction.normalized * _maxDistanceFromPlayer;
                targetPos = playerPosition + direction;
            }
            
            transform.position = targetPos;
        }


        private void Reset()
        {
            UnityEngine.Camera[] cameras = FindObjectsOfType<UnityEngine.Camera>();
            switch (cameras.Length)
            {
                case > 1:
                    Debug.LogError("There are multiple cameras in the scene. Please assign the correct camera to the CameraController.");
                    return;
                case 0:
                    Debug.LogError("There are no cameras in the scene. Please add a camera to the scene.");
                    return;
                default:
                    _camera = cameras[0];
                    break;
            }
        }


        private void OnDrawGizmosSelected()
        {
            if (_player == null) return;
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_player.position, _maxDistanceFromPlayer);
        }
    }
}