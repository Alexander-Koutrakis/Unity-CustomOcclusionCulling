using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomOcclusion
{
    public class CellGenerator 
    {
        private int[] triangleIndices = new int[]
        {
            0, 1, 2, 0, 2, 3, // Front face
            1, 5, 6, 1, 6, 2, // Right face
            5, 4, 7, 5, 7, 6, // Back face
            4, 0, 3, 4, 3, 7, // Left face
            3, 2, 6, 3, 6, 7, // Top face
            4, 5, 1, 4, 1, 0  // Bottom face
        };
        private Vector3[] vertices = new Vector3[]
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

        private Vector3[] TransformVertices(Cube cube)
        {
            Vector3[] transformedVertices = new Vector3[vertices.Length];
            Matrix4x4 transformMatrix = Matrix4x4.TRS(cube.Position, cube.Rotation, cube.Scale);

            for (int i = 0; i < vertices.Length; i++)
            {
                transformedVertices[i] = transformMatrix.MultiplyPoint3x4(vertices[i]);
            }

            return transformedVertices;
        }

        private Vector3[] GenerateTriangles(Vector3[] vertices)
        {
            Vector3[] triangleVertices = new Vector3[36];
            for (int i = 0; i < triangleIndices.Length; i++)
            {
                triangleVertices[i] = vertices[triangleIndices[i]];
            }

            return triangleVertices;
        }

        public Vector3[] GenerateCell(Cube cube)
        {
            Vector3[] transformedVertices = TransformVertices(cube);
            Vector3[] triangleVertices = GenerateTriangles(transformedVertices);

            return triangleVertices;
        }
    }

}