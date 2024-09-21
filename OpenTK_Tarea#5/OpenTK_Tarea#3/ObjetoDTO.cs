using OpenTK_Tarea_3;
using System.Text.Json;
using OpenTK.Mathematics;
public class ObjetoDTO
{
    public List<float> Posicion { get; set; }
    public List<ParteObjetoDTO> Partes { get; set; }

    public ObjetoDTO() { }

    public ObjetoDTO(Objeto3D objeto, Vector3 posicion)
    {
        Posicion = new List<float> { posicion.X, posicion.Y, posicion.Z };
        Partes = new List<ParteObjetoDTO>();

        foreach (var parte in objeto.Partes)
        {
            Partes.Add(new ParteObjetoDTO(parte.Key, parte.Value));
        }
    }

    public Vector3 ObtenerPosicion()
    {
        // Maneja los casos donde la lista de posiciones no tenga suficientes elementos
        if (Posicion.Count >= 3)
        {
            return new Vector3(Posicion[0], Posicion[1], Posicion[2]);
        }
        return Vector3.Zero;  // Posición por defecto si no hay suficientes elementos
    }

    public Objeto3D ReconstruirObjeto()
    {
        var objeto = new Objeto3D();
        foreach (var parteDTO in Partes)
        {
            var parte = parteDTO.ReconstruirParte();
            objeto.AgregarParte(parteDTO.Id, parte);  // Reconstruye y añade la parte correctamente
        }
        return objeto;
    }

    public void GuardarEnArchivo(string rutaArchivo)
    {
        string json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(rutaArchivo, json);
        Console.WriteLine("Objeto guardado correctamente en " + rutaArchivo);
    }

    public static ObjetoDTO CargarDesdeArchivo(string rutaArchivo)
    {
        if (!File.Exists(rutaArchivo))
        {
            throw new FileNotFoundException("El archivo " + rutaArchivo + " no existe.");
        }

        string json = File.ReadAllText(rutaArchivo);
        return JsonSerializer.Deserialize<ObjetoDTO>(json);
    }

    public class ParteObjetoDTO
    {
        public string Id { get; set; }
        public List<PoligonoDTO> Poligonos { get; set; }

        public ParteObjetoDTO() { }

        public ParteObjetoDTO(string id, ParteObjeto parte)
        {
            Id = id;
            Poligonos = new List<PoligonoDTO>();

            foreach (var poligono in parte.Poligonos)
            {
                Poligonos.Add(new PoligonoDTO(poligono.Key, poligono.Value));
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

    public class PoligonoDTO
    {
        public string Id { get; set; }
        public List<float> Vertices { get; set; }
        public List<uint> Indices { get; set; }

        public PoligonoDTO() { }

        public PoligonoDTO(string id, Poligono3D poligono)
        {
            Id = id;
            Vertices = new List<float>();

            foreach (var punto in poligono.Puntos.Values)
            {
                Vertices.Add(punto.Posicion.X);
                Vertices.Add(punto.Posicion.Y);
                Vertices.Add(punto.Posicion.Z);
            }

            Indices = new List<uint>(poligono.Indices);
        }

        public Poligono3D ReconstruirPoligono()
        {
            var puntos = new Dictionary<string, Punto3D>();

            for (int i = 0; i < Vertices.Count; i += 3)
            {
                string puntoId = $"P{i / 3}";
                var posicion = new Vector3(Vertices[i], Vertices[i + 1], Vertices[i + 2]);
                var punto = new Punto3D(posicion, Vector3.One);  // Asigna un color predeterminado
                puntos[puntoId] = punto;
            }

            return new Poligono3D(puntos, Indices.ToArray());
        }
    }
}
