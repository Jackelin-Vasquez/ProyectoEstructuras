using System.Collections.Generic;

public class ArbolBPlus
{
    private int orden;
    private int maxClaves;
    private NodoBPlus raiz;

    public ArbolBPlus(int orden = 4)
    {
        this.orden = orden;
        this.maxClaves = orden - 1;
        this.raiz = new NodoBPlus(true);
    }

    // Búsqueda en el árbol B+
    public bool Buscar(int clave)
    {
        return BuscarRec(raiz, clave);
    }

    private bool BuscarRec(NodoBPlus nodo, int clave)
    {
        if (nodo.Hoja)
        {
            return nodo.Claves.Contains(clave);
        }

        int posicion = 0;
        while (posicion < nodo.Claves.Count && clave >= nodo.Claves[posicion])
        {
            posicion++;
        }

        return BuscarRec(nodo.Hijos[posicion], clave);
    }

    // Método para mostrar la estrucurra del arbol:p
    public void Mostrar()
    {
        Mostrarrecorrido(raiz, 0);
    }

    private void Mostrarrecorrido(NodoBPlus nodo, int nivel)
    {
        string espacios = new string(' ', nivel * 4);
        if (nodo.Hoja)
        {
            Console.WriteLine($"{espacios}Hoja -> [{string.Join(", ", nodo.Claves)}]");
        }
        else
        {
            Console.WriteLine($"{espacios}Nodo Interno -> [{string.Join(", ", nodo.Claves)}]");
            foreach (var hijo in nodo.Hijos)
            {
                Mostrarrecorrido(hijo, nivel + 1);
            }
        }
    }
}