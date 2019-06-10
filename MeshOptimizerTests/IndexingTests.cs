﻿using MeshOptimizer;
using NUnit.Framework;

namespace MeshOptimizerTests
{
    [TestFixture]
    public class RunnerTests
    {
        [Test]
        public void ReindexTest()
        {
            var vertices = new Vertex[12]
            {
                new Vertex(1, 0, 0),
                new Vertex(0, 1, 0),
                new Vertex(0, 0, 1),
                new Vertex(0, 0, 1),
                new Vertex(0, 1, 0),
                new Vertex(1, 0, 0),
                new Vertex(1, 0, 0),
                new Vertex(0, 1, 0),
                new Vertex(0, 0, 1),
                new Vertex(0, 0, 1),
                new Vertex(0, 1, 0),
                new Vertex(1, 0, 0)
            };
            var mesh = new Mesh<Vertex>(vertices, null, Vertex.SizeInBytes, Vertex.Stride);
            MeshOperations.Optimize(mesh);
            Assert.AreEqual(new Vertex[3]
            {
                new Vertex(1, 0, 0),
                new Vertex(0, 1, 0),
                new Vertex(0, 0, 1),
            }, mesh.Vertices);
            Assert.AreEqual(new uint[12]
            {
                0,1,2,
                2,1,0,
                0,1,2,
                2,1,0
            }, mesh.Indices);
        }

        [Test]
        public void ReindexingWithIndexBuffer()
        {
            var vertices = new Vertex[6]
            {
                new Vertex(1, 0, 0),
                new Vertex(0, 1, 0),
                new Vertex(0, 0, 1),
                new Vertex(0, 0, 1),
                new Vertex(0, 1, 0),
                new Vertex(1, 0, 0),
            };
            var indices = new uint[12]
            {
                0, 1, 2,
                2, 1, 0,
                5, 4, 3,
                3, 4, 5
            };
            var mesh = new Mesh<Vertex>(vertices, indices, Vertex.SizeInBytes, Vertex.Stride);
            MeshOperations.Optimize(mesh);
            Assert.AreEqual(new Vertex[3]
            {
                new Vertex(1, 0, 0),
                new Vertex(0, 1, 0),
                new Vertex(0, 0, 1),
            }, mesh.Vertices);
            Assert.AreEqual(new uint[12]
            {
                0,1,2,
                2,1,0,
                0,1,2,
                2,1,0
            }, mesh.Indices);
        }
        
        [Test]
        public void ReindexWithExtraAttributes()
        {
            var vertices = new ComplexVertex[12]
            {
                new ComplexVertex(1, 0, 0, 1, 0, 0),
                new ComplexVertex(0, 1, 0, 0, 1, 0),
                new ComplexVertex(0, 0, 1, 0, 0, 1),
                new ComplexVertex(0, 0, 1, 0, 0, 1),
                new ComplexVertex(0, 1, 0, 0, 1, 0),
                new ComplexVertex(1, 0, 0, 1, 0, 0),
                new ComplexVertex(1, 0, 0, 1, 0, 0),
                new ComplexVertex(0, 1, 0, 0, 1, 0),
                new ComplexVertex(0, 0, 1, 0, 0, 1),
                new ComplexVertex(0, 0, 1, 0, 0, 1),
                new ComplexVertex(0, 1, 0, 0, 1, 0),
                new ComplexVertex(1, 0, 0, 1, 0 ,0)
            };
            var mesh = new Mesh<ComplexVertex>(vertices, null, ComplexVertex.SizeInBytes, ComplexVertex.SizeInBytes);
            MeshOperations.Optimize(mesh);
            Assert.AreEqual(new ComplexVertex[3]
            {
                new ComplexVertex(1, 0, 0, 1, 0, 0),
                new ComplexVertex(0, 1, 0, 0, 1, 0),
                new ComplexVertex(0, 0, 1, 0, 0, 1),
            }, mesh.Vertices);
            Assert.AreEqual(new uint[12]
            {
                0,1,2,
                2,1,0,
                0,1,2,
                2,1,0
            }, mesh.Indices);
        }

        struct ComplexVertex
        {
            public const int SizeInBytes = sizeof(float) * 5;
            public float X { get; }
            public float Y { get; }
            public float Z { get; }
            
            public int I { get; }
            public int J { get; }

            public ComplexVertex(float X, float Y, float Z, int I, int J, int K)
            {
                this.X = X;
                this.Y = Y;
                this.Z = Z;
                this.I = I;
                this.J = J;
            }
        }
        
        struct Vertex
        {
            public const int SizeInBytes = sizeof(float) * 3;
            public const int Stride = SizeInBytes;
            public float X { get; }
            public float Y { get; }
            public float Z { get; }

            public Vertex(float X, float Y, float Z)
            {
                this.X = X;
                this.Y = Y;
                this.Z = Z;
            }
        }
    }
}