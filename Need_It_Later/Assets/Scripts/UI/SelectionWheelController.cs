using UnityEngine;

/*
Modified from code by Unity user "image28", which was modified from original code by Unity user "JFo"
https://forum.unity.com/threads/create-a-pie-chart-in-unity.174529
*/

namespace UI
{
    public class SelectionWheelController : MonoBehaviour
    {
        public float delay = 0.00f;
        public Material mainMaterial;
        public Material[] materials;
        public int segments;
        public float radius;
        private SelectionWheelMesh _selectionWheelMesh;
        private float[] _mData;

        private void Start()
        {
            materials = new Material[segments];
            for (var i = 0; i < segments; i++)
            {
                /*var color =
                    i == 0 ? new Color32(255, 255, 255, 255) :
                    materials[i - 1].color == new Color32(255, 255, 255, 255) ? new Color32(220, 220, 220, 255) :
                    new Color32(255, 255, 255, 255);*/
                var color = Color.clear;

                materials[i] = new Material(mainMaterial)
                {
                    color = color
                };
            }

            _selectionWheelMesh = gameObject.AddComponent<SelectionWheelMesh>();
            if (_selectionWheelMesh != null)
            {
                _selectionWheelMesh.Init(_mData, 100, 0, 100, materials, delay);
                _mData = AssignLengths(segments);
                _selectionWheelMesh.Draw(_mData, radius);
            }
        }


        private float[] AssignLengths(int length)
        {
            var targets = new float[length];

            for (var i = 0; i < length; i++) targets[i] = 1;
            return targets;
        }

        internal void NewWheel()
        {
            _mData = AssignLengths(segments);
            _selectionWheelMesh.Draw(_mData, radius);
        }
    }
}