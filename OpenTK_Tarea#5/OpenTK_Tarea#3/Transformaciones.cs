using OpenTK.Mathematics;

public class Transformaciones
{
    private Matrix4 _matrizTransformacion;  // Matriz acumulada de transformación

    public Transformaciones()
    {
        _matrizTransformacion = Matrix4.Identity;  // Iniciar con la matriz identidad
    }

    // Método para aplicar una traslación
    public void AplicarTraslacion(Vector3 desplazamiento)
    {
        var matrizTraslacion = Matrix4.CreateTranslation(desplazamiento);
        _matrizTransformacion *= matrizTraslacion;
    }

    // Método para aplicar una rotación alrededor de un eje
    public void AplicarRotacion(Vector3 eje, float anguloEnGrados)
    {
        float anguloEnRadianes = MathHelper.DegreesToRadians(anguloEnGrados);
        var matrizRotacion = Matrix4.CreateFromAxisAngle(eje.Normalized(), anguloEnRadianes);
        _matrizTransformacion *= matrizRotacion;
    }

    // Método para aplicar una escala
    public void AplicarEscala(float factorEscala)
    {
        var matrizEscala = Matrix4.CreateScale(factorEscala);
        _matrizTransformacion *= matrizEscala;
    }

    // Obtener la matriz transformada final
    public Matrix4 ObtenerMatrizTransformada()
    {
        return _matrizTransformacion;
    }

    // Reiniciar la matriz de transformación a la identidad
    public void ReiniciarMatriz()
    {
        _matrizTransformacion = Matrix4.Identity;
    }
}
