using System;
using System.Collections.Generic;

namespace MeshOptimizer
{
    /// <summary>
    /// A collection of common meshoptimizer operations
    /// </summary>
    public static class MeshOperations
    {

        /// <summary>
        /// Executes the "standard" optimizations that are suggested in the meshoptimizer README
        /// See (https://github.com/zeux/meshoptimizer#pipeline)
        /// </summary>
        /// <param name="Vertices">The vertices of the mesh</param>
        /// <param name="Indices">The indices of the mesh</param>
        /// <param name="VertexSize">The size of T in bytes</param>
        /// <typeparam name="T">The vertex type</typeparam>
        /// <returns>A tuple with the new vertices and indices</returns>
        public static Tuple<T[], uint[]> Optimize<T>(T[] Vertices, uint[] Indices, uint VertexSize)
        {
            var results = Reindex(Vertices, Indices, VertexSize);
            var vertices = results.Item1;
            var indices = results.Item2;

            OptimizeCache(indices, vertices.Length);
            OptimizeOverdraw(indices, vertices, VertexSize, 1.05f);
            OptimizeVertexFetch(indices, vertices, VertexSize);
            return Tuple.Create(vertices, indices);
        }

        /// <summary>
        /// Reindex the given mesh. See (https://github.com/zeux/meshoptimizer#indexing)
        /// </summary>
        /// <param name="Vertices">The vertices of the mesh</param>
        /// <param name="Indices">The indices of the mesh</param>
        /// <param name="VertexSize">The size of T in bytes</param>
        /// <typeparam name="T">The vertex type</typeparam>
        /// <returns>A tuple with the new vertices and indices</returns>
        public static Tuple<T[], uint[]> Reindex<T>(T[] Vertices, uint[] Indices, uint VertexSize)
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
        
        /// <summary>
        /// Optimizes the mesh for the GPU cache. See (https://github.com/zeux/meshoptimizer#vertex-cache-optimization)
        /// </summary>
        /// <param name="Indices">The indices of the mesh</param>
        /// <param name="VertexCount">Total amount of vertices the mesh has</param>
        public static void OptimizeCache(uint[] Indices, int VertexCount)
        {
            MeshOptimizerNative.OptimizeVertexCache(Indices, Indices, (UIntPtr) Indices.Length, (UIntPtr) VertexCount);
        }
        
        /// <summary>
        /// Optimizes the mesh to reduce overdraw. See (https://github.com/zeux/meshoptimizer#vertex-cache-optimization)
        /// </summary>
        /// <param name="Indices">The indices of the mesh</param>
        /// <param name="Vertices">The vertices of the mesh</param>
        /// <param name="Stride">Space (in bytes) between each vertex</param>
        /// <param name="Threshold">The optimization threshold</param>
        /// <typeparam name="T"></typeparam>
        public static void OptimizeOverdraw<T>(uint[] Indices, T[] Vertices, uint Stride, float Threshold)
        {
            var pointer = Pointer.Create(Vertices);
            MeshOptimizerNative.OptimizeOverdraw(Indices, Indices, (UIntPtr) Indices.Length, pointer.Address, (UIntPtr) Vertices.Length, (UIntPtr) Stride, Threshold);
            pointer.Free();
        }
        
        /// <summary>
        /// Optimizes vertex fetching. See (https://github.com/zeux/meshoptimizer#vertex-cache-optimization)
        /// </summary>
        /// <param name="Indices">The indices of the mesh</param>
        /// <param name="Vertices">The vertices of the mesh</param>
        /// <param name="VertexSize">The size of T in bytes</param>
        /// <typeparam name="T">The vertex type</typeparam>
        public static void OptimizeVertexFetch<T>(uint[] Indices, T[] Vertices, uint VertexSize)
        {
            var pointer = Pointer.Create(Vertices);
            MeshOptimizerNative.OptimizeVertexFetch(pointer.Address, Indices, (UIntPtr) Indices.Length, pointer.Address, (UIntPtr) Vertices.Length, (UIntPtr) VertexSize);
            pointer.Free();
        }
    }
}