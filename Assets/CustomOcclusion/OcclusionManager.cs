using System.Collections;
using UnityEngine;
using System;
using UnityEngine.Rendering;
using Unity.Collections;

namespace CustomOcclusion
{
    public class OcclusionManager : MonoBehaviour
    {
            private ComputeBuffer reader;
            private ComputeBuffer writer;
            private int verticesLength;
            private Material occlusionMat;
            private Cell[] cells;
            private bool updateOcclusion;
            public static Action OnUpdate;
            private bool isDrawn = false;

            [SerializeField] private bool isDebugOn = false;
            [SerializeField] private float cameraRadius=3;

            private void Initialization()
            {

                Shader occlusionShader = Shader.Find("Custom/OcclusionShader");
                occlusionMat = new Material(occlusionShader);

                cells = FindObjectsOfType<Cell>();
                CellGenerator cellGenerator = new CellGenerator();

                verticesLength = cells.Length * 36;
                OcclusionVertice[] vertices = new OcclusionVertice[verticesLength];
                int index = 0;
                for (int i = 0; i < cells.Length; i++)
                {

                    Cube cube = cells[i].GetCube();
                    Vector3[] aabb = cellGenerator.GenerateCell(cube);

                    for (int j = 0; j < aabb.Length; j++)
                    {
                        Vector3 position = aabb[j];
                        int isColisionOnly = cells[i].IsColisionOnly ? 1 : 0;
                        vertices[index] = new OcclusionVertice(position, isColisionOnly, i);
                        index++;
                    }
                }

                writer = new ComputeBuffer(cells.Length, sizeof(int), ComputeBufferType.Default);
                reader = new ComputeBuffer(vertices.Length, sizeof(float) * 3 + sizeof(int) * 2, ComputeBufferType.Structured, ComputeBufferMode.Immutable);
                reader.SetData(vertices, 0, 0, index);
                Graphics.ClearRandomWriteTargets();
                Graphics.SetRandomWriteTarget(1, writer, false);
                occlusionMat.SetBuffer("_Reader", reader);
                occlusionMat.SetBuffer("_Writer", writer);
                occlusionMat.SetFloat("_CameraRadius", cameraRadius);
                if (isDebugOn)
                {
                    occlusionMat.EnableKeyword("DEBUG");
                }
                else
                {

                    occlusionMat.DisableKeyword("DEBUG");
                }



            }


            void Awake()
            {
                Initialization();
            }

        

            private void OnEnable()
            {
                updateOcclusion = true;
            }

            private void OnDisable()
            {
                updateOcclusion = false;
            }

            private void Start()
            {
                StartCoroutine(UpdateAsync());

            }

            private IEnumerator UpdateAsync()
            {
                while (updateOcclusion)
                {
                    AsyncGPUReadbackRequest request = AsyncGPUReadback.Request(writer);
                    yield return new WaitUntil(() => request.done);

                    NativeArray<int> results = request.GetData<int>(0);
                    for (int i = 0; i < results.Length; i++)
                    {
                        if (results[i] == 1)
                        {
                            cells[i].IsVisible = true;
                        }
                        else
                        {
                            cells[i].IsVisible = false;
                        }
                    }

                    OnUpdate?.Invoke();
                }
            }


            private void Update()
            {
                isDrawn = false;
            }
            void OnRenderObject()
            {
                if (isDrawn)
                    return;

                occlusionMat.SetPass(0);
                Graphics.DrawProceduralNow(MeshTopology.Triangles, verticesLength, 1);
                isDrawn = true;

            }

            private void OnDestroy()
            {
                writer.Dispose();
                reader.Dispose();
            }
    }

        public struct OcclusionVertice
        {
            public Vector3 position;
            public int IsColisionOnly;
            public int Index;
            public OcclusionVertice(Vector3 pos, int isColisionOnly, int index)
            {
                position = pos;
                IsColisionOnly = isColisionOnly;
                Index = index;
            }
        }
}

