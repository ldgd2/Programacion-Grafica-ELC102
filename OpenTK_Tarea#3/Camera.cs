using OpenTK.Mathematics;

public class Camera
{
    public Vector3 Position { get; set; } = new Vector3(0f, 0f, 5f); // 5 unidades hacia atrás en Z
    public float RotX { get; set; } = 0f;  // Rotación en el eje X
    public float RotY { get; set; } = 0f;  // Rotación en el eje Y

    private Matrix4 _projectionMatrix;

    public Camera(float aspectRatio)
    {
        _projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(
            MathHelper.DegreesToRadians(45f),
            aspectRatio,
            0.1f,
            100f);
    }

    public Matrix4 GetViewMatrix()
    {
        var rotation = Matrix4.CreateRotationX(RotX) * Matrix4.CreateRotationY(RotY);
        var translation = Matrix4.CreateTranslation(-Position);

        return rotation * translation;
    }

    public Matrix4 GetProjectionMatrix()
    {
        return _projectionMatrix;
    }

    // Método para ajustar el zoom
    public void AjustarZoom(float cantidad)
    {
        // Crear una nueva posición ajustando el valor Z
        var nuevaPosicion = Position;
        nuevaPosicion.Z += cantidad;

        // Limitar el zoom para evitar que la cámara pase el origen o se aleje demasiado
        if (nuevaPosicion.Z < 1f) nuevaPosicion.Z = 1f; // No acercarse demasiado
        if (nuevaPosicion.Z > 100f) nuevaPosicion.Z = 100f; // No alejarse demasiado

        // Asignar la nueva posición a la cámara
        Position = nuevaPosicion;
    }
}
