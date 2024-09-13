using System;
using System.IO;
using System.Text.Json;
using OpenTK.Mathematics;
using OpenTK_Tarea_3;

public class Serializador
{
    private const string RutaArchivo = "objeto.json";

    // Guardar objeto en archivo JSON
    public void GuardarObjeto(Objeto3D objeto, Vector3 posicion)
    {
        ObjetoDTO objetoDTO = new ObjetoDTO(objeto, posicion);
        string json = JsonSerializer.Serialize(objetoDTO, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(RutaArchivo, json);
        Console.WriteLine("Objeto guardado correctamente en objeto.json");
    }

    // Cargar objeto desde archivo JSON
    public Objeto3D CargarObjeto(out Vector3 posicion)
    {
        if (!File.Exists(RutaArchivo))
        {
            throw new FileNotFoundException("El archivo objeto.json no existe.");
        }

        string json = File.ReadAllText(RutaArchivo);
        ObjetoDTO objetoDTO = JsonSerializer.Deserialize<ObjetoDTO>(json);
        posicion = objetoDTO.ObtenerPosicion();
        return objetoDTO.ReconstruirObjeto();
    }
}
