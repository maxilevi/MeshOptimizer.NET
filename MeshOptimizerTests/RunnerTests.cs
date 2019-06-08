using MeshOptimizer;
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
            var mesh = new Mesh<Vertex>(vertices, null, Vertex.SizeInBytes);
            Optimizer.Reindex(mesh);
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

        struct Vertex
        {
            public const int SizeInBytes = sizeof(float) * 3;
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