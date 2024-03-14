using System;
using UnityEngine;

namespace World
{
    public class RotateAxis : MonoBehaviour
    {
        private enum RotateAroundAxis
        {
            X,
            Y,
            Z
        }

        [SerializeField]
        private RotateAroundAxis _axis;
        
        [SerializeField]
        private float _rotationSpeed = 10f;
        
        
        private void Update()
        {
            switch (_axis)
            {
                case RotateAroundAxis.X:
                    transform.Rotate(_rotationSpeed * Time.deltaTime, 0f, 0f);
                    break;
                case RotateAroundAxis.Y:
                    transform.Rotate(0f, _rotationSpeed * Time.deltaTime, 0f);
                    break;
                case RotateAroundAxis.Z:
                    transform.Rotate(0f, 0f, _rotationSpeed * Time.deltaTime);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}