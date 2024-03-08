using Entities.Player;
using UnityEngine;
using World;
using Random = UnityEngine.Random;

namespace Entities.Enemies
{
    /// <summary>
    /// Periodically selects a random point near the player, and moves towards that point.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class EnemyMovement : MonoBehaviour
    {
        private const float MAX_PATHING_TIME = 5f;
        
        private Rigidbody2D _rb;
        private Vector2 _targetPosition;
        private Vector2 _movementDirection;
        
        [HideInInspector]
        public float MovementSpeed = 5;
        
        private float _pathingTimer;


        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }


        private void Start()
        {
            DetermineTargetPosition();
        }


        private void Update()
        {
            bool isStuck = _pathingTimer >= MAX_PATHING_TIME;
            bool hasReachedTarget = Vector2.Distance(transform.position, _targetPosition) < 1;
            
            // If close enough to the target position, select a new target position.
            if (isStuck || hasReachedTarget)
                DetermineTargetPosition();
            
            _movementDirection = (_targetPosition - (Vector2) transform.position).normalized;
            _pathingTimer += Time.deltaTime;
        }


        private void DetermineTargetPosition()
        {
            Vector2 playerPos = PlayerController.Instance.transform.position;
            Vector2 position = playerPos + Random.insideUnitCircle * 15;
            position = ClampInsideWorld(position);
            
            // Ensure the position is at least 3 units away from the player.
            if (Vector2.Distance(playerPos, position) < 4)
                position = Random.insideUnitCircle.normalized * 4;
            
            _targetPosition = position;
            _pathingTimer = 0;
        }


        private void FixedUpdate()
        {
            _rb.velocity = _movementDirection * MovementSpeed;
        }


        private static Vector2 ClampInsideWorld(Vector2 position)
        {
            float worldRadius = GameWorld.Instance.Radius;

            if (position.magnitude > worldRadius)
                position = position.normalized * worldRadius;
            
            return position;
        }


        private void OnDrawGizmos()
        {
            // Draw line from enemy to target position.
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, _targetPosition);
        }
    }
}