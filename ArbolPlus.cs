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
        this.raiz = new NodoBPlus(true);
    }

    public Libro Buscar(int clave)
    {
        return BuscarRec(raiz, clave);
    }

    private Libro BuscarRec(NodoBPlus nodo, int clave)
    {
        if (nodo.Hoja)
        {
            int index = nodo.Claves.IndexOf(clave);
            if (index != -1)
                return nodo.Valores[index];
            return null;
        }

        int posicion = 0;
        while (posicion < nodo.Claves.Count && clave >= nodo.Claves[posicion])
        {
            posicion++;
        }

        return BuscarRec(nodo.Hijos[posicion], clave);
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
            for (int i = 0; i < nodo.Claves.Count; i++)
            {
                infoLibros.Add($"[{nodo.Claves[i]}: {nodo.Valores[i].Titulo}]");
            }
            Console.WriteLine($"{espacios}Hoja -> {string.Join(", ", infoLibros)}");
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

            var nuevaRaiz = new NodoBPlus(false);
            nuevaRaiz.Claves.Add(claveGuia);
            nuevaRaiz.Hijos.Add(raiz);
            nuevaRaiz.Hijos.Add(nodoDerecho);
            raiz = nuevaRaiz;
        }
    }

    private (int ClaveGuia, NodoBPlus NodoDerecho)? InsertarRec(NodoBPlus nodo, Libro libro)
    {
        if (nodo.Hoja)
        {
            int i = 0;
            while (i < nodo.Claves.Count && nodo.Claves[i] < libro.Codigo)
            {
                i++;
            }

            nodo.Claves.Insert(i, libro.Codigo);
            nodo.Valores.Insert(i, libro);

            if (nodo.Claves.Count <= maxClaves)
                return null;

            return DividirHoja(nodo);
        }

        int posicion = 0;
        while (posicion < nodo.Claves.Count && libro.Codigo >= nodo.Claves[posicion])
        {
            posicion++;
        }

        var resultado = InsertarRec(nodo.Hijos[posicion], libro);
        if (!resultado.HasValue)
            return null;

        var claveGuia = resultado.Value.ClaveGuia;
        var nodoDerecho = resultado.Value.NodoDerecho;

        nodo.Claves.Insert(posicion, claveGuia);
        nodo.Hijos.Insert(posicion + 1, nodoDerecho);

        if (nodo.Claves.Count <= maxClaves)
            return null;

        return DividirInterno(nodo);
    }

    private (int ClaveGuia, NodoBPlus NodoDerecho) DividirHoja(NodoBPlus hoja)
    {
        int punto = hoja.Claves.Count / 2;
        var nuevaHoja = new NodoBPlus(true);

        nuevaHoja.Claves.AddRange(hoja.Claves.GetRange(punto, hoja.Claves.Count - punto));
        nuevaHoja.Valores.AddRange(hoja.Valores.GetRange(punto, hoja.Valores.Count - punto));

        hoja.Claves.RemoveRange(punto, hoja.Claves.Count - punto);
        hoja.Valores.RemoveRange(punto, hoja.Valores.Count - punto);

        nuevaHoja.Siguiente = hoja.Siguiente;
        hoja.Siguiente = nuevaHoja;

        int claveGuia = nuevaHoja.Claves[0];
        return (claveGuia, nuevaHoja);
    }

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