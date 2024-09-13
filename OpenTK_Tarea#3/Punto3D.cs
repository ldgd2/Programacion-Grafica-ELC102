using OpenTK.Mathematics;

namespace OpenTK_Tarea_3
{
    public class Punto3D
    {
        public Vector3 Posicion { get; set; }
        public Vector3 Color { get; set; }

        public Punto3D(Vector3 posicion, Vector3 color)
        {
            Posicion = posicion;
            Color = color;
        }
    }
}
