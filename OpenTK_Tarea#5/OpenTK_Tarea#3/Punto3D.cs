using OpenTK.Mathematics;  // Para usar la clase Vector3

namespace OpenTK_Tarea_3
{
    public class Punto3D
    {
        public Vector3 Posicion { get; set; }  // Definir la propiedad Posicion
        public Vector3 Color { get; set; }     // Otra propiedad que almacena el color del punto

        public Punto3D(Vector3 posicion, Vector3 color)
        {
            Posicion = posicion;  // Inicializar la posición del punto
            Color = color;        // Inicializar el color del punto
        }
    }
}
