using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class DefomableImage : Image
    {
        private readonly List<Vector3> _vertexOffsets = new();
        
        
        public void Initialize(int vertexCount)
        {
            _vertexOffsets.Clear();
            for (int i = 0; i < vertexCount; i++)
                _vertexOffsets.Add(Vector3.zero);
        }
        
        
        public void SetVertexOffset(int index, Vector3 offset)
        {
            _vertexOffsets[index] = offset;
        }
        
        
        protected override void OnPopulateMesh(VertexHelper vh)
        {
            base.OnPopulateMesh(vh);

            for (int i = 0; i < vh.currentVertCount; i++)
            {
                UIVertex vert = UIVertex.simpleVert;
                vh.PopulateUIVertex(ref vert, i);
                Vector3 position = vert.position;

                //if (i < _vertexOffsets.Count)
                //    position += _vertexOffsets[i];

                vert.position = position;
                vh.SetUIVertex(vert, i);
            }
        }
    }
}