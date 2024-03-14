using UnityEngine;

namespace Entities
{
    public abstract class EntityRotation : MonoBehaviour
    {
        private enum RotationMethod
        {
            RotateTowards,
            Direct,
            Lerp
        }
        
        [SerializeField]
        private Transform _rotationTransform;
        
        [SerializeField]
        private RotationMethod _rotationMethod = RotationMethod.RotateTowards;
        
        [SerializeField]
        private float _rotationSpeed = 360;
        
        protected abstract Vector2 RotateTowardsPosition { get; }
        
        
        protected virtual void Update()
        {
            Vector2 direction = (RotateTowardsPosition - (Vector2)transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
            switch (_rotationMethod)
            {
                case RotationMethod.Direct:
                    _rotationTransform.eulerAngles = new Vector3(0, 0, angle);
                    break;
                case RotationMethod.RotateTowards:
                    _rotationTransform.rotation = Quaternion.RotateTowards(_rotationTransform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
                    break;
                case RotationMethod.Lerp:
                    _rotationTransform.rotation = Quaternion.Lerp(_rotationTransform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
                    break;
                default:
                    throw new System.ArgumentOutOfRangeException();
            }
        }
    }
}