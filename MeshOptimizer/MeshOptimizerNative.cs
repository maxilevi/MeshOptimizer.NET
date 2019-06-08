using System;
using System.Runtime.InteropServices;

namespace MeshOptimizer
{
    internal static class MeshOptimizerNative
    {
        private const string MeshOptimizerDLL = "meshoptimizer32.dll";
        
        [DllImport(MeshOptimizerDLL, CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint meshopt_generateVertexRemap(uint[] Destination, uint[] Indices, UIntPtr IndexCount, IntPtr Vertices, UIntPtr VertexCount, UIntPtr VertexSize);
        
        [DllImport(MeshOptimizerDLL, CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void meshopt_remapIndexBuffer(uint[] Destination, uint[] Indices, UIntPtr IndexCount, uint[] Remap);
        
        [DllImport(MeshOptimizerDLL, CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void meshopt_remapVertexBuffer(IntPtr Destination, IntPtr Vertices, UIntPtr VertexCount, UIntPtr VertexSize, uint[] Remap);
        
        /*
         *
size_t meshopt_generateVertexRemap(unsigned int* destination, const unsigned int* indices, size_t index_count, const void* vertices, size_t vertex_count, size_t vertex_size);
void meshopt_remapIndexBuffer(unsigned int* destination, const unsigned int* indices, size_t index_count, const unsigned int* remap);
void meshopt_remapVertexBuffer(void* destination, const void* vertices, size_t vertex_count, size_t vertex_size, const unsigned int* remap);

         * size_t index_count = face_count * 3;
std::vector<unsigned int> remap(index_count); // allocate temporary memory for the remap table
size_t vertex_count = meshopt_generateVertexRemap(&remap[0], NULL, index_count, &unindexed_vertices[0], index_count, sizeof(Vertex));
meshopt_remapIndexBuffer(indices, NULL, index_count, &remap[0]);
meshopt_remapVertexBuffer(vertices, &unindexed_vertices[0], index_count, sizeof(Vertex), &remap[0]);
         */
    }
}