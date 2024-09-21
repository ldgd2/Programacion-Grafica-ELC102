using System.Collections.Generic;

namespace OpenTK_Tarea_3
{
    public class Poligono3D
    {
        public Dictionary<string, Punto3D> Puntos { get; private set; }
        public uint[] Indices { get; set; }

        public Poligono3D()
        {
            Puntos = new Dictionary<string, Punto3D>();
        }

        public Poligono3D(Dictionary<string, Punto3D> puntos, uint[] indices)
        {
            Puntos = puntos;
            Indices = indices;
        }

        // Método para obtener los vértices en formato plano (array float)
        public float[] ObtenerVertices()
        {
            var vertices = new List<float>();
            foreach (var punto in Puntos.Values)
            {
                vertices.Add(punto.Posicion.X);
                vertices.Add(punto.Posicion.Y);
                vertices.Add(punto.Posicion.Z);
            }
            return vertices.ToArray();
        }

        // Método para obtener los índices
        public uint[] ObtenerIndices()
        {
            return Indices;
        }
    }
}
