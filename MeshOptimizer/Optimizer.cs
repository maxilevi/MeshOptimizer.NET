using System;
using System.Collections.Generic;

namespace MeshOptimizer
{
    public static class Optimizer
    {
        public static void Reindex<T>(Mesh<T> Target)
        {
            var remap = new uint[Target.Vertices.Length];
            var vertexPointer = Pointer.Create(Target.Vertices);
            var indexCount = (Target.Indices?.Length ?? Target.Vertices.Length);
            var totalVertices = MeshOptimizerNative.meshopt_generateVertexRemap(
                remap,
                Target.Indices,
                (UIntPtr) indexCount,
                vertexPointer.Address,
                (UIntPtr) Target.Vertices.Length,
                (UIntPtr) Target.VertexSize
            );

            var indices = new uint[indexCount];
            MeshOptimizerNative.meshopt_remapIndexBuffer(indices, Target.Indices, (UIntPtr) indexCount, remap);

            var vertices = new T[totalVertices];
            var targetVerticesPointer = Pointer.Create(vertices);
            MeshOptimizerNative.meshopt_remapVertexBuffer(targetVerticesPointer.Address, vertexPointer.Address, (UIntPtr) Target.Vertices.Length, (UIntPtr) Target.VertexSize, remap);

            Target.Vertices = vertices;
            Target.Indices = indices;
            vertexPointer.Free();
            targetVerticesPointer.Free();
        }
    }
}