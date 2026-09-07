public class NodoBPlus
{
    public bool Hoja { get; set; }
    public int[] Claves { get; set; }
    public Libro[] Valores { get; set; } // Guarda los libros si es hoja
    public NodoBPlus[] Hijos { get; set; }
    public int Count { get; set; } // Cantidad actual de claves en el nodo
    public NodoBPlus Siguiente { get; set; }

    public NodoBPlus(bool hoja, int orden = 4)
    {
        Hoja = hoja;
        // se asigna tamaño orden + 1 para permitir desbordamientos temporales antes de dividir
        Claves = new int[orden + 1];
        Valores = new Libro[orden + 1];
        Hijos = new NodoBPlus[orden + 2];
        Count = 0;
        Siguiente = null;
    }
}