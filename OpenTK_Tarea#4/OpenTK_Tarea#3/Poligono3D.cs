using OpenTK.Graphics.OpenGL4;
using System.Collections.Generic;

namespace OpenTK_Tarea_3
{
    public class Poligono3D
    {
        public Dictionary<string, Punto3D> Puntos { get; private set; }
        public uint[] Indices { get; private set; }

        private int _vao, _vbo, _ebo;

        public Poligono3D(Dictionary<string, Punto3D> puntos, uint[] indices)
        {
            Puntos = puntos;
            Indices = indices;
            ConfigurarBuffers();
        }

        private void ConfigurarBuffers()
        {
            List<float> verticesList = new List<float>();
            foreach (var punto in Puntos.Values)
            {
                verticesList.Add(punto.Posicion.X);
                verticesList.Add(punto.Posicion.Y);
                verticesList.Add(punto.Posicion.Z);
            }

            float[] vertices = verticesList.ToArray();

            _vao = GL.GenVertexArray();
            GL.BindVertexArray(_vao);

            _vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            _ebo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, Indices.Length * sizeof(uint), Indices, BufferUsageHint.StaticDraw);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
        }

        public float[] ObtenerVertices()
        {
            List<float> verticesList = new List<float>();
            foreach (var punto in Puntos.Values)
            {
                verticesList.Add(punto.Posicion.X);
                verticesList.Add(punto.Posicion.Y);
                verticesList.Add(punto.Posicion.Z);
            }
            return verticesList.ToArray();
        }

        public uint[] ObtenerIndices()
        {
            return Indices;
        }

        public void Dibujar()
        {
            GL.BindVertexArray(_vao);
            GL.DrawElements(PrimitiveType.Triangles, Indices.Length, DrawElementsType.UnsignedInt, 0);
        }

        public void Liberar()
        {
            GL.DeleteBuffer(_vbo);
            GL.DeleteBuffer(_ebo);
            GL.DeleteVertexArray(_vao);
        }
    }
}
