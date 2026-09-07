using System;

public class MaxHeap
{
    private Libro[] heap;
    private int count;

    public MaxHeap(int capacidadInicial = 10)
    {
        heap = new Libro[capacidadInicial];
        count = 0;
    }

    public int Count => count;

    public void Limpiar()
    {
        heap = new Libro[10];
        count = 0;
    }

    public void Insertar(Libro valor)
    {
        if (count >= heap.Length)
        {
            AmpliarCapacidad();
        }
        heap[count] = valor;
        HeapifyUp(count);
        count++;
    }

    private void AmpliarCapacidad()
    {
        Libro[] nuevoHeap = new Libro[heap.Length * 2];
        for (int i = 0; i < count; i++)
        {
            nuevoHeap[i] = heap[i];
        }
        heap = nuevoHeap;
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

    public Libro ExtraerMax()
    {
        if (count == 0) return null;

        Libro raiz = heap[0];
        heap[0] = heap[count - 1];
        heap[count - 1] = null;
        count--;
        HeapifyDown(0);

        return raiz;
    }

    private void HeapifyDown(int indice)
    {
        while (true)
        {
            int izquierdo = 2 * indice + 1;
            int derecho = 2 * indice + 2;
            int mayor = indice;

            if (izquierdo < count && heap[izquierdo].CompareTo(heap[mayor]) > 0)
                mayor = izquierdo;
            if (derecho < count && heap[derecho].CompareTo(heap[mayor]) > 0)
                mayor = derecho;

            if (mayor != indice)
            {
                Libro temp = heap[indice];
                heap[indice] = heap[mayor];
                heap[mayor] = temp;
                indice = mayor;
            }
            else
            {
                break;
            }
        }
    }

    public void Mostrar()
    {
        if (count == 0)
        {
            Console.WriteLine("No hay registros en el Max Heap.");
            return;
        }

        for (int i = 0; i < count; i++)
        {
            Console.WriteLine(heap[i].ToString());
        }
    }
}