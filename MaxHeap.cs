using System;

public class MaxHeap
{
    private Libro[] heap;
    private int count;

    public MaxHeap(int capacidadInicial = 10)
    {
        // Se arranca el array con una capacidad por defecto (10) y el contador en 0
        heap = new Libro[capacidadInicial];
        count = 0;
    }

    public int Count => count;

    public void Limpiar()
    {
        // Se borra todo reiniciando el array y el contador
        heap = new Libro[10];
        count = 0;
    }

    public void Insertar(Libro valor)
    {
        // Si ya se excede el tamaño, se procede a duplicar el espacio con el método de abajo
        if (count >= heap.Length)
        {
            AmpliarCapacidad();
        }
        heap[count] = valor;
        // Se sube el elemento si es mayor que su padre para mantener la propiedad del max heap
        HeapifyUp(count);
        count++;
    }

    private void AmpliarCapacidad()
    {
        // Se crea uno nuevo con el doble de tamaño y se copian los dats anteriores
        Libro[] nuevoHeap = new Libro[heap.Length * 2];
        for (int i = 0; i < count; i++)
        {
            nuevoHeap[i] = heap[i];
        }
        heap = nuevoHeap;
    }

    private void HeapifyUp(int indice)
    {
        // Se compara con e padre y si se es mayor se intercambian
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

        // Se extrae la raíz (el elemento más grande)
        Libro raiz = heap[0];
        // Se pasa el último elemento a la raíz y se acomoda hacia abajo
        heap[0] = heap[count - 1];
        heap[count - 1] = null;
        count--;
        HeapifyDown(0);

        return raiz;
    }

    private void HeapifyDown(int indice)
    {
        // Se va bajando el elemento por el árbol si es menor que alguno de sus hijos
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

        // Se recorre el heap imprimiendo cada libro
        for (int i = 0; i < count; i++)
        {
            Console.WriteLine(heap[i].ToString());
        }
    }
}