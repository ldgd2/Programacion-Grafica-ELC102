using OpenTK_Tarea_3;

public class Objeto3D
{
    public Dictionary<string, ParteObjeto> Partes { get; set; }

    public Objeto3D()
    {
        Partes = new Dictionary<string, ParteObjeto>();
    }

    public void AgregarParte(string nombre, ParteObjeto parte)
    {
        Partes[nombre] = parte;
    }
}
