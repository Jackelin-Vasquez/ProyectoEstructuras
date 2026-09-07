using System;
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
        this.raiz = new NodoBPlus(true, orden);
    }

    public Libro Buscar(int clave)
    {
        return BuscarRec(raiz, clave);
    }

    private Libro BuscarRec(NodoBPlus nodo, int clave)
    {
        if (nodo.Hoja)
        {
            for (int i = 0; i < nodo.Count; i++)
            {
                if (nodo.Claves[i] == clave)
                    return nodo.Valores[i];
            }
            return null;
        }

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

        while (!actual.Hoja)
        {
            actual = actual.Hijos[0];
        }

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
        if (Buscar(libro.Codigo) != null)
        {
            Console.WriteLine($"El código {libro.Codigo} ya está registrado.");
            return;
        }

        var resultado = InsertarRec(raiz, libro);
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
}