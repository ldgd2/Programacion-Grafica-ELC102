using System.Collections.Generic;

namespace OpenTK_Tarea_3
{
    public class ParteObjeto
    {
        public Dictionary<string, Poligono3D> Poligonos { get; private set; }

        public ParteObjeto()
        {
            Poligonos = new Dictionary<string, Poligono3D>();
        }

        public void AgregarPoligono(string id, Poligono3D poligono)
        {
            if (!Poligonos.ContainsKey(id))
            {
                Poligonos.Add(id, poligono);
            }
        }

        public void Dibujar()
        {
            foreach (var poligono in Poligonos.Values)
            {
                poligono.Dibujar();
            }
        }

        public void Liberar()
        {
            foreach (var poligono in Poligonos.Values)
            {
                poligono.Liberar();
            }
        }
    }
}
