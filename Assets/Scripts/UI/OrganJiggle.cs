using UnityEngine;

namespace UI
{
    public class OrganJiggle : MonoBehaviour
    {
        [SerializeField]
        private DefomableImage _organImage;

        [Tooltip("These four transforms are attached with springs to the center of the image, and are used to as position targets for the mesh corners.")]
        [SerializeField]
        private Transform[] _meshCornerTargetTransforms;

        private void Start()
        {
            _organImage.Initialize(4);
        }


        private void Update()
        {
            UpdateMeshCorners();
        }

        private void UpdateMeshCorners()
        {
            // Loop through each corner target transform
            for (int i = 0; i < 4; i++)
            {
                // Calculate the position of the target transform relative to the image's transform
                Vector3 relativePosition = _organImage.transform.InverseTransformPoint(_meshCornerTargetTransforms[i].position);

                // Assign this relative position to the corresponding vertex
                _organImage.SetVertexOffset(i, relativePosition);
            }
        }
    }
}