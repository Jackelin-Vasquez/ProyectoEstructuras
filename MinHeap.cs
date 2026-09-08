using System;

public class MinHeap
{
    private Libro[] heap;
    private int count;

    public MinHeap(int capacidadInicial = 10)
    {
        // se arracna el array con una capacidad por defecto (10) y contador en 0
        heap = new Libro[capacidadInicial];
        count = 0;
    }

    public int Count => count;

    public void Limpiar()
    {
        // Se boora todo reiniciando el array y el contador
        heap = new Libro[10];
        count = 0;
    }

    public void Insertar(Libro valor)
    {
        // Si ya se oasa del tamaño, toca duplicar espacio con el método de abajo
        if (count >= heap.Length)
        {
            AmpliarCapacidad();
        }
        heap[count] = valor;
        // Sube el elemento si es menor que su padre para mantener la propiedad del min heap
        HeapifyUp(count);
        count++;
    }

    private void AmpliarCapacidad()
    {
        // se crea uno nuevo el doble de grande y se copian los datos viejos
        Libro[] nuevoHeap = new Libro[heap.Length * 2];
        for (int i = 0; i < count; i++)
        {
            nuevoHeap[i] = heap[i];
        }
        heap = nuevoHeap;
    }

    private void HeapifyUp(int indice)
    {
        // se comparan con el padre y si son menores, intercambian
        while (indice > 0)
        {
            int padre = (indice - 1) / 2;

            if (heap[indice].CompareTo(heap[padre]) < 0)
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

    public Libro ExtraerMin()
    {
        if (count == 0) return null;

        // se saca la raíz (el elemento más pequeño)
        Libro raiz = heap[0];
        // Pasa  el último elemento a la raíz y se acomoda hacia abajo
        heap[0] = heap[count - 1];
        heap[count - 1] = null;
        count--;
        HeapifyDown(0);

        return raiz;
    }

    private void HeapifyDown(int indice)
    {
        // se va hacia abajo el elemento por el árbol si es mayor que alguno de sus hijos
        while (true)
        {
            int izquierdo = 2 * indice + 1;
            int derecho = 2 * indice + 2;
            int menor = indice;

            if (izquierdo < count && heap[izquierdo].CompareTo(heap[menor]) < 0)
                menor = izquierdo;
            if (derecho < count && heap[derecho].CompareTo(heap[menor]) < 0)
                menor = derecho;

            if (menor != indice)
            {
                Libro temp = heap[indice];
                heap[indice] = heap[menor];
                heap[menor] = temp;
                indice = menor;
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
            Console.WriteLine("No hay registros en el Min Heap.");
            return;
        }

        // se recorre el heap imprimiendo cada libro
        for (int i = 0; i < count; i++)
        {
            Console.WriteLine(heap[i].ToString());
        }
    }
}