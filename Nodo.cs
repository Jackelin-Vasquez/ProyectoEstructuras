using System.Collections.Generic;

public class NodoBPlus
{
    public bool Hoja { get; set; }
    public List<int> Claves { get; set; }
    public List<Libro> Valores { get; set; } // Guarda los libros si es hoja
    public List<NodoBPlus> Hijos { get; set; }
    public NodoBPlus Siguiente { get; set; }

    public NodoBPlus(bool hoja)
    {
        Hoja = hoja;
        Claves = new List<int>();
        Valores = new List<Libro>(); // Inicializar lista de valores
        Hijos = new List<NodoBPlus>();
        Siguiente = null;
    }
}