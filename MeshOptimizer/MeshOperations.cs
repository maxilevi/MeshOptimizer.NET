using System;
using System.Collections.Generic;

namespace MeshOptimizer
{
    public static class MeshOperations
    {
        public static void Optimize<T>(Mesh<T> Target)
        {
            var results = Index(Target.Vertices, Target.Indices, Target.VertexSize);
            var vertices = results.Item1;
            var indices = results.Item2;

            //OptimizeCache(indices, vertices.Length);
            //OptimizeOverdraw(indices, vertices, Target.Stride, 1.05f);
            //OptimizeVertexFetch(indices, vertices, Target.VertexSize);
            Target.Vertices = vertices;
            Target.Indices = indices;
        }

        public static Tuple<T[], uint[]> Index<T>(T[] Vertices, uint[] Indices, uint VertexSize)
        {
            var remap = new uint[Vertices.Length];
            var vertexPointer = Pointer.Create(Vertices);
            var indexCount = (Indices?.Length ?? Vertices.Length);
            var totalVertices = MeshOptimizerNative.GenerateVertexRemap(
                remap,
                Indices,
                (UIntPtr) indexCount,
                vertexPointer.Address,
                (UIntPtr) Vertices.Length,
                (UIntPtr) VertexSize
            );

            var indices = new uint[indexCount];
            MeshOptimizerNative.RemapIndexBuffer(indices, Indices, (UIntPtr) indexCount, remap);

            var vertices = new T[totalVertices];
            var targetVerticesPointer = Pointer.Create(vertices);
            MeshOptimizerNative.RemapVertexBuffer(targetVerticesPointer.Address, vertexPointer.Address, (UIntPtr) Vertices.Length, (UIntPtr) VertexSize, remap);

            vertexPointer.Free();
            targetVerticesPointer.Free();
            return Tuple.Create(vertices, indices);
        }
        
        public static void OptimizeCache(uint[] Indices, int VertexCount)
        {
            MeshOptimizerNative.OptimizeVertexCache(Indices, Indices, (UIntPtr) Indices.Length, (UIntPtr) VertexCount);
        }
        
        public static void OptimizeOverdraw<T>(uint[] Indices, T[] Vertices, uint Stride, float Threshold)
        {
            var pointer = Pointer.Create(Vertices);
            MeshOptimizerNative.OptimizeOverdraw(Indices, Indices, (UIntPtr) Indices.Length, pointer.Address, (UIntPtr) Vertices.Length, (UIntPtr) Stride, Threshold);
            pointer.Free();
        }
        
        public static void OptimizeVertexFetch<T>(uint[] Indices, T[] Vertices, uint VertexSize)
        {
            var pointer = Pointer.Create(Vertices);
            MeshOptimizerNative.OptimizeVertexFetch(pointer.Address, Indices, (UIntPtr) Indices.Length, pointer.Address, (UIntPtr) Vertices.Length, (UIntPtr) VertexSize);
            pointer.Free();
        }
    }
}