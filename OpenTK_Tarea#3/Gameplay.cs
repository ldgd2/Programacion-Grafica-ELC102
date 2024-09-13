using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Graphics.OpenGL4;
using System;
using OpenTK.Windowing.Common;

namespace OpenTK_Tarea_3
{
    public class Gameplay
    {
        private Escena _escena;
        private Camera _camera;
        private Serializador _serializador;
        public Gameplay(Escena escena, Camera camera)
        {
            _escena = escena;
            _camera = camera;
            _serializador = new Serializador();
        }

        public Gameplay(Camera camera)
        {
            _camera = camera;
        }

        public void LimpiarPantalla()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }

        public void OnKeyPress(KeyboardState keyboardState)
        {
            // Detectar teclas para acercar y alejar con + y -
            if (keyboardState.IsKeyPressed(Keys.Z))
            {
                _camera.AjustarZoom(-0.5f); // Acercar
            }

            if (keyboardState.IsKeyPressed(Keys.X))
            {
                _camera.AjustarZoom(0.5f); // Alejar
            }

            if (keyboardState.IsKeyPressed(Keys.R))
            {
                Console.WriteLine("Ingrese las coordenadas en el formato: x,y,z");
                string input = Console.ReadLine();

                if (!string.IsNullOrEmpty(input) && ProcesarEntrada(input, out Vector3 posicion))
                {
                    var nuevoObjeto = new Objeto3D();
                    nuevoObjeto.InicializarVertices();
                    _escena.AgregarObjeto(nuevoObjeto, posicion.X, posicion.Y, posicion.Z);
                    Console.WriteLine($"Objeto agregado en posición: ({posicion.X}, {posicion.Y}, {posicion.Z})");
                }
                else
                {
                    Console.WriteLine("Coordenadas inválidas, ingrese coordenadas válidas.");
                }
            }






            if (keyboardState.IsKeyPressed(Keys.G))
            {
                var objeto = _escena.ObtenerUltimoObjeto(out Vector3 posicion);
                ObjetoDTO dto = new ObjetoDTO(objeto, posicion);
                dto.GuardarEnArchivo("objeto.json");
                Console.WriteLine("Objeto guardado en objeto.json");
            }

            if (keyboardState.IsKeyPressed(Keys.C))
            {
                Console.WriteLine("Ingrese las coordenadas en el formato: x,y,z para cargar el objeto:");
                string input = Console.ReadLine();

                if (!string.IsNullOrEmpty(input) && ProcesarEntrada(input, out Vector3 posicion))
                {
                    ObjetoDTO objetoDTO = ObjetoDTO.CargarDesdeArchivo("objeto.json");
                    var objetoCargado = objetoDTO.ReconstruirObjeto();
                    _escena.AgregarObjeto(objetoCargado, posicion.X, posicion.Y, posicion.Z);
                    Console.WriteLine($"Objeto cargado en posición: ({posicion.X}, {posicion.Y}, {posicion.Z})");
                }
                else
                {
                    Console.WriteLine("Coordenadas inválidas, ingrese coordenadas válidas.");
                }
            }
        }

        public void OnMouseMove(MouseState mouseState, bool arrastrar, ref Vector2 posRaton)
        {
            if (arrastrar)
            {
                var deltaX = mouseState.X - posRaton.X;
                var deltaY = mouseState.Y - posRaton.Y;

                // Las rotaciones ahora están correctamente asociadas
                _camera.RotX += deltaY * 0.01f;  // Movimiento en Y ajusta RotX
                _camera.RotY += deltaX * 0.01f;  // Movimiento en X ajusta RotY
            }

            posRaton = new Vector2(mouseState.X, mouseState.Y);
        }

        public void OnMouseWheel(MouseWheelEventArgs e)
        {
            // Rueda del ratón para zoom
            if (e.OffsetY > 0)
            {
                _camera.AjustarZoom(-0.5f); // Acercar
            }
            else if (e.OffsetY < 0)
            {
                _camera.AjustarZoom(0.5f); // Alejar
            }
        }

        private bool ProcesarEntrada(string input, out Vector3 posicion)
        {
            string[] coords = input.Split(',');

            if (coords.Length == 3 &&
                float.TryParse(coords[0], out float x) &&
                float.TryParse(coords[1], out float y) &&
                float.TryParse(coords[2], out float z))
            {
                posicion = new Vector3(x, y, z);
                return true;
            }

            posicion = Vector3.Zero;
            return false;
        }

        public void OnMouseDown(MouseButton button, ref bool arrastrar)
        {
            if (button == MouseButton.Left)
                arrastrar = true;
        }

        public void OnMouseUp(MouseButton button, ref bool arrastrar)
        {
            if (button == MouseButton.Left)
                arrastrar = false;
        }
    }
}
