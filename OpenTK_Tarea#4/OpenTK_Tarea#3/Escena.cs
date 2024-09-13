using System.Collections.Generic;
using OpenTK.Mathematics;

namespace OpenTK_Tarea_3
{
    public class Escena
    {
        private readonly List<(Objeto3D Objeto, Vector3 Posicion)> _objetos;

        public Escena()
        {
            _objetos = new List<(Objeto3D, Vector3)>();
        }

        public Objeto3D ObtenerUltimoObjeto(out Vector3 posicion)
        {
            if (_objetos.Count > 0)
            {
                var ultimoObjeto = _objetos[_objetos.Count - 1];
                posicion = ultimoObjeto.Posicion;
                return ultimoObjeto.Objeto;
            }
            else
            {
                posicion = Vector3.Zero;
                return null; // O lanza una excepción si prefieres
            }
        }

        public void AgregarObjeto(Objeto3D objeto, float x, float y, float z)
        {
            _objetos.Add((objeto, new Vector3(x, y, z)));
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
