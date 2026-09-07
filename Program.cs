using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

class Program
{
    private static string archivoDatos = "biblioteca.csv";

    static void Main(string[] args)
    {
        // Asegurar soporte de caracteres especiales/emojis en la consola de Windows
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        ArbolBPlus arbolBPlus = new ArbolBPlus();
        MinHeap minHeap = new MinHeap();
        MaxHeap maxHeap = new MaxHeap();

        CargarDesdeArchivo(arbolBPlus, minHeap, maxHeap);

        int opcion = 0;
        do
        {
            Console.Clear();
            
            // Encabezado llamativo con colores
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╔══════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                    SISTEMA DE BIBLIOTECA                 ║");
            Console.WriteLine("║            Gestión con Árbol B+ y Heaps (Min/Max)        ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════════╝");
            Console.ResetColor();

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("  MENÚ PRINCIPAL");
            Console.ResetColor();
            Console.WriteLine("  ────────────────────────────────────────────────────────");
            
            Console.ForegroundColor = ConsoleColor.Green; Console.Write("  [1] "); Console.ResetColor(); Console.WriteLine("Registrar nuevo libro");
            Console.ForegroundColor = ConsoleColor.Green; Console.Write("  [2] "); Console.ResetColor(); Console.WriteLine("Buscar libro por código (Árbol B+)");
            Console.ForegroundColor = ConsoleColor.Green; Console.Write("  [3] "); Console.ResetColor(); Console.WriteLine("Mostrar estructura del Árbol B+");
            Console.ForegroundColor = ConsoleColor.Green; Console.Write("  [4] "); Console.ResetColor(); Console.WriteLine("Listar catálogo ordenado por título");
            Console.ForegroundColor = ConsoleColor.Green; Console.Write("  [5] "); Console.ResetColor(); Console.WriteLine("Registrar préstamo de libro");
            Console.ForegroundColor = ConsoleColor.Green; Console.Write("  [6] "); Console.ResetColor(); Console.WriteLine("Registrar devolución de libro");
            Console.ForegroundColor = ConsoleColor.Green; Console.Write("  [7] "); Console.ResetColor(); Console.WriteLine("Mostrar libros menos prestados (Min Heap)");
            Console.ForegroundColor = ConsoleColor.Green; Console.Write("  [8] "); Console.ResetColor(); Console.WriteLine("Mostrar libros más prestados (Max Heap)");
            Console.ForegroundColor = ConsoleColor.Red;   Console.Write("  [9] "); Console.ResetColor(); Console.WriteLine("Salir del sistema");
            
            Console.WriteLine("  ────────────────────────────────────────────────────────");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("Seleccione una opción: ");
            Console.ResetColor();

            string input = Console.ReadLine();
            if (int.TryParse(input, out opcion))
            {
                Console.Clear();
                switch (opcion)
                {
                    case 1:
                        MostrarTituloSeccion("REGISTRAR NUEVO LIBRO");
                        RegistrarLibro(arbolBPlus, minHeap, maxHeap);
                        GuardarEnArchivo(arbolBPlus);
                        break;
                    case 2:
                        MostrarTituloSeccion("BÚSQUEDA DE LIBRO");
                        BuscarLibro(arbolBPlus);
                        break;
                    case 3:
                        MostrarTituloSeccion("ESTRUCTURA DEL ÁRBOL B+");
                        arbolBPlus.Mostrar();
                        break;
                    case 4:
                        MostrarTituloSeccion("CATÁLOGO ORDENADO POR TÍTULO");
                        ListarPorTitulo(arbolBPlus);
                        break;
                    case 5:
                        MostrarTituloSeccion("REGISTRAR PRÉSTAMO");
                        RegistrarPrestamo(arbolBPlus, minHeap, maxHeap);
                        GuardarEnArchivo(arbolBPlus);
                        break;
                    case 6:
                        MostrarTituloSeccion("REGISTRAR DEVOLUCIÓN");
                        RegistrarDevolucion(arbolBPlus, minHeap, maxHeap);
                        GuardarEnArchivo(arbolBPlus);
                        break;
                    case 7:
                        MostrarTituloSeccion("LIBROS MENOS PRESTADOS (MIN HEAP)");
                        minHeap.Mostrar();
                        break;
                    case 8:
                        MostrarTituloSeccion("LIBROS MÁS PRESTADOS (MAX HEAP)");
                        maxHeap.Mostrar();
                        break;
                    case 9:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\n Guardando datos y cerrando sistema... ¡Hasta luego!");
                        Console.ResetColor();
                        GuardarEnArchivo(arbolBPlus);
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\n x Opción inválida. Intente de nuevo.");
                        Console.ResetColor();
                        break;
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n x Por favor, ingrese un número válido.");
                Console.ResetColor();
            }

            if (opcion != 9)
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("Presione cualquier tecla para volver al menú...");
                Console.ResetColor();
                Console.ReadKey();
            }

        } while (opcion != 9);
    }

    static void MostrarTituloSeccion(string titulo)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"=== {titulo} ===");
        Console.ResetColor();
        Console.WriteLine();
    }

    static void GuardarEnArchivo(ArbolBPlus arbol)
    {
        try
        {
            var libros = arbol.ObtenerTodosLosLibros();
            using (StreamWriter sw = new StreamWriter(archivoDatos))
            {
                foreach (var l in libros)
                {
                    // se guradan 7 campos: Código, Título, Autor, Categoría, TotalCopias, CopiasDisponibles, VecesPrestado
                    sw.WriteLine($"{l.Codigo},{l.Titulo},{l.Autor},{(int)l.Categoria},{l.TotalCopias},{l.CopiasDisponibles},{l.VecesPrestado}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al guardar en el archivo: {ex.Message}");
        }
    }

    static void CargarDesdeArchivo(ArbolBPlus arbol, MinHeap minH, MaxHeap maxH)
    {
        if (!File.Exists(archivoDatos))
        {
            CargarDatosIniciales(arbol, minH, maxH);
            GuardarEnArchivo(arbol);
            return;
        }

        try
        {
            string[] lineas = File.ReadAllLines(archivoDatos);
            foreach (var linea in lineas)
            {
                if (string.IsNullOrWhiteSpace(linea)) continue;
                string[] partes = linea.Split(',');
                
                // se espran 7 partes 
                if (partes.Length == 7)
                {
                    int codigo = int.Parse(partes[0]);
                    string titulo = partes[1];
                    string autor = partes[2];
                    Libro.CategoriaLibro categoria = (Libro.CategoriaLibro)int.Parse(partes[3]);
                    int totalCopias = int.Parse(partes[4]);
                    int copiasDisponibles = int.Parse(partes[5]);
                    int vecesPrestado = int.Parse(partes[6]);

                    Libro libro = new Libro(codigo, titulo, autor, categoria, totalCopias, copiasDisponibles, vecesPrestado);

                    arbol.Insertar(libro);
                    minH.Insertar(libro);
                    maxH.Insertar(libro);
                }
                // Compatibilidad por si tenías registros viejos de 6 columnas en tu CSV antiguo
                else if (partes.Length == 6)
                {
                    int codigo = int.Parse(partes[0]);
                    string titulo = partes[1];
                    string autor = partes[2];
                    Libro.CategoriaLibro categoria = (Libro.CategoriaLibro)int.Parse(partes[3]);
                    int copias = int.Parse(partes[4]);
                    int vecesPrestado = int.Parse(partes[5]);

                    Libro libro = new Libro(codigo, titulo, autor, categoria, copias, vecesPrestado);

                    arbol.Insertar(libro);
                    minH.Insertar(libro);
                    maxH.Insertar(libro);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al leer el archivo: {ex.Message}");
        }
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
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\n✔ ¡Préstamo registrado con éxito para '{libro.Titulo}'!");
                    Console.ResetColor();
                    Console.WriteLine($"Copias disponibles restantes: {libro.CopiasDisponibles}");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n✖ Lo sentimos, no hay copias disponibles de este libro.");
                    Console.ResetColor();
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
    var libros = arbol.ObtenerTodosLosLibros();
    if (libros.Count == 0)
    {
        Console.WriteLine("No hay libros registrados en el sistema.");
        return;
    }

    Console.WriteLine("Catálogo actual:");
    foreach (var l in libros)
    {
        Console.WriteLine($"  [Código: {l.Codigo}] {l.Titulo} (Disponibles: {l.CopiasDisponibles}/{l.TotalCopias})");
    }
    Console.WriteLine();

    Console.Write("Ingrese el código del libro a devolver: ");
    if (int.TryParse(Console.ReadLine(), out int codigo))
    {
        Libro libro = arbol.Buscar(codigo);
        if (libro != null)
        {
            // Validación: No se puede devolver más de lo que la biblioteca posee originalmente
            if (libro.CopiasDisponibles < libro.TotalCopias)
            {
                libro.CopiasDisponibles++;
                ReconstruirHeaps(arbol, minH, maxH);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\n¡Devolución registrada con éxito para '{libro.Titulo}'!");
                Console.ResetColor();
                Console.WriteLine($"Copias disponibles actuales: {libro.CopiasDisponibles}/{libro.TotalCopias}");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n x Error: Ya se encuentran todas las copias en la biblioteca. No se puede recibir una devolución.");
                Console.ResetColor();
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

    static void CargarDatosIniciales(ArbolBPlus arbol, MinHeap minH, MaxHeap maxH)
    {
        Libro l1 = new Libro(10, "C# Avanzado", "Autor A", Libro.CategoriaLibro.Tecnologia, 5, 12);
        Libro l2 = new Libro(5, "Estructuras de Datos", "Autor B", Libro.CategoriaLibro.Tecnologia, 3, 45);
        Libro l3 = new Libro(20, "Algoritmos", "Autor C", Libro.CategoriaLibro.Ciencia, 2, 3);

        arbol.Insertar(l1); arbol.Insertar(l2); arbol.Insertar(l3);
        minH.Insertar(l1); minH.Insertar(l2); minH.Insertar(l3);
        maxH.Insertar(l1); maxH.Insertar(l2); maxH.Insertar(l3);
    }

    static void RegistrarLibro(ArbolBPlus arbol, MinHeap minH, MaxHeap maxH)
    {
        try
        {
            Console.Write("Ingrese Código (número): ");
            int codigo = int.Parse(Console.ReadLine());

            if (arbol.Buscar(codigo) != null)
            {
                Console.WriteLine($"\nEl código {codigo} ya está registrado en el sistema.");
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
                Console.WriteLine($"  {i + 1}. {valoresCategorias.GetValue(i)}");
            }
            Console.Write("Opción: ");

            if (int.TryParse(Console.ReadLine(), out int opcionCat) && opcionCat >= 1 && opcionCat <= valoresCategorias.Length)
            {
                Libro.CategoriaLibro categoriaSeleccionada = (Libro.CategoriaLibro)valoresCategorias.GetValue(opcionCat - 1);

                Console.Write("Ingrese Copias Disponibles: ");
                int copias = int.Parse(Console.ReadLine());

                Libro nuevoLibro = new Libro(codigo, titulo, autor, categoriaSeleccionada, copias, 0);

                arbol.Insertar(nuevoLibro);
                minH.Insertar(nuevoLibro);
                maxH.Insertar(nuevoLibro);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n✔ ¡Libro registrado con éxito!");
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine("\nCategoría inválida.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nError al registrar el libro: {ex.Message}");
        }
    }

    static void BuscarLibro(ArbolBPlus arbol)
    {
        Console.Write("Ingrese el código del libro a buscar: ");
        if (int.TryParse(Console.ReadLine(), out int codigo))
        {
            Libro libroEncontrado = arbol.Buscar(codigo);
            if (libroEncontrado != null)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n✔ ¡Libro encontrado!");
                Console.ResetColor();
                Console.WriteLine($"  • Código: {libroEncontrado.Codigo}");
                Console.WriteLine($"  • Título: {libroEncontrado.Titulo}");
                Console.WriteLine($"  • Autor: {libroEncontrado.Autor}");
                Console.WriteLine($"  • Categoría: {libroEncontrado.Categoria}");
                Console.WriteLine($"  • Copias disponibles: {libroEncontrado.CopiasDisponibles}");
                Console.WriteLine($"  • Veces prestado: {libroEncontrado.VecesPrestado}");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n✖ El libro con código {codigo} no existe.");
                Console.ResetColor();
            }
        }
        else
        {
            Console.WriteLine("Código inválido.");
        }
    }
}