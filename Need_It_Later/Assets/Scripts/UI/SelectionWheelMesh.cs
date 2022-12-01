using System.Collections;
using System.Collections.Generic;
using Singletons;
using UnityEngine;
using UnityEngine.InputSystem;

/*
Modified from code by Unity user "image28", which was modified from original code by Unity user "JFo"
https://forum.unity.com/threads/create-a-pie-chart-in-unity.174529
*/

namespace UI
{
    public class SelectionWheelMesh : MonoBehaviour
    {
        private float _delay = 0.1f;
        private Color32 _lastColor = Color.clear; //new(255, 255, 255, 255);
        public int _lastSelected;
        private MeshRenderer _mMeshRenderer;
        private readonly Vector3 _mNormal = new(0f, 0f, -1f);
        private Vector3[] _mNormals;

        private int[] _mTriangles;
        private Vector2[] _mUvs;

        private Vector3[] _mVertices;

        private SelectionWheelController _myController;

        private Camera _cam;

        internal List<SpriteRenderer> ItemSprites = new();

        // Properties
        public float[] Data { get; set; }

        public int Slices { get; set; }

        public float RotationAngle { get; set; }

        public float Radius { get; set; }

        public Material[] Materials { get; set; }

        public bool initialized = false;

        private void Start()
        {
            _myController = FindObjectOfType<SelectionWheelController>();
            _cam = Camera.main;
            Invoke(nameof(StartDelay), 3);

        }

        private void StartDelay()
        {
            initialized = true;
        }

        private void Update()
        {
            if (!initialized) return;
            
            var camTransformPos = _cam.transform.position;
            transform.position = new Vector2(camTransformPos.x, camTransformPos.y);
            
            var position = _myController.transform.position;
            var mousePosition = _cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            var ang = Mathf.Atan2(mousePosition.y - position.y, mousePosition.x - position.x) * Mathf.Rad2Deg;
            float newAngle = 0;
            if (ang > 0)
            {
                newAngle = 180 - ang;
            }
            else if (ang < 0)
            {
                var diff = 180 + ang;
                diff = 180 - diff;
                newAngle = 180 + diff;
                //newAngle = 360 + ang;
            }

            var degInSlice = 360 / _myController.segments;
            var segment = Mathf.RoundToInt((newAngle + degInSlice / 2f) / degInSlice) - 1; //newAngle / degInSlice

            if (segment >= 0 && segment < Slices && segment != _lastSelected)
            {
                //var hit = Physics2D.Raycast(transform.position, -Vector2.up);
                var meshMat = _mMeshRenderer.materials;
                meshMat[_lastSelected].color = _lastColor;
                _lastColor = meshMat[segment].color;
                meshMat[segment].color = new Color32(255, 255, 150, 100);
                _lastSelected = segment;
            }
        }

        /*private void OnEnable()
        {
            if (!_cam)
            {
                _cam = Camera.main;
            }
            var mousePosition = _cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            transform.position = new Vector2(mousePosition.x, mousePosition.y);
        }*/

        public void Init(float[] data, int slices, float rotationAngle, float radius, Material[] materials, float speed)
        {
            Data = data;
            Slices = slices;
            RotationAngle = rotationAngle;
            Radius = radius;
            _delay = speed;

            // Get Mesh Renderer
            _mMeshRenderer = gameObject.GetComponent<MeshRenderer>(); // as MeshRenderer
            if (_mMeshRenderer == null)
            {
                gameObject.AddComponent<MeshRenderer>();
                _mMeshRenderer = gameObject.GetComponent<MeshRenderer>(); // as MeshRenderer
            }

            _mMeshRenderer.materials = materials;

            Materials = materials;


            Init(data);
        }

        public void Init(float[] data)
        {
            Slices = 100;
            RotationAngle = 90f;
            Radius = 0.3f;

            Data = data;
        }

        public void Draw(float[] data, float rad)
        {
            Radius = rad;
            Data = data;
            StopAllCoroutines();
            StartCoroutine(Draw());
        }

        public IEnumerator Draw()
        {
            //Check data validity for pie chart...
            while (Data == null)
            {
                Debug.Log("Selection Wheel: Data null");
                yield return null;
            }

            for (var i = 0; i < Data.Length; i++)
                if (Data[i] < 0)
                {
                    Debug.Log("Selection Wheel: Data < 0");
                    yield return null;
                }

            // Calculate sum of data values
            float sumOfData = 0;
            foreach (var value in Data) sumOfData += value;

            if (sumOfData <= 0)
            {
                Debug.Log("Selection Wheel: Data sum <= 0");
                yield return null;
            }

            // Determine how many triangles in slice
            var slice = new int[Data.Length];
            var numOfTris = 0;
            var numOfSlices = 0;
            var countedSlices = 0;

            // Caluclate slice size
            for (var i = 0; i < Data.Length; i++)
            {
                numOfTris = (int) (Data[i] / sumOfData * Slices);
                slice[numOfSlices++] = numOfTris;
                countedSlices += numOfTris;
            }

            // Check that all slices are counted.. if not -> add/sub to/from biggest slice..
            var idxOfLargestSlice = 0;
            var largestSliceCount = 0;
            for (var i = 0; i < Data.Length; i++)
                if (largestSliceCount < slice[i])
                {
                    idxOfLargestSlice = i;
                    largestSliceCount = slice[i];
                }

            // Check validity for pie chart
            if (countedSlices == 0)
            {
                Debug.Log("Selection Wheel: Slices == 0");
                yield return null;
            }

            // Adjust largest dataset to get proper slice
            slice[idxOfLargestSlice] += Slices - countedSlices;

            // Check validity for pie chart data
            if (slice[idxOfLargestSlice] <= 0)
            {
                Debug.Log("Selection Wheel: Largest pie <= 0");
                yield return null;
            }

            // Init vertices and triangles arrays
            _mVertices = new Vector3[Slices * 3];
            _mNormals = new Vector3[Slices * 3];
            _mUvs = new Vector2[Slices * 3];
            _mTriangles = new int[Slices * 3];

            //gameObject.AddComponent("MeshFilter");
            //gameObject.AddComponent("MeshRenderer");

            var mesh = GetComponent<MeshFilter>().mesh;

            mesh.Clear();

            mesh.name = "Selection Wheel";

            // Rotation offset (to get star point to "12 o'clock")
            var rotOffset = RotationAngle / 360f * 2f * Mathf.PI;

            // Calc the points in circle
            float angle;
            var x = new float[Slices];
            var y = new float[Slices];

            for (var i = 0; i < Slices; i++)
            {
                angle = i * 2f * Mathf.PI / Slices;
                x[i] = Mathf.Cos(angle + rotOffset) * Radius;
                y[i] = Mathf.Sin(angle + rotOffset) * Radius;
            }

            // Generate mesh with slices (vertices and triangles)
            for (var i = 0; i < Slices; i++)
            {
                _mVertices[i * 3 + 0] = new Vector3(0f, 0f, 0f);
                _mVertices[i * 3 + 1] = new Vector3(x[i], y[i], 0f);
                // This will ensure that last vertex = first vertex..
                _mVertices[i * 3 + 2] = new Vector3(x[(i + 1) % Slices], y[(i + 1) % Slices], 0f);

                _mNormals[i * 3 + 0] = _mNormal;
                _mNormals[i * 3 + 1] = _mNormal;
                _mNormals[i * 3 + 2] = _mNormal;

                _mUvs[i * 3 + 0] = new Vector2(0f, 0f);
                _mUvs[i * 3 + 1] = new Vector2(x[i], y[i]);
                // This will ensure that last uv = first uv..
                _mUvs[i * 3 + 2] = new Vector2(x[(i + 1) % Slices], y[(i + 1) % Slices]);

                _mTriangles[i * 3 + 0] = i * 3 + 0;
                _mTriangles[i * 3 + 1] = i * 3 + 1;
                _mTriangles[i * 3 + 2] = i * 3 + 2;
            }


            // Assign verts, norms, uvs and tris to mesh and calc normals
            mesh.vertices = _mVertices;
            mesh.normals = _mNormals;
            mesh.uv = _mUvs;
            //mesh.triangles = triangles;

            mesh.subMeshCount = Data.Length;

            var subTris = new int[Data.Length][];

            countedSlices = 0;

            transform.position = new Vector2(-100, -100);


            for (var i = 0; i < Data.Length; i++)
            {
                // Every triangle has three veritces..
                subTris[i] = new int[slice[i] * 3];

                // Add tris to subTris
                for (var j = 0; j < slice[i]; j++)
                {
                    subTris[i][j * 3 + 0] = _mTriangles[countedSlices * 3 + 0];
                    subTris[i][j * 3 + 1] = _mTriangles[countedSlices * 3 + 1];
                    subTris[i][j * 3 + 2] = _mTriangles[countedSlices * 3 + 2];

                    if (j % 5 == 0)
                        yield return new WaitForSeconds(_delay);

                    mesh.SetTriangles(subTris[i], i);
                    countedSlices++;
                }

                float newAngle;
                var degInSlice = 360 / _myController.segments;
                if (i != 0)
                {
                    newAngle = i * degInSlice;
                }
                else
                {
                    newAngle = 0;
                }

                var newGameObject = new GameObject("Segment" + i);
                newGameObject.transform.parent = transform;
                var newSpriteRenderer = newGameObject.AddComponent<SpriteRenderer>();
                newSpriteRenderer.sortingOrder = 1;
                Vector3 spritePos = transform.position +
                                    Quaternion.AngleAxis(newAngle, Vector3.forward) *
                                    Vector3.up * (Radius * 0.75f);

                newSpriteRenderer.sprite = GameManager.instance.testSprite;
                newSpriteRenderer.transform.position = spritePos;
                newSpriteRenderer.sortingLayerName = "Items";
                newSpriteRenderer.size = new Vector2(0.3f, 0.3f);
                ItemSprites.Add(newSpriteRenderer);

            }
            gameObject.SetActive(false);
        }
    }
}