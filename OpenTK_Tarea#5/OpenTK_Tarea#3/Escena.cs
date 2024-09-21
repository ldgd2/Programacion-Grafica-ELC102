using OpenTK_Tarea_3;
using OpenTK.Mathematics;


public class Escena
{
    public Dictionary<string, (Objeto3D Objeto, Vector3 Posicion)> Objetos { get; set; }

    public Escena()
    {
        Objetos = new Dictionary<string, (Objeto3D, Vector3)>();
    }

    public void AgregarObjeto(string nombre, Objeto3D objeto, Vector3 posicion)
    {
        Objetos[nombre] = (objeto, posicion);
        Console.WriteLine($"Objeto '{nombre}' agregado en la posición: {posicion}");
    }

}
