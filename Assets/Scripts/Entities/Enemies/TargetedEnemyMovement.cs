using Entities.Player;
using UnityEngine;

namespace Entities.Enemies
{
    /// <summary>
    /// Always moves towards the player.
    /// </summary>
    public class TargetedEnemyMovement : MonoBehaviour
    {
        [SerializeField]
        private float _movementSpeed = 5;

        private Rigidbody2D _rb;
        private Vector2 _movementDirection;
        private Enemy _enemy;
        private Transform _playerTransform;
        
        
        private void Awake()
        {
            _enemy = GetComponent<Enemy>();
            _rb = GetComponent<Rigidbody2D>();
        }
        
        
        private void Start()
        {
            _playerTransform = PlayerController.Instance.transform;
        }
        
        
        private void Update()
        {
            _movementDirection = (_playerTransform.position - transform.position).normalized;
        }
        
        
        private void FixedUpdate()
        {
            _rb.velocity = _movementDirection * _movementSpeed;
        }
    }
}