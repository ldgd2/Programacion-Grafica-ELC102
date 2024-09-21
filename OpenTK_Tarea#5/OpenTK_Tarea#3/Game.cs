using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using FormsKeys = System.Windows.Forms.Keys;
using OpenTKKeys = OpenTK.Windowing.GraphicsLibraryFramework.Keys;

using OpenTK_Tarea_3;

public class Game : GameWindow
{

    private int shaderProgram;
    private ModelRenderer _renderizador3D;
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
        // Crear el shader
        


        // Crear los shaders y el modelo
        shaderProgram = ShaderHandler.CrearShader();
        var modeloT = new ModeloT(); // Suponiendo que ya tienes esta clase para tu modelo "T"
        _renderizador3D = new ModelRenderer(shaderProgram);
        // Inicializar los buffers para todas las partes y polígonos del objeto
        _renderizador3D.InicializarBuffersParaObjeto(modeloT.Objeto);
        _gameplay = new Gameplay(_escena, _camera, _renderizador3D);
        // Crear instancia de Gameplay, pasando la escena, cámara y el renderizador
        _gameplay.DibujarObjetos();
        //_escena.AgregarObjeto("DefaultObject", modeloT.Objeto, new Vector3(0, 0, 0));
        base.OnLoad();
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {

        base.OnRenderFrame(e);

        _gameplay.LimpiarPantalla();

        // Actualizar el movimiento del ratón
        _gameplay.OnMouseMove(MouseState, _arrastrar, ref _posRaton);
        _gameplay.DibujarObjetos();
        // Obtener matrices de la cámara
        var matVista = _camera.GetViewMatrix();
        var matProyeccion = _camera.GetProjectionMatrix();

        // Calcular la matriz MVP (Model-View-Projection)
        Matrix4 mvp = matVista * matProyeccion;

        // Actualizar la MVP en el renderizador
        _renderizador3D.ActualizarMVP(mvp);

        // Renderizar el modelo
        _renderizador3D.Renderizar();

        SwapBuffers();
    }

    protected override void OnUnload()
    {
        base.OnUnload();
        _renderizador3D.Limpiar();
    }

    protected override void OnUpdateFrame(FrameEventArgs e)
    {
        _gameplay.OnKeyPress(KeyboardState);

       // if (keyboardState.IsKeyPressed(OpenTKKeys.Escape))
       // {
        //    Close();
       // }
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
