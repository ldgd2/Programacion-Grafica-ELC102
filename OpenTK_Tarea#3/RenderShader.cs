using OpenTK.Graphics.OpenGL4;

namespace OpenTK_Tarea_3
{
    internal static class RenderShader
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
}
