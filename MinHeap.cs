using System;
using System.Collections.Generic;

public class MinHeap<T> where T : IComparable<T>
{
    private List<T> heap = new List<T>();
    public int Count => heap.Count;

    public void Insertar(T valor)
    {
        heap.Add(valor);
        HeapifyUp(heap.Count - 1);
    }

    private void HeapifyUp(int indice)
    {
        while (indice > 0)
        {
            int padre = (indice - 1) / 2;

            // Si el hijo es menor que el padre, los intercambiamos
            if (heap[indice].CompareTo(heap[padre]) < 0)
            {
                T temp = heap[indice];
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
}