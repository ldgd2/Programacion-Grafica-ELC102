using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OpenTK_Tarea_1
{
    internal class Simple3DGame : GameWindow
    {
        private int vao, vbo, ebo, shaderProgram;
        private Vector2 lastMousePos;
        private float rotationX, rotationY;
        private bool isDragging;

        private readonly float[] vertices = {
           
            -0.5f,  0.5f,  0.1f,
             0.5f,  0.5f,  0.1f,
             0.5f,  0.3f,  0.1f, 
            -0.5f,  0.3f,  0.1f, 
            -0.1f,  0.3f,  0.1f, 
             0.1f,  0.3f,  0.1f, 
             0.1f, -0.5f,  0.1f, 
            -0.1f, -0.5f,  0.1f, 

           
            -0.5f,  0.5f, -0.1f, 
             0.5f,  0.5f, -0.1f,
             0.5f,  0.3f, -0.1f, 
            -0.5f,  0.3f, -0.1f, 
            -0.1f,  0.3f, -0.1f, 
             0.1f,  0.3f, -0.1f,
             0.1f, -0.5f, -0.1f, 
            -0.1f, -0.5f, -0.1f  
        };

        private readonly uint[] indices = {
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

        public Simple3DGame(int width, int height, string title)
            : base(GameWindowSettings.Default, new NativeWindowSettings()
            {
                ClientSize = new Vector2i(width, height),
                Title = title
            })
        { }

        protected override void OnLoad()
        {
            GL.ClearColor(1f, 1f, 1f, 1f);
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

            shaderProgram = CreateShaderProgram(VertexShaderSource, FragmentShaderSource);
            GL.UseProgram(shaderProgram);

            VSync = VSyncMode.On;
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            if (isDragging)
            {
                var mouseDelta = MouseState.Position - lastMousePos;
                rotationX += mouseDelta.Y * 0.005f;
                rotationY += mouseDelta.X * 0.005f;
            }
            lastMousePos = MouseState.Position;

            Matrix4 model = Matrix4.CreateRotationX(rotationX) * Matrix4.CreateRotationY(rotationY);
            GL.UniformMatrix4(GL.GetUniformLocation(shaderProgram, "model"), false, ref model);

            GL.BindVertexArray(vao);
            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);

            SwapBuffers();
        }

        protected override void OnMouseDown(MouseButtonEventArgs e) => isDragging = e.Button == MouseButton.Left;
        protected override void OnMouseUp(MouseButtonEventArgs e) => isDragging = !(e.Button == MouseButton.Left);

        protected override void OnUnload()
        {
            GL.DeleteBuffer(vbo);
            GL.DeleteBuffer(ebo);
            GL.DeleteVertexArray(vao);
            GL.DeleteProgram(shaderProgram);
        }

        private int CreateShaderProgram(string vertexSource, string fragmentSource)
        {
            int vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, vertexSource);
            GL.CompileShader(vertexShader);

            int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, fragmentSource);
            GL.CompileShader(fragmentShader);

            int program = GL.CreateProgram();
            GL.AttachShader(program, vertexShader);
            GL.AttachShader(program, fragmentShader);
            GL.LinkProgram(program);

            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);

            return program;
        }

        private const string VertexShaderSource = @"
            #version 330 core
            layout(location = 0) in vec3 aPosition;
            uniform mat4 model;
            void main() { gl_Position = model * vec4(aPosition, 1.0); }";

        private const string FragmentShaderSource = @"
            #version 330 core
            out vec4 FragColor;
            void main() { FragColor = vec4(0.0, 1.0, 1.0, 1.0); }"; 
    }
}
