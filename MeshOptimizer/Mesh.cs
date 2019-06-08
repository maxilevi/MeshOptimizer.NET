using System.Collections.Generic;

namespace MeshOptimizer
{
    public class Mesh<T>
    {
        public T[] Vertices { get; set; }
        public uint[] Indices { get; set; }
        public uint VertexSize { get; }
        
        public Mesh(T[] Vertices, uint[] Indices, uint VertexSize)
        {
            this.Vertices = Vertices;
            this.Indices = Indices;
            this.VertexSize = VertexSize;
        }
    }
}