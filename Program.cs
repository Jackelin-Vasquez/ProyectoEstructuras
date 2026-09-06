using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        ArbolBPlus arbolBPlus = new ArbolBPlus();
        MinHeap minHeap = new MinHeap();
        MaxHeap maxHeap = new MaxHeap();

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
            Console.WriteLine("3. Mostrar estructura del Árbol B+");
            Console.WriteLine("4. Listar catálogo ordenado por título");
            Console.WriteLine("5. Registrar préstamo de libro");
            Console.WriteLine("6. Registrar devolución de libro");
            Console.WriteLine("7. Mostrar libros menos prestados (Min Heap)");
            Console.WriteLine("8. Mostrar libros más prestados (Max Heap)");
            Console.WriteLine("9. Salir");
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
                        arbolBPlus.Mostrar();
                        break;
                    case 4:
                        ListarPorTitulo(arbolBPlus);
                        break;
                    case 5:
                        RegistrarPrestamo(arbolBPlus, minHeap, maxHeap);
                        break;
                    case 6:
                        RegistrarDevolucion(arbolBPlus, minHeap, maxHeap);
                        break;
                    case 7:
                        Console.WriteLine("\n--- LIBROS MENOS PRESTADOS (MIN HEAP) ---");
                        minHeap.Mostrar();
                        break;
                    case 8:
                        Console.WriteLine("\n--- LIBROS MÁS PRESTADOS (MAX HEAP) ---");
                        maxHeap.Mostrar();
                        break;
                    case 9:
                        Console.WriteLine("\nSaliendo del sistema...");
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

            if (opcion != 9)
            {
                Console.WriteLine("\nPresione cualquier tecla para continuar...");
                Console.ReadKey();
            }

        } while (opcion != 9);
    }

    static void ReconstruirHeaps(ArbolBPlus arbol, MinHeap minH, MaxHeap maxH)
    {
        minH.Limpiar();
        maxH.Limpiar();

        var libros = arbol.ObtenerTodosLosLibros();
        foreach (var libro in libros)
        {
            minH.Insertar(libro);
            maxH.Insertar(libro);
        }
    }

    static void ListarPorTitulo(ArbolBPlus arbol)
    {
        Console.WriteLine("\n--- CATÁLOGO ORDENADO POR TÍTULO ---");
        var libros = arbol.ObtenerTodosLosLibros();
        var librosOrdenados = libros.OrderBy(l => l.Titulo).ToList();

        if (librosOrdenados.Count == 0)
        {
            Console.WriteLine("No hay libros registrados en el sistema.");
            return;
        }

        foreach (var libro in librosOrdenados)
        {
            Console.WriteLine(libro.ToString());
        }
    }

static void RegistrarPrestamo(ArbolBPlus arbol, MinHeap minH, MaxHeap maxH)
    {
        Console.WriteLine("\n--- REGISTRAR PRÉSTAMO ---");
        
        //Para que el usuario pued ver los 
        var libros = arbol.ObtenerTodosLosLibros();
        if (libros.Count == 0)
        {
            Console.WriteLine("No hay libros registrados en el sistema.");
            return;
        }

        Console.WriteLine("Catálogo actual:");
        foreach (var l in libros)
        {
            Console.WriteLine($"  [Código: {l.Codigo}] {l.Titulo} (Disponibles: {l.CopiasDisponibles})");
        }
        Console.WriteLine();

        Console.Write("Ingrese el código del libro a prestar: ");
        if (int.TryParse(Console.ReadLine(), out int codigo))
        {
            Libro libro = arbol.Buscar(codigo);
            if (libro != null)
            {
                if (libro.CopiasDisponibles > 0)
                {
                    libro.CopiasDisponibles--;
                    libro.VecesPrestado++;
                    ReconstruirHeaps(arbol, minH, maxH);
                    Console.WriteLine($"\n¡Préstamo registrado con éxito para '{libro.Titulo}'!");
                    Console.WriteLine($"Copias disponibles restantes: {libro.CopiasDisponibles}");
                    Console.WriteLine($"Veces prestado total: {libro.VecesPrestado}");
                }
                else
                {
                    Console.WriteLine("\nLo sentimos, no hay copias disponibles de este libro para prestar.");
                }
            }
            else
            {
                Console.WriteLine("\nEl libro con ese código no existe en el sistema.");
            }
        }
        else
        {
            Console.WriteLine("Código inválido.");
        }
    }

    static void RegistrarDevolucion(ArbolBPlus arbol, MinHeap minH, MaxHeap maxH)
    {
        Console.WriteLine("\n--- REGISTRAR DEVOLUCIÓN ---");
        
        // Mostramos los libros para facilitar la selección
        var libros = arbol.ObtenerTodosLosLibros();
        if (libros.Count == 0)
        {
            Console.WriteLine("No hay libros registrados en el sistema.");
            return;
        }

        Console.WriteLine("Catálogo actual:");
        foreach (var l in libros)
        {
            Console.WriteLine($"  [Código: {l.Codigo}] {l.Titulo} (Disponibles: {l.CopiasDisponibles})");
        }
        Console.WriteLine();

        Console.Write("Ingrese el código del libro a devolver: ");
        if (int.TryParse(Console.ReadLine(), out int codigo))
        {
            Libro libro = arbol.Buscar(codigo);
            if (libro != null)
            {
                libro.CopiasDisponibles++;
                ReconstruirHeaps(arbol, minH, maxH);
                Console.WriteLine($"\n¡Devolución registrada con éxito para '{libro.Titulo}'!");
                Console.WriteLine($"Copias disponibles actuales: {libro.CopiasDisponibles}");
            }
            else
            {
                Console.WriteLine("\nEl libro con ese código no existe en el sistema.");
            }
        }
        else
        {
            Console.WriteLine("Código inválido.");
        }
    }

    static void CargarDatosIniciales(ArbolBPlus arbol, MinHeap minH, MaxHeap maxH)
    {
        Libro l1 = new Libro(10, "C# Avanzado", "Autor A", Libro.CategoriaLibro.Tecnologia, 5, 12);
        Libro l2 = new Libro(5, "Estructuras de Datos", "Autor B", Libro.CategoriaLibro.Tecnologia, 3, 45);
        Libro l3 = new Libro(20, "Algoritmos", "Autor C", Libro.CategoriaLibro.Ciencia, 2, 3);

        arbol.Insertar(l1);
        arbol.Insertar(l2);
        arbol.Insertar(l3);

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

            // Validar que el código no exista previamente en el árbol
            if (arbol.Buscar(codigo) != null)
            {
                Console.WriteLine($"\nEl código {codigo} ya está registrado en el sistema. Operación cancelada.");
                return;
            }

            Console.Write("Ingrese Título: ");
            string titulo = Console.ReadLine();

            Console.Write("Ingrese Autor: ");
            string autor = Console.ReadLine();

            Console.WriteLine("\nSeleccione la Categoría:");
            Array valoresCategorias = Enum.GetValues(typeof(Libro.CategoriaLibro));
            for (int i = 0; i < valoresCategorias.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {valoresCategorias.GetValue(i)}");
            }
            Console.Write("Opción: ");

            if (int.TryParse(Console.ReadLine(), out int opcionCat) && opcionCat >= 1 && opcionCat <= valoresCategorias.Length)
            {
                Libro.CategoriaLibro categoriaSeleccionada = (Libro.CategoriaLibro)valoresCategorias.GetValue(opcionCat - 1);

                Console.Write("Ingrese Copias Disponibles: ");
                int copias = int.Parse(Console.ReadLine());

                // Se asigna 0 por defecto al registrar un libro nuevo, porque al 
                //ser neuvo no ha tenido ningu prestamo o devolución
                int vecesPrestadoInicial = 0;

                Libro nuevoLibro = new Libro(codigo, titulo, autor, categoriaSeleccionada, copias, vecesPrestadoInicial);

                arbol.Insertar(nuevoLibro);
                minH.Insertar(nuevoLibro);
                maxH.Insertar(nuevoLibro);

                Console.WriteLine("\n¡Libro registrado con éxito con 0 préstamos iniciales!");
            }
            else
            {
                Console.WriteLine("\nCategoría inválida. Operación cancelada.");
            }
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
            Libro libroEncontrado = arbol.Buscar(codigo);
            if (libroEncontrado != null)
            {
                Console.WriteLine($"\n¡Libro encontrado!");
                Console.WriteLine($"Código: {libroEncontrado.Codigo}");
                Console.WriteLine($"Título: {libroEncontrado.Titulo}");
                Console.WriteLine($"Autor: {libroEncontrado.Autor}");
                Console.WriteLine($"Categoría: {libroEncontrado.Categoria}");
                Console.WriteLine($"Copias disponibles: {libroEncontrado.CopiasDisponibles}");
                Console.WriteLine($"Veces prestado: {libroEncontrado.VecesPrestado}");
            }
            else
            {
                Console.WriteLine($"\nEl libro con código {codigo} no existe en el sistema.");
            }
        }
        else
        {
            Console.WriteLine("Código inválido.");
        }
    }
}