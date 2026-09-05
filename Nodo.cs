using System.Collections.Generic;

public class NodoBPlus
{
    // True si es hoja (guarda las claves), False si es nodo interno (guarda guías e hijos)
    public bool Hoja { get; set; }
    
    // Lista donde guardamos las codigos de libos
    public List<int> Claves { get; set; }
    
    public List<NodoBPlus> Hijos { get; set; }     // Lista de punteros a los hijos
    
    public NodoBPlus Siguiente { get; set; }

    public NodoBPlus(bool hoja = true)
    {
        Hoja = hoja;
        Claves = new List<int>();
        Hijos = new List<NodoBPlus>();
        Siguiente = null;
    }
}