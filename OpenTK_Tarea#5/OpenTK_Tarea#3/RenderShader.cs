using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics; // Para Matrices y Vectores
using OpenTK_Tarea_3;
using System.Collections.Generic;

namespace OpenTK_Tarea_3
{
    // Clase para manejar shaders
    internal static class ShaderHandler
    {
        public static int CrearShader()
        {
            const string vertShaderSrc = @"
                #version 330 core
                layout(location = 0) in vec3 aPos;
                uniform mat4 mvp;
                void main() { gl_Position = mvp * vec4(aPos, 1.0); }";

            const string fragShaderSrc = @"
                #version 330 core
                out vec4 color;
                void main() { color = vec4(0.0, 1.0, 1.0, 1.0); }";

            int vertShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertShader, vertShaderSrc);
            GL.CompileShader(vertShader);

            int fragShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragShader, fragShaderSrc);
            GL.CompileShader(fragShader);

            int shaderProgram = GL.CreateProgram();
            GL.AttachShader(shaderProgram, vertShader);
            GL.AttachShader(shaderProgram, fragShader);
            GL.LinkProgram(shaderProgram);

            GL.DeleteShader(vertShader);
            GL.DeleteShader(fragShader);

            return shaderProgram;
        }
    }
    public class ModelRenderer
    {
        private Dictionary<Poligono3D, int> _vaos = new Dictionary<Poligono3D, int>(); // VAO asociado a cada polígono
        private List<int> _vbos = new List<int>();
        private List<int> _ebos = new List<int>();
        private int _shaderProgram;
        private Matrix4 _mvp;
        private Dictionary<Poligono3D, int> _vaoMap = new Dictionary<Poligono3D, int>();



        public ModelRenderer(int shaderProgram)
        {
            _shaderProgram = shaderProgram;
            _mvp = Matrix4.Identity;
        }

        public void InicializarBuffers(Poligono3D poligono)
        {
            // Create VAO, VBO, and EBO for each polygon
            int vao = GL.GenVertexArray();
            GL.BindVertexArray(vao);

            int vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, poligono.ObtenerVertices().Length * sizeof(float), poligono.ObtenerVertices(), BufferUsageHint.StaticDraw);

            int ebo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, poligono.ObtenerIndices().Length * sizeof(uint), poligono.ObtenerIndices(), BufferUsageHint.StaticDraw);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            GL.BindVertexArray(0);

            // Store the VAO for the current polygon
            _vaoMap[poligono] = vao;
        }



        public void InicializarBuffersParaObjeto(Objeto3D objeto)
        {
            foreach (var parte in objeto.Partes.Values)
            {
                foreach (var poligono in parte.Poligonos.Values)
                {
                    InicializarBuffers(poligono);
                }
            }
        }



        // Método para inicializar los buffers para todas las partes y polígonos de un Objeto3D


        private void DibujarObjeto(Objeto3D objeto)
        {
            foreach (var parte in objeto.Partes.Values)
            {
                foreach (var poligono in parte.Poligonos.Values)
                {
                    if (_vaoMap.TryGetValue(poligono, out int vao))
                    {
                       // Console.WriteLine($"Drawing object with VAO: {vao} and {poligono.ObtenerIndices().Length} indices.");
                        GL.BindVertexArray(vao);
                        GL.DrawElements(PrimitiveType.Triangles, poligono.ObtenerIndices().Length, DrawElementsType.UnsignedInt, 0);
                    }
                    else
                    {
                        Console.WriteLine("VAO not found for the given polygon.");
                    }
                }
            }
            GL.BindVertexArray(0);
        }





        public void DibujarObjetosEnEscena(Escena escena, Camera camera)
        {
            GL.UseProgram(_shaderProgram);

            foreach (var entry in escena.Objetos)
            {
                var objeto = entry.Value.Objeto;  // Obtenemos el objeto 3D
                var posicion = entry.Value.Posicion;  // Obtenemos la posición

                // Calculamos la matriz MVP
                Matrix4 modelMatrix = Matrix4.CreateTranslation(posicion);
                Matrix4 mvp = modelMatrix * camera.GetViewMatrix() * camera.GetProjectionMatrix();

                // Establecemos la matriz MVP en el shader
                int mvpLocation = GL.GetUniformLocation(_shaderProgram, "mvp");
                GL.UniformMatrix4(mvpLocation, false, ref mvp);

                // Dibujamos el objeto
                DibujarObjeto(objeto);
            }
        }









        // Método para actualizar la matriz MVP (Model-View-Projection)
        public void Renderizar()
        {
            GL.UseProgram(_shaderProgram);
            int mvpLocation = GL.GetUniformLocation(_shaderProgram, "mvp");
            GL.UniformMatrix4(mvpLocation, false, ref _mvp);

            // Iterar sobre los valores del diccionario _vaos, que son los VAOs
            foreach (int vao in _vaos.Values)
            {
                GL.BindVertexArray(vao);
                GL.DrawElements(PrimitiveType.Triangles, 36, DrawElementsType.UnsignedInt, 0); // Asegúrate de que el tamaño de los índices es correcto
            }

            GL.BindVertexArray(0);  // Desenlazar el VAO
        }


        public void ActualizarMVP(Matrix4 nuevaMvp)
        {
            _mvp = nuevaMvp;
        }

        public void Limpiar()
        {
            foreach (var vbo in _vbos)
            {
                GL.DeleteBuffer(vbo);
            }
            foreach (var ebo in _ebos)
            {
                GL.DeleteBuffer(ebo);
            }
            foreach (var vao in _vaos.Values)
            {
                GL.DeleteVertexArray(vao);
            }
        }
    }
}


