using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace OpenTK_Tarea_1
{
    internal class Objeto
    {
        private int vao, vbo, ebo, shaderProgram;
        private Matrix4 modelMatrix;

        // Vértices de la letra T en 3D
        private readonly float[] vertices = {
            // Cara delantera
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

        // Índices que definen los triángulos de la letra T
        private readonly uint[] indices = {
            // Cara delantera
            0, 1, 2, 2, 3, 0,
            4, 5, 6, 6, 7, 4,    

            // Cara trasera
            8, 9, 10, 10, 11, 8,
            12, 13, 14, 14, 15, 12,

            // Lados conectando la cara delantera y trasera
            0, 1, 9, 9, 8, 0,
            1, 2, 10, 10, 9, 1,
            2, 3, 11, 11, 10, 2,
            3, 0, 8, 8, 11, 3,
            4, 5, 13, 13, 12, 4,
            5, 6, 14, 14, 13, 5,
            6, 7, 15, 15, 14, 6,
            7, 4, 12, 12, 15, 7
        };

        public Objeto()
        {
            modelMatrix = Matrix4.Identity;
        }

        public void Load()
        {
            vao = GL.GenVertexArray();
            GL.BindVertexArray(vao);

            vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            ebo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            shaderProgram = CreateShaderProgram();
        }

        public void SetPosition(float x, float y, float z)
        {
            modelMatrix = Matrix4.CreateTranslation(x, y, z);
        }

        public void Render(Matrix4 viewMatrix, Matrix4 projectionMatrix)
        {
            GL.UseProgram(shaderProgram);

            var mvp = modelMatrix * viewMatrix * projectionMatrix;
            GL.UniformMatrix4(0, false, ref mvp);

            GL.BindVertexArray(vao);
            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
        }

        public void Unload()
        {
            GL.DeleteBuffer(vbo);
            GL.DeleteBuffer(ebo);
            GL.DeleteVertexArray(vao);
            GL.DeleteProgram(shaderProgram);
        }

        private int CreateShaderProgram()
        {
            const string vertexShaderSource = @"
                #version 330 core
                layout(location = 0) in vec3 aPosition;
                uniform mat4 mvp;
                void main() { gl_Position = mvp * vec4(aPosition, 1.0); }";

            const string fragmentShaderSource = @"
                #version 330 core
                out vec4 FragColor;
                void main() { FragColor = vec4(0.0, 1.0, 1.0, 1.0); }";

            int vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, vertexShaderSource);
            GL.CompileShader(vertexShader);

            int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, fragmentShaderSource);
            GL.CompileShader(fragmentShader);

            int program = GL.CreateProgram();
            GL.AttachShader(program, vertexShader);
            GL.AttachShader(program, fragmentShader);
            GL.LinkProgram(program);

            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);

            return program;
        }
    }
}
