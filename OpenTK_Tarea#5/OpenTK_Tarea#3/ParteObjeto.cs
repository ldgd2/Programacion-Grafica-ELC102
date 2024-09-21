using OpenTK_Tarea_3;

public class ParteObjeto
{
    public Dictionary<string, Poligono3D> Poligonos { get; set; }

    public ParteObjeto()
    {
        Poligonos = new Dictionary<string, Poligono3D>();
    }

    public void AgregarPoligono(string nombre, Poligono3D poligono)
    {
        Poligonos[nombre] = poligono;
    }
}
