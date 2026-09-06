using System;
using System.Collections.Generic;

public class MaxHeap
{
    private List<Libro> heap = new List<Libro>();

    public int Count => heap.Count;

    public void Insertar(Libro valor)
    {
        heap.Add(valor);
        HeapifyUp(heap.Count - 1);
    }

    private void HeapifyUp(int indice)
    {
        while (indice > 0)
        {
            int padre = (indice - 1) / 2;

            if (heap[indice].CompareTo(heap[padre]) > 0)
            {
                Libro temp = heap[indice];
                heap[indice] = heap[padre];
                heap[padre] = temp;
                indice = padre;
            }
            else
            {
                break;
            }
        }
    }

    public Libro ObtenerMaximo()
    {
        if (heap.Count == 0) return null;
        return heap[0];
    }

    public Libro EliminarMaximo()
    {
        if (heap.Count == 0) return null;
        if (heap.Count == 1)
        {
            Libro unico = heap[0];
            heap.RemoveAt(0);
            return unico;
        }

        Libro maximo = heap[0];
        heap[0] = heap[heap.Count - 1];
        heap.RemoveAt(heap.Count - 1);
        HeapifyDown(0);

        return maximo;
    }

    private void HeapifyDown(int indice)
    {
        int cantidad = heap.Count;

        while (true)
        {
            int mayor = indice;
            int hijoIzquierdo = 2 * indice + 1;
            int hijoDerecho = 2 * indice + 2;

            if (hijoIzquierdo < cantidad && heap[hijoIzquierdo].CompareTo(heap[mayor]) > 0)
            {
                mayor = hijoIzquierdo;
            }

            if (hijoDerecho < cantidad && heap[hijoDerecho].CompareTo(heap[mayor]) > 0)
            {
                mayor = hijoDerecho;
            }

            if (mayor == indice) break;

            Libro temp = heap[indice];
            heap[indice] = heap[mayor];
            heap[mayor] = temp;
            indice = mayor;
        }
    }

    public void Mostrar()
    {
        foreach (var libro in heap)
        {
            Console.WriteLine(libro.ToString());
        }
    }
}