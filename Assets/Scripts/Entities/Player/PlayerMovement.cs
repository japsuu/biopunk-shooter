using System;
using UnityEngine;
using World;

namespace Entities.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement : MonoBehaviour
    {
        private enum MovementMethod
        {
            Force,
            Velocity,
            MovePosition
        }
        
        [SerializeField]
        private WiggleRun _wiggleRun;

        [HideInInspector]
        public float MovementSpeed;

        [SerializeField]
        private MovementMethod _movementMethod;

        private Vector2 _input;
        
        public bool CanMove { get; set; } = true;
        public Rigidbody2D Rb { get; private set; }


        private void Awake()
        {
            Rb = GetComponent<Rigidbody2D>();
            Rb.angularDrag = 0f;
            Rb.gravityScale = 0f;
        }


        private void Update()
        {
            if (!CanMove)
            {
                _input = Vector2.zero;
                return;
            }
            
            _input.x = Input.GetAxisRaw("Horizontal");
            _input.y = Input.GetAxisRaw("Vertical");

            if (_input.magnitude > 0.2f)
            {
                _wiggleRun.UpdateWiggle();
            }
        }


        private void FixedUpdate()
        {
            switch (_movementMethod)
            {
                case MovementMethod.Force:
                    Rb.AddForce(_input.normalized * MovementSpeed);
                    break;
                case MovementMethod.Velocity:
                    Rb.velocity = _input.normalized * MovementSpeed;
                    break;
                case MovementMethod.MovePosition:
                    Rb.MovePosition(Rb.position + _input.normalized * (MovementSpeed * Time.fixedDeltaTime));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


        private void LateUpdate()
        {
            ClampToCircle();
        }


        private void ClampToCircle()
        {
            float worldRadius = GameWorld.Instance.Radius;
            Vector3 currentPos = transform.position;

            if (currentPos.magnitude > worldRadius)
                transform.position = currentPos.normalized * worldRadius;
        }


        private void OnDisable()
        {
            _input = Vector2.zero;
        }
    }
}