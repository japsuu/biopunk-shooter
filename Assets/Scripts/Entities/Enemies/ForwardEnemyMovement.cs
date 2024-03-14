using UnityEngine;

namespace Entities.Enemies
{
    /// <summary>
    /// Always moves forward relative to some transform.
    /// </summary>
    public class ForwardEnemyMovement : MonoBehaviour
    {
        [SerializeField]
        private Transform _forwardReferenceTransform;
        
        [SerializeField]
        private float _movementSpeed = 5;
        
        private Rigidbody2D _rb;
        private Vector2 _movementDirection;
        
        
        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }
        
        
        private void FixedUpdate()
        {
            _movementDirection = _forwardReferenceTransform.right;
            _rb.velocity = _movementDirection * _movementSpeed;
        }
    }
}