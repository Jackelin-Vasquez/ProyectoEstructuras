using System;
using System.Collections.Generic;

public class ArbolBPlus
{
    private int orden;
    private int maxClaves;
    private NodoBPlus raiz;

    public ArbolBPlus(int orden = 4)
    {
        // :D Se inicializan los parámetros principales del árbol y se crea una raíz que por defecto es hoja
        this.orden = orden;
        this.maxClaves = orden - 1;
        this.raiz = new NodoBPlus(true, orden);
    }

    public Libro Buscar(int clave)
    {
        // Se delega la búsqueda a un método recursivo auxiliar :p
        return BuscarRec(raiz, clave);
    }

    private Libro BuscarRec(NodoBPlus nodo, int clave)
    {
        // Si se llega a un nodo hoja, se recorren las claves para encontrar el libro coincidente
        if (nodo.Hoja)
        {
            for (int i = 0; i < nodo.Count; i++)
            {
                if (nodo.Claves[i] == clave)
                    return nodo.Valores[i];
            }
            return null;
        }

        // Si es un nodo interno, se busca la posición adecuada para descender por el hijo correspondiente :]
        int posicion = 0;
        while (posicion < nodo.Count && clave >= nodo.Claves[posicion])
        {
            posicion++;
        }

        return BuscarRec(nodo.Hijos[posicion], clave);
    }

    public List<Libro> ObtenerTodosLosLibros()
    {
        List<Libro> listaLibros = new List<Libro>();
        NodoBPlus actual = raiz;

        // Se desciende hasta el primer nodo hoja ubicado más a la izquierda
        while (!actual.Hoja)
        {
            actual = actual.Hijos[0];
        }

        // Se aprovecha el enlace encadenado entre hojas para recorrer todo el catálogo de manera secuencial
        while (actual != null)
        {
            for (int i = 0; i < actual.Count; i++)
            {
                listaLibros.Add(actual.Valores[i]);
            }
            actual = actual.Siguiente;
        }

        return listaLibros;
    }

    public void Mostrar()
    {
        // Se inicia el recorrido gráfico de la estructura desde la raíz en el nivel 0 
        Mostrarrecorrido(raiz, 0);
    }

    private void Mostrarrecorrido(NodoBPlus nodo, int nivel)
    {
        string espacios = new string(' ', nivel * 4);
        if (nodo.Hoja)
        {
            var infoLibros = new List<string>();
            for (int i = 0; i < nodo.Count; i++)
            {
                infoLibros.Add($"[{nodo.Claves[i]}: {nodo.Valores[i].Titulo}]");
            }
            Console.WriteLine($"{espacios}Hoja -> {string.Join(", ", infoLibros)}");
        }
        else
        {
            var clavesInternas = new List<string>();
            for (int i = 0; i < nodo.Count; i++)
            {
                clavesInternas.Add(nodo.Claves[i].ToString());
            }
            Console.WriteLine($"{espacios}Nodo Interno -> [{string.Join(", ", clavesInternas)}]");
            
            for (int i = 0; i <= nodo.Count; i++)
            {
                if (nodo.Hijos[i] != null)
                {
                    Mostrarrecorrido(nodo.Hijos[i], nivel + 1);
                }
            }
        }
    }

    public void Insertar(Libro libro)
    {
        // Se valida que el código no existaantes para evitar duplicados en el árbol 
        if (Buscar(libro.Codigo) != null)
        {
            Console.WriteLine($"El código {libro.Codigo} ya está registrado.");
            return;
        }

        var resultado = InsertarRec(raiz, libro);
        // Si la inserción genera una partición en la raíz, se crea una nueva raíz que es superior
        if (resultado.HasValue)
        {
            var claveGuia = resultado.Value.ClaveGuia;
            var nodoDerecho = resultado.Value.NodoDerecho;

            var nuevaRaiz = new NodoBPlus(false, orden);
            nuevaRaiz.Claves[0] = claveGuia;
            nuevaRaiz.Hijos[0] = raiz;
            nuevaRaiz.Hijos[1] = nodoDerecho;
            nuevaRaiz.Count = 1;
            raiz = nuevaRaiz;
        }
    }

    private (int ClaveGuia, NodoBPlus NodoDerecho)? InsertarRec(NodoBPlus nodo, Libro libro)
    {
        if (nodo.Hoja)
        {
            int i = 0;
            while (i < nodo.Count && nodo.Claves[i] < libro.Codigo)
            {
                i++;
            }

            for (int j = nodo.Count; j > i; j--)
            {
                nodo.Claves[j] = nodo.Claves[j - 1];
                nodo.Valores[j] = nodo.Valores[j - 1];
            }

            nodo.Claves[i] = libro.Codigo;
            nodo.Valores[i] = libro;
            nodo.Count++;

            if (nodo.Count <= maxClaves)
                return null;

            return DividirHoja(nodo);
        }

        int posicion = 0;
        while (posicion < nodo.Count && libro.Codigo >= nodo.Claves[posicion])
        {
            posicion++;
        }

        var resultado = InsertarRec(nodo.Hijos[posicion], libro);
        if (!resultado.HasValue)
            return null;

        var claveGuia = resultado.Value.ClaveGuia;
        var nodoDerecho = resultado.Value.NodoDerecho;

        int k = 0;
        while (k < nodo.Count && nodo.Claves[k] < claveGuia)
        {
            k++;
        }

        for (int j = nodo.Count; j > k; j--)
        {
            nodo.Claves[j] = nodo.Claves[j - 1];
        }
        for (int j = nodo.Count + 1; j > k + 1; j--)
        {
            nodo.Hijos[j] = nodo.Hijos[j - 1];
        }

        nodo.Claves[k] = claveGuia;
        nodo.Hijos[k + 1] = nodoDerecho;
        nodo.Count++;

        if (nodo.Count <= maxClaves)
            return null;

        return DividirInterno(nodo);
    }

    private (int ClaveGuia, NodoBPlus NodoDerecho) DividirHoja(NodoBPlus hoja)
    {
        int punto = hoja.Count / 2;
        var nuevaHoja = new NodoBPlus(true, orden);

        int elementosANuevo = hoja.Count - punto;
        for (int i = 0; i < elementosANuevo; i++)
        {
            nuevaHoja.Claves[i] = hoja.Claves[punto + i];
            nuevaHoja.Valores[i] = hoja.Valores[punto + i];
            hoja.Claves[punto + i] = 0;
            hoja.Valores[punto + i] = null;
        }

        nuevaHoja.Count = elementosANuevo;
        hoja.Count = punto;

        // Se actualizan los punteros de la lista enlazada de hojas
        nuevaHoja.Siguiente = hoja.Siguiente;
        hoja.Siguiente = nuevaHoja;

        int claveGuia = nuevaHoja.Claves[0];
        return (claveGuia, nuevaHoja);
    }

    private (int ClaveGuia, NodoBPlus NodoDerecho) DividirInterno(NodoBPlus nodo)
    {
        int centro = nodo.Count / 2;
        int claveQueSube = nodo.Claves[centro];
        nodo.Claves[centro] = 0;

        var nuevoNodo = new NodoBPlus(false, orden);

        int elementosANuevo = nodo.Count - (centro + 1);
        for (int i = 0; i < elementosANuevo; i++)
        {
            nuevoNodo.Claves[i] = nodo.Claves[centro + 1 + i];
            nodo.Claves[centro + 1 + i] = 0;
        }

        for (int i = 0; i <= elementosANuevo; i++)
        {
            nuevoNodo.Hijos[i] = nodo.Hijos[centro + 1 + i];
            nodo.Hijos[centro + 1 + i] = null;
        }

        nuevoNodo.Count = elementosANuevo;
        nodo.Count = centro;

        return (claveQueSube, nuevoNodo);
    }

    private int minClaves => (orden - 1) / 2;

    public void Eliminar(int clave)
    {
        // Se valida que el elemento exista antes de seguir con la eliminación
        if (Buscar(clave) == null)
        {
            Console.WriteLine($"El código {clave} no existe en el sistema.");
            return;
        }

        EliminarRec(raiz, clave);

        // Si la raíz no es hoja y se queda sin claves, su único hijo pasa a ser la nueva raíz del árbol
        if (!raiz.Hoja && raiz.Count == 0 && raiz.Hijos[0] != null)
        {
            raiz = raiz.Hijos[0];
        }
        
        Console.WriteLine($"Libro con código {clave} eliminado correctamente del Árbol.");
    }

    private void EliminarRec(NodoBPlus nodo, int clave)
    {
        if (nodo.Hoja)
        {
            int idx = -1;
            for (int i = 0; i < nodo.Count; i++)
            {
                if (nodo.Claves[i] == clave)
                {
                    idx = i;
                    break;
                }
            }

            if (idx != -1)
            {
                for (int i = idx; i < nodo.Count - 1; i++)
                {
                    nodo.Claves[i] = nodo.Claves[i + 1];
                    nodo.Valores[i] = nodo.Valores[i + 1];
                }
                nodo.Claves[nodo.Count - 1] = 0;
                nodo.Valores[nodo.Count - 1] = null;
                nodo.Count--;
            }
            return;
        }

        int posicion = 0;
        while (posicion < nodo.Count && clave >= nodo.Claves[posicion])
        {
            posicion++;
        }

        NodoBPlus hijo = nodo.Hijos[posicion];
        EliminarRec(hijo, clave);

        // Se verifica si ocurre un subdesbordamiento (underflow) en el hijo tras la eliminación
        if (hijo.Count < minClaves)
        {
            ManejarUnderflow(nodo, posicion);
        }
    }

    private void ManejarUnderflow(NodoBPlus padre, int hijoIdx)
    {
        NodoBPlus hijo = padre.Hijos[hijoIdx];
        NodoBPlus hermanoIzquierda = (hijoIdx > 0) ? padre.Hijos[hijoIdx - 1] : null;
        NodoBPlus hermanoDerecha = (hijoIdx < padre.Count) ? padre.Hijos[hijoIdx + 1] : null;

        if (hijo.Hoja)
        {
            // Se intenta tomar prestado un elemento del hermano izquierdo si este tiene excedente
            if (hermanoIzquierda != null && hermanoIzquierda.Count > minClaves)
            {
                for (int i = hijo.Count; i > 0; i--)
                {
                    hijo.Claves[i] = hijo.Claves[i - 1];
                    hijo.Valores[i] = hijo.Valores[i - 1];
                }
                hijo.Claves[0] = hermanoIzquierda.Claves[hermanoIzquierda.Count - 1];
                hijo.Valores[0] = hermanoIzquierda.Valores[hermanoIzquierda.Count - 1];
                hermanoIzquierda.Claves[hermanoIzquierda.Count - 1] = 0;
                hermanoIzquierda.Valores[hermanoIzquierda.Count - 1] = null;
                hermanoIzquierda.Count--;
                hijo.Count++;
                padre.Claves[hijoIdx - 1] = hijo.Claves[0];
            }
            // Se intenta tomar prestado un elemento del hermano derecho si este tiene excedente
            else if (hermanoDerecha != null && hermanoDerecha.Count > minClaves)
            {
                hijo.Claves[hijo.Count] = hermanoDerecha.Claves[0];
                hijo.Valores[hijo.Count] = hermanoDerecha.Valores[0];
                hijo.Count++;

                for (int i = 0; i < hermanoDerecha.Count - 1; i++)
                {
                    hermanoDerecha.Claves[i] = hermanoDerecha.Claves[i + 1];
                    hermanoDerecha.Valores[i] = hermanoDerecha.Valores[i + 1];
                }
                hermanoDerecha.Claves[hermanoDerecha.Count - 1] = 0;
                hermanoDerecha.Valores[hermanoDerecha.Count - 1] = null;
                hermanoDerecha.Count--;
                padre.Claves[hijoIdx] = hermanoDerecha.Claves[0];
            }
            // Se realiza la fusión con el hermano izquierdo o derecho si no se puede redistribuir
            else if (hermanoIzquierda != null)
            {
                for (int i = 0; i < hijo.Count; i++)
                {
                    hermanoIzquierda.Claves[hermanoIzquierda.Count + i] = hijo.Claves[i];
                    hermanoIzquierda.Valores[hermanoIzquierda.Count + i] = hijo.Valores[i];
                }
                hermanoIzquierda.Count += hijo.Count;
                hermanoIzquierda.Siguiente = hijo.Siguiente;

                // Se remueve la clave correspondiente del nodo padre
                for (int i = hijoIdx - 1; i < padre.Count - 1; i++)
                {
                    padre.Claves[i] = padre.Claves[i + 1];
                    padre.Hijos[i + 1] = padre.Hijos[i + 2];
                }
                padre.Claves[padre.Count - 1] = 0;
                padre.Hijos[padre.Count] = null;
                padre.Count--;
            }
            else if (hermanoDerecha != null)
            {
                for (int i = 0; i < hermanoDerecha.Count; i++)
                {
                    hijo.Claves[hijo.Count + i] = hermanoDerecha.Claves[i];
                    hijo.Valores[hijo.Count + i] = hermanoDerecha.Valores[i];
                }
                hijo.Count += hermanoDerecha.Count;
                hijo.Siguiente = hermanoDerecha.Siguiente;

                for (int i = hijoIdx; i < padre.Count - 1; i++)
                {
                    padre.Claves[i] = padre.Claves[i + 1];
                    padre.Hijos[i + 1] = padre.Hijos[i + 2];
                }
                padre.Claves[padre.Count - 1] = 0;
                padre.Hijos[padre.Count] = null;
                padre.Count--;
            }
        }
    }
}