using System;
using System.Collections.Generic;

public class MaxHeap
{
    private List<Libro> heap = new List<Libro>();

    public int Count => heap.Count;

    public void Limpiar()
    {
        heap.Clear();
    }

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

    public void Mostrar()
    {
        foreach (var libro in heap)
        {
            Console.WriteLine(libro.ToString());
        }
    }
}