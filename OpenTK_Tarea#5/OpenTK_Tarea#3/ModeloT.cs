using System.Collections.Generic;
using OpenTK.Mathematics;

namespace OpenTK_Tarea_3
{
    public class ModeloT
    {
        public Objeto3D Objeto { get; private set; }

        public ModeloT()
        {
            // Inicializamos el objeto "T" con sus partes y polígonos
            Objeto = new Objeto3D();
            InicializarPartes();
        }

        private void InicializarPartes()
        {
            // Creamos las dos partes: parte superior ("-") y parte inferior ("|")
            var parteSuperior = CrearParteSuperior();
            var parteInferior = CrearParteInferior();

            // Agregar las partes al Objeto3D
            Objeto.AgregarParte("ParteSuperior", parteSuperior);
            Objeto.AgregarParte("ParteInferior", parteInferior);
        }

        // Crear la parte superior del modelo "T" ("-")
        private ParteObjeto CrearParteSuperior()
        {
            var puntosParteSuperior = new Dictionary<string, Punto3D>
            {
                { "P0", new Punto3D(new Vector3(-0.5f, 0.5f, 0.1f), Vector3.One) },
                { "P1", new Punto3D(new Vector3(0.5f, 0.5f, 0.1f), Vector3.One) },
                { "P2", new Punto3D(new Vector3(0.5f, 0.3f, 0.1f), Vector3.One) },
                { "P3", new Punto3D(new Vector3(-0.5f, 0.3f, 0.1f), Vector3.One) },
                // Cara trasera
                { "P4", new Punto3D(new Vector3(-0.5f, 0.5f, -0.1f), Vector3.One) },
                { "P5", new Punto3D(new Vector3(0.5f, 0.5f, -0.1f), Vector3.One) },
                { "P6", new Punto3D(new Vector3(0.5f, 0.3f, -0.1f), Vector3.One) },
                { "P7", new Punto3D(new Vector3(-0.5f, 0.3f, -0.1f), Vector3.One) }
            };

            uint[] indicesParteSuperior = {
                0, 1, 2, 2, 3, 0, // Cara frontal
                4, 5, 6, 6, 7, 4, // Cara trasera
                0, 1, 5, 5, 4, 0, // Lado superior
                1, 2, 6, 6, 5, 1, // Lado derecho
                2, 3, 7, 7, 6, 2, // Lado inferior
                3, 0, 4, 4, 7, 3  // Lado izquierdo
            };

            var poligonoSuperior = new Poligono3D(puntosParteSuperior, indicesParteSuperior);
            var parteSuperior = new ParteObjeto();
            parteSuperior.AgregarPoligono("PoligonoSuperior", poligonoSuperior);

            return parteSuperior;
        }

        // Crear la parte inferior del modelo "T" ("|")
        private ParteObjeto CrearParteInferior()
        {
            var puntosParteInferior = new Dictionary<string, Punto3D>
            {
                { "P0", new Punto3D(new Vector3(-0.1f, 0.3f, 0.1f), Vector3.One) },
                { "P1", new Punto3D(new Vector3(0.1f, 0.3f, 0.1f), Vector3.One) },
                { "P2", new Punto3D(new Vector3(0.1f, -0.5f, 0.1f), Vector3.One) },
                { "P3", new Punto3D(new Vector3(-0.1f, -0.5f, 0.1f), Vector3.One) },
                // Cara trasera
                { "P4", new Punto3D(new Vector3(-0.1f, 0.3f, -0.1f), Vector3.One) },
                { "P5", new Punto3D(new Vector3(0.1f, 0.3f, -0.1f), Vector3.One) },
                { "P6", new Punto3D(new Vector3(0.1f, -0.5f, -0.1f), Vector3.One) },
                { "P7", new Punto3D(new Vector3(-0.1f, -0.5f, -0.1f), Vector3.One) }
            };

            uint[] indicesParteInferior = {
                0, 1, 2, 2, 3, 0, // Cara frontal
                4, 5, 6, 6, 7, 4, // Cara trasera
                0, 1, 5, 5, 4, 0, // Lado superior
                1, 2, 6, 6, 5, 1, // Lado derecho
                2, 3, 7, 7, 6, 2, // Lado inferior
                3, 0, 4, 4, 7, 3  // Lado izquierdo
            };

            var poligonoInferior = new Poligono3D(puntosParteInferior, indicesParteInferior);
            var parteInferior = new ParteObjeto();
            parteInferior.AgregarPoligono("PoligonoInferior", poligonoInferior);

            return parteInferior;
        }
    }
}
