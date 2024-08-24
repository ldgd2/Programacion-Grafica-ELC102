using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OpenTK_Tarea_1
{
    internal class Simple3DGame : GameWindow
    {
        private Objeto objeto3D;
        private Matrix4 projectionMatrix;
        private Matrix4 viewMatrix;
        private Vector2 lastMousePos;
        private float rotationX = 0f;
        private float rotationY = 0f;
        private bool isDragging = false;

        public Simple3DGame(int width, int height, string title)
            : base(GameWindowSettings.Default, new NativeWindowSettings()
            {
                ClientSize = new Vector2i(width, height),
                Title = title
            })
        { }

        protected override void OnLoad()
        {
            GL.ClearColor(0f, 0f, 0f, 1f); // Fondo negro
            objeto3D = new Objeto();
            objeto3D.Load();
            VSync = VSyncMode.On;

            // Configura la matriz de proyección (perspectiva)
            projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45f), (float)Size.X / Size.Y, 0.1f, 100f);
            // Configura la vista inicial
            viewMatrix = Matrix4.CreateTranslation(0f, 0f, -3f);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            if (isDragging)
            {
                var deltaX = MouseState.X - lastMousePos.X;
                var deltaY = MouseState.Y - lastMousePos.Y;

                rotationY += deltaX * 0.01f;
                rotationX += deltaY * 0.01f;
            }

            lastMousePos = new Vector2(MouseState.X, MouseState.Y);

            // Actualiza la matriz de vista para la rotación de la cámara
            viewMatrix = Matrix4.CreateRotationX(rotationX) * Matrix4.CreateRotationY(rotationY) * Matrix4.CreateTranslation(0f, 0f, -3f);

            // Renderiza el objeto con la vista actualizada
            objeto3D.Render(viewMatrix, projectionMatrix);

            SwapBuffers();
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            if (e.Button == MouseButton.Left)
                isDragging = true;
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            if (e.Button == MouseButton.Left)
                isDragging = false;
        }

        protected override void OnUnload()
        {
            objeto3D.Unload();
        }
    }
}

