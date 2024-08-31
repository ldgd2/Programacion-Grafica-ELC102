using System.Collections.Generic;
using OpenTK.Mathematics;

namespace OpenTK_Tarea_3
{
    internal class Escena
    {
        private readonly List<(Objeto3D Objeto, Vector3 Posicion)> _objetos;

        public Escena()
        {
            _objetos = new List<(Objeto3D, Vector3)>();
        }

        public void AgregarObjeto(Objeto3D obj, float x, float y, float z)
        {
            _objetos.Add((obj, new Vector3(x, y, z)));
        }

        public void Dibujar(Matrix4 matVista, Matrix4 matProy)
        {
            foreach (var (obj, posicion) in _objetos)
            {
                obj.Dibujar(posicion.X, posicion.Y, posicion.Z, matVista, matProy);
            }
        }

        public void LiberarRecursos()
        {
            foreach (var (obj, _) in _objetos)
            {
                obj.Liberar();
            }
        }
    }
}
