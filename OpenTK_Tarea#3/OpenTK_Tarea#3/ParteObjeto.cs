using System.Collections.Generic;

namespace OpenTK_Tarea_3
{
    internal class ParteObjeto
    {
        private readonly List<Poligono3D> _poligonos;

        public ParteObjeto()
        {
            _poligonos = new List<Poligono3D>();
        }

        public void AgregarPoligono(Poligono3D poligono)
        {
            _poligonos.Add(poligono);
        }

        public void Dibujar()
        {
            foreach (var poligono in _poligonos)
            {
                poligono.Dibujar();
            }
        }

        public void Liberar()
        {
            foreach (var poligono in _poligonos)
            {
                poligono.Liberar();
            }
        }
    }
}
