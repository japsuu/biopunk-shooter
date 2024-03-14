using UnityEngine;

namespace Entities
{
    /// <summary>
    /// Rotates the character back and forth to create a wiggle effect.
    /// </summary>
    public class WiggleRun : MonoBehaviour
    {
        [SerializeField]
        private Transform _transform;
        
        [SerializeField]
        private float _wiggleDegrees = 10f;
        
        [SerializeField]
        private float _wiggleSpeed = 1f;
        
        private float _wiggleTimer;
        
        
        public void UpdateWiggle()
        {
            _wiggleTimer += Time.deltaTime * _wiggleSpeed;
            _transform.localRotation = Quaternion.Euler(0f, 0f, Mathf.Sin(_wiggleTimer) * _wiggleDegrees);
        }
    }
}