using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Graphics.OpenGL4;
using System;
using FormsKeys = System.Windows.Forms.Keys;
using OpenTKKeys = OpenTK.Windowing.GraphicsLibraryFramework.Keys;

using OpenTK.Windowing.Common;

namespace OpenTK_Tarea_3
{
    public class Gameplay
    {
        private Escena _escena;
        private Camera _camera;
        private Serializador _serializador;
        private ModelRenderer _renderizador3D;  // Corregido: nombre en minúscula

        public Gameplay(Escena escena, Camera camera, ModelRenderer renderizador3D)
        {
            _escena = escena ?? throw new ArgumentNullException(nameof(escena));
            _camera = camera;
            _serializador = new Serializador();
            _renderizador3D = renderizador3D;  // Inicializar renderizador
        }

        public void LimpiarPantalla()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }

        public void OnKeyPress(KeyboardState keyboardState)
        {
            // Acciones para zoom
            if (keyboardState.IsKeyPressed(OpenTKKeys.Z)) { _camera.AjustarZoom(-0.5f); }
            if (keyboardState.IsKeyPressed(OpenTKKeys.X)) { _camera.AjustarZoom(0.5f); }

            // Acciones para guardar y cargar
            if (keyboardState.IsKeyPressed(OpenTKKeys.G)) { GuardarObjeto(); }
            if (keyboardState.IsKeyPressed(OpenTKKeys.C)) { CargarObjeto(); }

            // Acciones para crear nuevo objeto
            if (keyboardState.IsKeyPressed(OpenTKKeys.R)) { CrearNuevoObjeto(); }
        }

        private void GuardarObjeto()
        {
            var objeto = ObtenerUltimoObjeto(out Vector3 posicion);
            if (objeto != null)
            {
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
                    saveFileDialog.Title = "Guardar objeto como";

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        // Guardar el objeto en la ruta seleccionada
                        _serializador.GuardarObjeto(objeto, posicion, saveFileDialog.FileName);
                        Console.WriteLine($"Objeto guardado correctamente en {saveFileDialog.FileName}");
                    }
                }
            }
            else
            {
                Console.WriteLine("No hay ningún objeto para guardar.");
            }
        }


        private void CargarObjeto()
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
                openFileDialog.Title = "Cargar objeto";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Pedir las coordenadas para cargar el objeto
                    Console.WriteLine("Ingrese las coordenadas para cargar el objeto en formato: x,y,z");
                    string input = Console.ReadLine();

                    // Procesar la entrada de coordenadas
                    if (ProcesarEntrada(input, out Vector3 posicion))
                    {
                        // Cargar el objeto desde el archivo
                        var objetoCargado = _serializador.CargarObjeto(out _, openFileDialog.FileName); // Se ignora la posición guardada

                        if (objetoCargado != null)
                        {
                            // Inicializar buffers para el objeto cargado
                            _renderizador3D.InicializarBuffersParaObjeto(objetoCargado);

                            // Agregar el objeto a la escena con las coordenadas proporcionadas
                            Console.WriteLine("Ingrese un nombre para el objeto cargado:");
                            string nombreObjeto = Console.ReadLine();
                            _escena.AgregarObjeto(nombreObjeto, objetoCargado, posicion);
                            Console.WriteLine($"Objeto '{nombreObjeto}' cargado y añadido a la escena en la posición: {posicion}.");
                        }
                        else
                        {
                            Console.WriteLine("No se pudo cargar el objeto desde el archivo.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Coordenadas inválidas. No se pudo cargar el objeto.");
                    }
                }
            }
        }




        private void CrearNuevoObjeto()
        {
            // Ask for the name of the new object
            Console.WriteLine("Ingrese un nombre para el nuevo objeto:");
            string nombreObjeto = Console.ReadLine();

            // Ask for the position
            Console.WriteLine("Ingrese las coordenadas para el nuevo objeto en formato: x,y,z");
            string input = Console.ReadLine();

            if (ProcesarEntrada(input, out Vector3 posicion))
            {
                // Create the model "T"
                var modeloT = new ModeloT();
                var nuevoObjeto = modeloT.Objeto;

                // Initialize buffers for the new object
                _renderizador3D.InicializarBuffersParaObjeto(nuevoObjeto);

                // Add the new object to the scene
                _escena.AgregarObjeto(nombreObjeto, nuevoObjeto, posicion);
                Console.WriteLine($"Nuevo objeto '{nombreObjeto}' agregado en la posición ({posicion.X}, {posicion.Y}, {posicion.Z}).");
            }
            else
            {
                Console.WriteLine("Coordenadas inválidas. No se pudo crear el objeto.");
            }
        }






        private Objeto3D ObtenerUltimoObjeto(out Vector3 posicion)
        {
            if (_escena.Objetos.Count > 0)
            {
                var ultimoObjeto = _escena.Objetos.Last();
                posicion = ultimoObjeto.Value.Posicion;  // Usar la posición correcta
                return ultimoObjeto.Value.Objeto;        // Devolver solo el Objeto3D
            }
            else
            {
                posicion = Vector3.Zero;
                return null;
            }
        }


        public void DibujarObjetos()
        {
            if (_escena.Objetos.Count == 0)
            {
               // Console.WriteLine("No hay objetos en la escena para dibujar.");
            }

            _renderizador3D.DibujarObjetosEnEscena(_escena, _camera);
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
