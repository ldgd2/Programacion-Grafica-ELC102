using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;

namespace OpenTK_Tarea_3
{
    internal class Game : GameWindow
    {
        private Escena _escena;
        private Matrix4 _matProy;
        private Vector2 _posRaton;
        private float _rotX, _rotY;
        private bool _arrastrar;

        public Game(int ancho, int alto, string titulo)
            : base(GameWindowSettings.Default, new NativeWindowSettings()
            {
                ClientSize = new Vector2i(ancho, alto),
                Title = titulo
            })
        { }

        protected override void OnLoad()
        {
            GL.ClearColor(0f, 0f, 0f, 1f);
            GL.Enable(EnableCap.DepthTest);

            _escena = new Escena();

            // Configurar matriz de proyección (perspectiva)
            _matProy = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45f), (float)Size.X / Size.Y, 0.1f, 100f);

            // Configurar matriz de vista para la cámara, ubicada en (0, 0, 10), mirando hacia el origen
            _rotX = 0;
            _rotY = 0;
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            if (_arrastrar)
            {
                var deltaX = MouseState.X - _posRaton.X;
                var deltaY = MouseState.Y - _posRaton.Y;

                _rotY += deltaX * 0.01f;
                _rotX += deltaY * 0.01f;
            }

            _posRaton = new Vector2(MouseState.X, MouseState.Y);

            // Colocamos la cámara en (0, 0, 10) mirando hacia el origen (0, 0, 0)
            Matrix4 matVista = Matrix4.CreateRotationX(_rotX) * Matrix4.CreateRotationY(_rotY) * Matrix4.CreateTranslation(0f, 0f, -10f);

            // Dibujar la escena
            _escena.Dibujar(matVista, _matProy);

            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            if (KeyboardState.IsKeyPressed(Keys.R))
            {
                // Pedir coordenadas
                Console.WriteLine("Ingrese las coordenadas en el formato: x,y,z");
                string input = Console.ReadLine();

                // Procesar entrada
                ProcesarEntrada(input);
            }
        }

        private void ProcesarEntrada(string input)
        {
            string[] coords = input.Split(',');

            if (coords.Length == 3 &&
                float.TryParse(coords[0], out float x) &&
                float.TryParse(coords[1], out float y) &&
                float.TryParse(coords[2], out float z))
            {
                var nuevoObjeto = new Objeto3D();
                nuevoObjeto.InicializarVertices();
                _escena.AgregarObjeto(nuevoObjeto, x, y, z);
                Console.WriteLine($"Objeto agregado en posición: ({x}, {y}, {z})");
            }
            else
            {
                Console.WriteLine("Coordenadas inválidas, ingrese coordenadas válidas.");
            }
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            if (e.Button == MouseButton.Left)
                _arrastrar = true;
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            if (e.Button == MouseButton.Left)
                _arrastrar = false;
        }

        protected override void OnUnload()
        {
            _escena.LiberarRecursos();
        }
    }
}
