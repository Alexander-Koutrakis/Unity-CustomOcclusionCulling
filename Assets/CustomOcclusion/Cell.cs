using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomOcclusion
{
    public class Cell : MonoBehaviour
    {
        public bool IsVisible = false;
        public bool IsColisionOnly = false;


        public Cube GetCube()
        {
            return new Cube(transform.position, transform.rotation, transform.localScale);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Cube cube = GetCube();
            Vector3[] transformedVertices = TransformVertices(cube);

            // Draw wireframe of the cube
            DrawWireCube(transformedVertices);
        }

        private Vector3[] TransformVertices(Cube cube)
        {
            Vector3[] vertices = new Vector3[]
            {
                new Vector3(-1f, -1f, -1f),
                new Vector3(1f, -1f, -1f),
                new Vector3(1f, 1f, -1f),
                new Vector3(-1f, 1f, -1f),
                new Vector3(-1f, -1f, 1f),
                new Vector3(1f, -1f, 1f),
                new Vector3(1f, 1f, 1f),
                new Vector3(-1f, 1f, 1f)
            };

            Vector3[] transformedVertices = new Vector3[vertices.Length];
            Matrix4x4 transformMatrix = Matrix4x4.TRS(cube.Position, cube.Rotation, cube.Scale);

            for (int i = 0; i < vertices.Length; i++)
            {
                transformedVertices[i] = transformMatrix.MultiplyPoint3x4(vertices[i]);
            }

            return transformedVertices;
        }

        private void DrawWireCube(Vector3[] vertices)
        {
            if (vertices.Length != 8) return;

            // Draw lines between vertices to form the wireframe cube
            Gizmos.DrawLine(vertices[0], vertices[1]);
            Gizmos.DrawLine(vertices[1], vertices[2]);
            Gizmos.DrawLine(vertices[2], vertices[3]);
            Gizmos.DrawLine(vertices[3], vertices[0]);

            Gizmos.DrawLine(vertices[4], vertices[5]);
            Gizmos.DrawLine(vertices[5], vertices[6]);
            Gizmos.DrawLine(vertices[6], vertices[7]);
            Gizmos.DrawLine(vertices[7], vertices[4]);

            Gizmos.DrawLine(vertices[0], vertices[4]);
            Gizmos.DrawLine(vertices[1], vertices[5]);
            Gizmos.DrawLine(vertices[2], vertices[6]);
            Gizmos.DrawLine(vertices[3], vertices[7]);
        }
    }
}
