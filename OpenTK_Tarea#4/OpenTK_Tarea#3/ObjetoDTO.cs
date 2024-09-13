using OpenTK.Mathematics;
using OpenTK_Tarea_3;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public class ObjetoDTO
{
    public List<float> Posicion { get; set; }
    public List<ParteObjetoDTO> Partes { get; set; }

    // Constructor sin parámetros para deserialización
    public ObjetoDTO() { }

    // Constructor para inicializar un ObjetoDTO
    public ObjetoDTO(Objeto3D objeto, Vector3 posicion)
    {
        Posicion = new List<float> { posicion.X, posicion.Y, posicion.Z };
        Partes = new List<ParteObjetoDTO>();
        foreach (var parteKeyValue in objeto.Partes)
        {
            Partes.Add(new ParteObjetoDTO(parteKeyValue.Key, parteKeyValue.Value));
        }
    }

    public Vector3 ObtenerPosicion()
    {
        return new Vector3(Posicion[0], Posicion[1], Posicion[2]);
    }

    public Objeto3D ReconstruirObjeto()
    {
        var objeto = new Objeto3D();
        foreach (var parteDTO in Partes)
        {
            objeto.AgregarParte(parteDTO.Id, parteDTO.ReconstruirParte());
        }
        return objeto;
    }

    // Método para guardar en un archivo JSON
    public void GuardarEnArchivo(string rutaArchivo)
    {
        string json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(rutaArchivo, json);
        Console.WriteLine("Objeto guardado correctamente en " + rutaArchivo);
    }

    // Método para cargar desde un archivo JSON
    public static ObjetoDTO CargarDesdeArchivo(string rutaArchivo)
    {
        if (!File.Exists(rutaArchivo))
        {
            throw new FileNotFoundException("El archivo " + rutaArchivo + " no existe.");
        }

        string json = File.ReadAllText(rutaArchivo);
        return JsonSerializer.Deserialize<ObjetoDTO>(json);
    }

    // Clase anidada ParteObjetoDTO
    public class ParteObjetoDTO
    {
        public string Id { get; set; }
        public List<PoligonoDTO> Poligonos { get; set; }

        // Constructor sin parámetros para deserialización
        public ParteObjetoDTO() { }

        public ParteObjetoDTO(string id, ParteObjeto parte)
        {
            Id = id;
            Poligonos = new List<PoligonoDTO>();
            foreach (var poligonoKeyValue in parte.Poligonos)
            {
                Poligonos.Add(new PoligonoDTO(poligonoKeyValue.Key, poligonoKeyValue.Value));
            }
        }

        public ParteObjeto ReconstruirParte()
        {
            var parte = new ParteObjeto();
            foreach (var poligonoDTO in Poligonos)
            {
                parte.AgregarPoligono(poligonoDTO.Id, poligonoDTO.ReconstruirPoligono());
            }
            return parte;
        }
    }

    // Clase anidada PoligonoDTO
    public class PoligonoDTO
    {
        public string Id { get; set; }
        public List<float> Vertices { get; set; }
        public List<uint> Indices { get; set; }

        // Constructor sin parámetros para deserialización
        public PoligonoDTO() { }

        public PoligonoDTO(string id, Poligono3D poligono)
        {
            Id = id;
            Vertices = new List<float>(poligono.ObtenerVertices());
            Indices = new List<uint>(poligono.ObtenerIndices());
        }

        public Poligono3D ReconstruirPoligono()
        {
            Dictionary<string, Punto3D> puntos = new Dictionary<string, Punto3D>();
            int i = 0;
            while (i < Vertices.Count)
            {
                Vector3 posicion = new Vector3(Vertices[i], Vertices[i + 1], Vertices[i + 2]);
                puntos.Add($"P{i / 3}", new Punto3D(posicion, Vector3.One)); // Ajusta el color según tu lógica
                i += 3;
            }
            return new Poligono3D(puntos, Indices.ToArray());
        }
    }
}
