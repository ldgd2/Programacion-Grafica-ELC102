using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Collections.Generic;

namespace OpenTK_Tarea_3
{
    public class Objeto3D
    {
        public Dictionary<string, ParteObjeto> Partes { get; private set; }
        public Vector3 Posicion { get; set; }

        private int _shaderProgram;

        public Objeto3D()
        {
            Partes = new Dictionary<string, ParteObjeto>();
            Posicion = Vector3.Zero;
            _shaderProgram = RenderShader.CrearShader();
        }

        public void AgregarParte(string id, ParteObjeto parte)
        {
            if (!Partes.ContainsKey(id))
            {
                Partes.Add(id, parte);
            }
        }

        // Configuración de vértices e índices para la geometría "T"
        public void InicializarVertices()
        {
            var puntos = new Dictionary<string, Punto3D>
            {
                { "P0", new Punto3D(new Vector3(-0.5f,  0.5f,  0.1f), new Vector3(7 / 255.0f, 230 / 255.0f, 255 / 255.0f)) },
                { "P1", new Punto3D(new Vector3(0.5f,  0.5f,  0.1f), new Vector3(7 / 255.0f, 230 / 255.0f, 255 / 255.0f)) },
                { "P2", new Punto3D(new Vector3(0.5f,  0.3f,  0.1f), new Vector3(7 / 255.0f, 230 / 255.0f, 255 / 255.0f)) },
                { "P3", new Punto3D(new Vector3(-0.5f,  0.3f,  0.1f), new Vector3(7 / 255.0f, 230 / 255.0f, 255 / 255.0f)) },
                { "P4", new Punto3D(new Vector3(-0.1f,  0.3f,  0.1f), new Vector3(7 / 255.0f, 230 / 255.0f, 255 / 255.0f)) },
                { "P5", new Punto3D(new Vector3(0.1f,  0.3f,  0.1f), new Vector3(7 / 255.0f, 230 / 255.0f, 255 / 255.0f)) },
                { "P6", new Punto3D(new Vector3(0.1f, -0.5f,  0.1f), new Vector3(7 / 255.0f, 230 / 255.0f, 255 / 255.0f)) },
                { "P7", new Punto3D(new Vector3(-0.1f, -0.5f,  0.1f), new Vector3(7 / 255.0f, 230 / 255.0f, 255 / 255.0f)) },
                // Cara trasera
                { "P8", new Punto3D(new Vector3(-0.5f,  0.5f, -0.1f), new Vector3(7 / 255.0f, 230 / 255.0f, 255 / 255.0f)) },
                { "P9", new Punto3D(new Vector3(0.5f,  0.5f, -0.1f), new Vector3(7 / 255.0f, 230 / 255.0f, 255 / 255.0f)) },
                { "P10", new Punto3D(new Vector3(0.5f,  0.3f, -0.1f), new Vector3(7 / 255.0f, 230 / 255.0f, 255 / 255.0f)) },
                { "P11", new Punto3D(new Vector3(-0.5f,  0.3f, -0.1f), new Vector3(7 / 255.0f, 230 / 255.0f, 255 / 255.0f)) },
                { "P12", new Punto3D(new Vector3(-0.1f,  0.3f, -0.1f), new Vector3(7 / 255.0f, 230 / 255.0f, 255 / 255.0f)) },
                { "P13", new Punto3D(new Vector3(0.1f,  0.3f, -0.1f), new Vector3(7 / 255.0f, 230 / 255.0f, 255 / 255.0f)) },
                { "P14", new Punto3D(new Vector3(0.1f, -0.5f, -0.1f), new Vector3(7 / 255.0f, 230 / 255.0f, 255 / 255.0f)) },
                { "P15", new Punto3D(new Vector3(-0.1f, -0.5f, -0.1f), new Vector3(7 / 255.0f, 230 / 255.0f, 255 / 255.0f)) }
            };

            uint[] indices = new uint[]
            {
                0, 1, 2, 2, 3, 0,
                4, 5, 6, 6, 7, 4,
                8, 9, 10, 10, 11, 8,
                12, 13, 14, 14, 15, 12,
                0, 1, 9, 9, 8, 0,
                1, 2, 10, 10, 9, 1,
                2, 3, 11, 11, 10, 2,
                3, 0, 8, 8, 11, 3,
                4, 5, 13, 13, 12, 4,
                5, 6, 14, 14, 13, 5,
                6, 7, 15, 15, 14, 6,
                7, 4, 12, 12, 15, 7
            };

            var poligono = new Poligono3D(puntos, indices);
            var parte = new ParteObjeto();
            parte.AgregarPoligono("PoligonoT", poligono);

            AgregarParte("ParteT", parte);
        }

        public void Dibujar(float x, float y, float z, Matrix4 matVista, Matrix4 matProy)
        {
            Matrix4 modelMatrix = Matrix4.CreateTranslation(x, y, z);
            Matrix4 mvp = modelMatrix * matVista * matProy;

            GL.UseProgram(_shaderProgram);
            GL.UniformMatrix4(0, false, ref mvp);

            foreach (var parte in Partes.Values)
            {
                parte.Dibujar();
            }
        }

        public void Liberar()
        {
            foreach (var parte in Partes.Values)
            {
                parte.Liberar();
            }
            GL.DeleteProgram(_shaderProgram);
        }
    }
}
