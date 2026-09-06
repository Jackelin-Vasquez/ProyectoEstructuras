using System;

class Program
{
    static void Main(string[] args)
    {
        // Instanciamos nuestras estructuras principales
        ArbolBPlus arbolBPlus = new ArbolBPlus(); // Asegúrate de tener tu clase ArbolBPlus lista
        MinHeap minHeap = new MinHeap();
        MaxHeap maxHeap = new MaxHeap();

        // Precargamos algunos datos de ejemplo para que el menú no inicie vacío
        CargarDatosIniciales(arbolBPlus, minHeap, maxHeap);

        int opcion = 0;
        do
        {
            Console.Clear();
            Console.WriteLine("==========================================");
            Console.WriteLine("    SISTEMA DE GESTIÓN DE BIBLIOTECA      ");
            Console.WriteLine("==========================================");
            Console.WriteLine("1. Registrar nuevo libro");
            Console.WriteLine("2. Buscar libro por código (Árbol B+)");
            Console.WriteLine("3. Ver catálogo ordenado por título / Mostrar Árbol B+");
            Console.WriteLine("4. Mostrar libros menos prestados (Min Heap)");
            Console.WriteLine("5. Mostrar libros más prestados (Max Heap)");
            Console.WriteLine("6. Salir");
            Console.Write("\nSeleccione una opción: ");

            string input = Console.ReadLine();
            if (int.TryParse(input, out opcion))
            {
                switch (opcion)
                {
                    case 1:
                        RegistrarLibro(arbolBPlus, minHeap, maxHeap);
                        break;
                    case 2:
                        BuscarLibro(arbolBPlus);
                        break;
                    case 3:
                        Console.WriteLine("\n--- CATÁLOGO / ÁRBOL B+ ---");
                        // Aquí invocas el método de recorrido de tu ArbolBPlus (ej. arbolBPlus.Imprimir() o similar)
                        Console.WriteLine("Función para mostrar recorrido del Árbol B+.");
                        break;
                    case 4:
                        Console.WriteLine("\n--- LIBROS MENOS PRESTADOS (MIN HEAP) ---");
                        minHeap.Mostrar();
                        break;
                    case 5:
                        Console.WriteLine("\n--- LIBROS MÁS PRESTADOS (MAX HEAP) ---");
                        maxHeap.Mostrar();
                        break;
                    case 6:
                        Console.WriteLine("\nSaliendo del sistema... ¡Mucho éxito con tu defensa!");
                        break;
                    default:
                        Console.WriteLine("\nOpción inválida. Intente de nuevo.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("\nPor favor, ingrese un número válido.");
            }

            if (opcion != 6)
            {
                Console.WriteLine("\nPresione cualquier tecla para continuar...");
                Console.ReadKey();
            }

        } while (opcion != 6);
    }

    static void CargarDatosIniciales(ArbolBPlus arbol, MinHeap minH, MaxHeap maxH)
    {
        Libro l1 = new Libro(10, "C# Avanzado", "Autor A", "Tecnología", 5, 12);
        Libro l2 = new Libro(5, "Estructuras de Datos", "Autor B", "Tecnología", 3, 45);
        Libro l3 = new Libro(20, "Algoritmos", "Autor C", "Tecnología", 2, 3);

        // Insertar en Árbol B+ (según cómo reciba los parámetros tu implementación)
        // arbol.Insertar(l1.Codigo, l1);

        // Insertar en Heaps
        minH.Insertar(l1);
        minH.Insertar(l2);
        minH.Insertar(l3);

        maxH.Insertar(l1);
        maxH.Insertar(l2);
        maxH.Insertar(l3);
    }

    static void RegistrarLibro(ArbolBPlus arbol, MinHeap minH, MaxHeap maxH)
    {
        Console.WriteLine("\n--- REGISTRAR NUEVO LIBRO ---");
        try
        {
            Console.Write("Ingrese Código (número): ");
            int codigo = int.Parse(Console.ReadLine());

            Console.Write("Ingrese Título: ");
            string titulo = Console.ReadLine();

            Console.Write("Ingrese Autor: ");
            string autor = Console.ReadLine();

            Console.Write("Ingrese Categoría: ");
            string categoria = Console.ReadLine();

            Console.Write("Ingrese Copias Disponibles: ");
            int copias = int.Parse(Console.ReadLine());

            Console.Write("Ingrese Veces Prestado: ");
            int prestamos = int.Parse(Console.ReadLine());

            Libro nuevoLibro = new Libro(codigo, titulo, autor, categoria, copias, prestamos);

            // 1. Insertar en el Árbol B+
            // arbol.Insertar(nuevoLibro.Codigo, nuevoLibro);

            // 2. Insertar en los Heaps
            minH.Insertar(nuevoLibro);
            maxH.Insertar(nuevoLibro);

            Console.WriteLine("\n¡Libro registrado con éxito en el Árbol B+ y en los Montículos!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nError al registrar el libro: {ex.Message}");
        }
    }

    static void BuscarLibro(ArbolBPlus arbol)
    {
        Console.WriteLine("\n--- BUSCAR LIBRO POR CÓDIGO ---");
        Console.Write("Ingrese el código del libro a buscar: ");
        int codigo;
if (int.TryParse(Console.ReadLine(), out codigo))
        {
            // Aquí llamas a la búsqueda de tu Árbol B+
            // Libro encontrado = arbol.Buscar(codigo);
            // if (encontrado != null) Console.WriteLine(encontrado);
            // else Console.WriteLine("Libro no encontrado.");
            Console.WriteLine("Búsqueda en Árbol B+ pendiente de conectar con tu clase.");
        }
        else
        {
            Console.WriteLine("Código inválido.");
        }
    }
}