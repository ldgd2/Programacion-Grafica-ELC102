using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK_Tarea_3;

public class Game : GameWindow
{
    private Camera _camera;
    private Gameplay _gameplay;
    private Escena _escena;
    private Vector2 _posRaton;
    private bool _arrastrar;

    public Game(int ancho, int alto, string titulo)
        : base(GameWindowSettings.Default, new NativeWindowSettings()
        {
            Size = new Vector2i(ancho, alto),
            Title = titulo
        })
    { }

    protected override void OnLoad()
    {
        _escena = new Escena();
        _camera = new Camera(Size.X / Size.Y);
        _gameplay = new Gameplay(_escena, _camera);  // Pasamos correctamente la escena

        base.OnLoad();
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
        _gameplay.LimpiarPantalla();

        // Actualizar el movimiento del ratón
        _gameplay.OnMouseMove(MouseState, _arrastrar, ref _posRaton);

        // Obtener matrices de la cámara
        var matVista = _camera.GetViewMatrix();
        var matProyeccion = _camera.GetProjectionMatrix();

        // Dibujar la escena
        _escena.Dibujar(matVista, matProyeccion);

        SwapBuffers();
    }

    protected override void OnUpdateFrame(FrameEventArgs e)
    {
        _gameplay.OnKeyPress(KeyboardState);

        if (KeyboardState.IsKeyPressed(Keys.Escape))
        {
            Close();
        }
    }

    protected override void OnMouseDown(MouseButtonEventArgs e)
    {
        _gameplay.OnMouseDown(e.Button, ref _arrastrar);
    }

    protected override void OnMouseUp(MouseButtonEventArgs e)
    {
        _gameplay.OnMouseUp(e.Button, ref _arrastrar);
    }

    protected override void OnMouseWheel(MouseWheelEventArgs e)
    {
        _gameplay.OnMouseWheel(e);  // Manejo del zoom con la rueda del ratón
    }
}
