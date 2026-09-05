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

    // Método para insertar una clave :D
    public void Insertar(int clave)
    {
        if (Buscar(clave))
        {
            Console.WriteLine($"El código {clave} ya está registrado.");
            return;
        }

        var resultado = InsertarRec(raiz, clave);
        if (resultado.HasValue) // Si la raíz se dividió, se crea una nueva raíz :p
        {
            var claveGuia = resultado.Value.ClaveGuia;
            var nodoDerecho = resultado.Value.NodoDerecho;

            var nuevaRaiz = new NodoBPlus(false);
            nuevaRaiz.Claves.Add(claveGuia);
            nuevaRaiz.Hijos.Add(raiz);
            nuevaRaiz.Hijos.Add(nodoDerecho);
            raiz = nuevaRaiz;
        }
    }

    // Inserción recursiva  :D
    private (int ClaveGuia, NodoBPlus NodoDerecho)? InsertarRec(NodoBPlus nodo, int clave)
    {
        if (nodo.Hoja)
        {
            nodo.Claves.Add(clave);
            nodo.Claves.Sort();

            if (nodo.Claves.Count <= maxClaves)
                return null; // Si cabe, todo bien

            return DividirHoja(nodo); // Si se pasa, partimos la hoja
        }

        int posicion = 0;
        while (posicion < nodo.Claves.Count && clave >= nodo.Claves[posicion])
        {
            posicion++;
        }

        var resultado = InsertarRec(nodo.Hijos[posicion], clave);
        if (!resultado.HasValue)
            return null;

        var claveGuia = resultado.Value.ClaveGuia;
        var nodoDerecho = resultado.Value.NodoDerecho;

        nodo.Claves.Insert(posicion, claveGuia);
        nodo.Hijos.Insert(posicion + 1, nodoDerecho);

        if (nodo.Claves.Count <= maxClaves)
            return null;

        return DividirInterno(nodo); // Si el nodo interno se pasa,se parte
    }

    // Parte una hoja a la mitad cuando se rebalsa
    private (int ClaveGuia, NodoBPlus NodoDerecho) DividirHoja(NodoBPlus hoja)
    {
        int punto = hoja.Claves.Count / 2;
        var nuevaHoja = new NodoBPlus(true);

        nuevaHoja.Claves.AddRange(hoja.Claves.GetRange(punto, hoja.Claves.Count - punto));
        hoja.Claves.RemoveRange(punto, hoja.Claves.Count - punto);

        nuevaHoja.Siguiente = hoja.Siguiente;
        hoja.Siguiente = nuevaHoja;

        int claveGuia = nuevaHoja.Claves[0]; // La primera de la derecha sube como guía
        return (claveGuia, nuevaHoja);
    }

    // Parte un nodo interno cuando se rebalsa
    private (int ClaveGuia, NodoBPlus NodoDerecho) DividirInterno(NodoBPlus nodo)
    {
        int centro = nodo.Claves.Count / 2;
        int claveQueSube = nodo.Claves[centro];

        var nuevoNodo = new NodoBPlus(false);

        nuevoNodo.Claves.AddRange(nodo.Claves.GetRange(centro + 1, nodo.Claves.Count - (centro + 1)));
        nuevoNodo.Hijos.AddRange(nodo.Hijos.GetRange(centro + 1, nodo.Hijos.Count - (centro + 1)));

        nodo.Claves.RemoveRange(centro, nodo.Claves.Count - centro);
        nodo.Hijos.RemoveRange(centro + 1, nodo.Hijos.Count - (centro + 1));

        return (claveQueSube, nuevoNodo);
    }
}

