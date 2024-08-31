using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace OpenTK_Tarea_3
{
    internal class Objeto3D
    {
        private int _vao, _vbo, _ebo;
        private ParteObjeto _parte;
        private Matrix4 _modelMatrix;
        private int _shaderProgram;

        public Objeto3D()
        {
            _modelMatrix = Matrix4.Identity;
            _parte = new ParteObjeto();
            _shaderProgram = RenderShader.CrearShader();
        }

        public void InicializarVertices()
        {
            float[] vertices = {
                // Cara frontal
                -0.5f,  0.5f,  0.1f,
                 0.5f,  0.5f,  0.1f,
                 0.5f,  0.3f,  0.1f,
                -0.5f,  0.3f,  0.1f,
                -0.1f,  0.3f,  0.1f,
                 0.1f,  0.3f,  0.1f,
                 0.1f, -0.5f,  0.1f,
                -0.1f, -0.5f,  0.1f,
                // Cara trasera
                -0.5f,  0.5f, -0.1f,
                 0.5f,  0.5f, -0.1f,
                 0.5f,  0.3f, -0.1f,
                -0.5f,  0.3f, -0.1f,
                -0.1f,  0.3f, -0.1f,
                 0.1f,  0.3f, -0.1f,
                 0.1f, -0.5f, -0.1f,
                -0.1f, -0.5f, -0.1f
            };

            uint[] indices = {
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

            var poligono = new Poligono3D(vertices, indices);
            _parte.AgregarPoligono(poligono);
        }

        public void Dibujar(float x, float y, float z, Matrix4 matVista, Matrix4 matProy)
        {
            _modelMatrix = Matrix4.CreateTranslation(x, y, z);

            var mvp = _modelMatrix * matVista * matProy;
            GL.UseProgram(_shaderProgram);
            GL.UniformMatrix4(0, false, ref mvp);

            _parte.Dibujar();

            //Console.WriteLine($"Dibujando objeto en posición: ({x}, {y}, {z})");
        }

        public void Liberar()
        {
            _parte.Liberar();
            GL.DeleteProgram(_shaderProgram);
        }
    }
}
