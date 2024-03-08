using Singletons;
using UnityEngine;

namespace World
{
    public class GameWorld : SingletonBehaviour<GameWorld>
    {
        [SerializeField]
        private float _radius = 50f;

        [SerializeField]
        private Transform _floorTransform;
        
        public float Radius => _radius;


        private void OnValidate()
        {
            if (_floorTransform != null)
            {
                _floorTransform.localScale = new Vector3(_radius * 2f, _radius * 2f, 1f);
            }
        
        }


        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(Vector3.zero, _radius);
        }
    }
}