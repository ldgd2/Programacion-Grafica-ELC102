using System;
using System.IO;
using System.Text.Json;
using OpenTK.Mathematics;
using System.Text.Json;
using System.Windows.Forms;
using OpenTK_Tarea_3;

public class Serializador
{
    private const string RutaArchivo = "objeto.json";

    // Guardar objeto en archivo JSON
    public void GuardarObjeto(Objeto3D objeto, Vector3 posicion, string rutaArchivo)
    {
        var objetoDTO = new ObjetoDTO(objeto, posicion);
        string json = JsonSerializer.Serialize(objetoDTO, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(rutaArchivo, json);
        Console.WriteLine($"Objeto guardado correctamente en {rutaArchivo}");
    }

    // Cargar objeto desde archivo JSON usando una ruta
    public Objeto3D CargarObjeto(out Vector3 posicion, string rutaArchivo)
    {
        if (!File.Exists(rutaArchivo))
        {
            throw new FileNotFoundException($"El archivo {rutaArchivo} no existe.");
        }

        string json = File.ReadAllText(rutaArchivo);
        var objetoDTO = JsonSerializer.Deserialize<ObjetoDTO>(json);
        posicion = objetoDTO.ObtenerPosicion();
        return objetoDTO.ReconstruirObjeto();
    }
}
