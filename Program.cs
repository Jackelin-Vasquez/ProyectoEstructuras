using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

class Program
{
    private static string archivoDatos = "biblioteca.csv";

    static void Main(string[] args)
    {
        // Asegurar soporte de caracteres especiales/emojis y secuencias ANSI en la consola de Windows
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.CursorVisible = false; // Oculta el cursor parpadeante para que el menú interactivo se vea limpio

        ArbolBPlus arbolBPlus = new ArbolBPlus();
        MinHeap minHeap = new MinHeap();
        MaxHeap maxHeap = new MaxHeap();

        // Aqui se carga lo que ya se tiene guardado antes de abrir el menú
        CargarDesdeArchivo(arbolBPlus, minHeap, maxHeap);

        string[] opciones = {
            "Registrar nuevo libro",
            "Buscar libro por código",
            "Mostrar estructura del Árbol B+",
            "Listar catálogo ordenado por título",
            "Registrar préstamo de libro",
            "Registrar devolución de libro",
            "Mostrar libros menos prestados",
            "Mostrar libros más prestados",
            "Eliminar libro del sistema",
            "Salir del sistema"
        };

        int opcionSeleccionada = 0;
        bool salir = false;

        while (!salir)
        {
            LimpiarPantalla();
            
            // Encabezado con morado (Violeta profundo)
            Console.Write("\u001b[38;2;114;9;183m");
            Console.WriteLine("╔══════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                  SISTEMA DE BIBLIOTECA                   ║");
            Console.WriteLine("║                    Gestión de libros                     ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════════╝");
            Console.Write("\u001b[0m");

            Console.WriteLine();
            Console.Write("\u001b[38;2;114;9;183m");
            Console.WriteLine("  MENÚ PRINCIPAL");
            Console.Write("\u001b[0m");
            Console.WriteLine("  ────────────────────────────────────────────────────────");
            
            // Opciones del menú con navegación interactiva por flechas
            for (int i = 0; i < opciones.Length; i++)
            {
                int numeroOpcion = i + 1;
                string textoOpcion = $"[{numeroOpcion}] {opciones[i]}";

                if (i == opcionSeleccionada)
                {
                    // Resaltar la opción seleccionada como un botón
                    Console.BackgroundColor = ConsoleColor.DarkMagenta;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine($"  ► {textoOpcion.PadRight(52)} ");
                    Console.ResetColor();
                }
                else
                {
                    // Opciones no seleccionadas con  color morado 
                    Console.Write("\u001b[38;2;157;78;221m");
                    Console.WriteLine($"    {textoOpcion}");
                    Console.Write("\u001b[0m");
                }
            }
            
            Console.WriteLine("  ────────────────────────────────────────────────────────");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("   Use las flechas ↑ ↓ para mover entre opciones y Enter para elegir");
            Console.ResetColor();

            // Leer las teclas del usuario
            ConsoleKeyInfo tecla = Console.ReadKey(true);

            if (tecla.Key == ConsoleKey.UpArrow)
            {
                opcionSeleccionada--;
                if (opcionSeleccionada < 0) opcionSeleccionada = opciones.Length - 1; // Vuelve al final si sube desde la primera
            }
            else if (tecla.Key == ConsoleKey.DownArrow)
            {
                opcionSeleccionada++;
                if (opcionSeleccionada >= opciones.Length) opcionSeleccionada = 0; // Vuelve al inicio si baja desde la última
            }
            else if (tecla.Key == ConsoleKey.Enter)
            {
                LimpiarPantalla();
                Console.CursorVisible = true; // Mostra el cursor porque las opciones piden datos por teclado

                switch (opcionSeleccionada)
                {
                    case 0:
                        MostrarTituloSeccion("REGISTRAR NUEVO LIBRO");
                        RegistrarLibro(arbolBPlus, minHeap, maxHeap);
                        GuardarEnArchivo(arbolBPlus);
                        break;
                    case 1:
                        MostrarTituloSeccion("BÚSQUEDA DE LIBRO");
                        BuscarLibro(arbolBPlus);
                        break;
                    case 2:
                        MostrarTituloSeccion("ESTRUCTURA DEL ÁRBOL B+");
                        arbolBPlus.Mostrar();
                        break;
                    case 3:
                        MostrarTituloSeccion("CATÁLOGO ORDENADO POR TÍTULO");
                        ListarPorTitulo(arbolBPlus);
                        break;
                    case 4:
                        MostrarTituloSeccion("REGISTRAR PRÉSTAMO");
                        RegistrarPrestamo(arbolBPlus, minHeap, maxHeap);
                        GuardarEnArchivo(arbolBPlus);
                        break;
                    case 5:
                        MostrarTituloSeccion("REGISTRAR DEVOLUCIÓN");
                        RegistrarDevolucion(arbolBPlus, minHeap, maxHeap);
                        GuardarEnArchivo(arbolBPlus);
                        break;
                    case 6:
                        MostrarTituloSeccion("LIBROS MENOS PRESTADOS");
                        minHeap.Mostrar();
                        break;
                    case 7:
                        MostrarTituloSeccion("LIBROS MÁS PRESTADOS");
                        maxHeap.Mostrar();
                        break;
                    case 8:
                        MostrarTituloSeccion("ELIMINAR LIBRO");
                        EliminarLibroMenu(arbolBPlus, minHeap, maxHeap);
                        GuardarEnArchivo(arbolBPlus);
                        break;
                    case 9:
                        Console.Write("\u001b[38;2;114;9;183m");
                        Console.WriteLine("\n Guardando datos y cerrando sistema... ¡Hasta luego!");
                        Console.Write("\u001b[0m");
                        GuardarEnArchivo(arbolBPlus);
                        salir = true;
                        break;
                }

                if (!salir)
                {
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write("Presione cualquier tecla para volver al menú...");
                    Console.ResetColor();
                    Console.ReadKey(true);
                    Console.CursorVisible = false; // se oculta de nuevo el cursor al regresar al menú principal
                }
            }
        }
    }

    

    static void LimpiarPantalla()
    {
        Console.Write("\u001b[2J\u001b[3J\u001b[H");
        Console.Out.Flush();
    }

    // recuadro dinamico
    static void MostrarTituloSeccion(string titulo)
    {
        // Calcula el tamaño dependiendo de qué tan largo sea el texto para que no se deforme
        int ancho = titulo.Length + 4; // Espacio interno a los lados
        string lineaHorizontal = new string('═', ancho);

        Console.Write("\u001b[38;2;114;9;183m"); // Morado profundo
        Console.WriteLine($"╔{lineaHorizontal}╗");
        Console.WriteLine($"║  {titulo}  ║");
        Console.WriteLine($"╚{lineaHorizontal}╝");
        Console.Write("\u001b[0m");
        Console.WriteLine();
    }

    static void GuardarEnArchivo(ArbolBPlus arbol)
    {
        try
        {
            var libros = arbol.ObtenerTodosLosLibros();
            using (StreamWriter sw = new StreamWriter(archivoDatos))
            {
                // Se escribe la cabecera para que sean los nombres de las columnas de los aatos
                sw.WriteLine("Codigo;Titulo;Autor;Categoria;TotalCopias;CopiasDisponibles;VecesPrestado");

                foreach (var l in libros)
                {
                    // Se utiliza punto y coma (;) para separar campos
                    sw.WriteLine($"{l.Codigo};{l.Titulo};{l.Autor};{(int)l.Categoria};{l.TotalCopias};{l.CopiasDisponibles};{l.VecesPrestado}");
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
        // Si el archivo no existe, inicia vacío
        if (!File.Exists(archivoDatos))
        {
            return;
        }

        try
        {
            string[] lineas = File.ReadAllLines(archivoDatos);
            foreach (var linea in lineas)
            {
                if (string.IsNullOrWhiteSpace(linea)) continue;
                string[] partes = linea.Split(';');
                
                // Se evalúa si el primer campo se puede parsear como número (evita error al leer la cabecera "Codigo")
                if (partes.Length >= 6 && int.TryParse(partes[0], out int codigo))
                {
                    string titulo = partes[1];
                    string autor = partes[2];
                    Libro.CategoriaLibro categoria = (Libro.CategoriaLibro)int.Parse(partes[3]);
                    
                    if (partes.Length == 7)
                    {
                        int totalCopias = int.Parse(partes[4]);
                        int copiasDisponibles = int.Parse(partes[5]);
                        int vecesPrestado = int.Parse(partes[6]);

                        Libro libro = new Libro(codigo, titulo, autor, categoria, totalCopias, copiasDisponibles, vecesPrestado);

                        arbol.Insertar(libro);
                        minH.Insertar(libro);
                        maxH.Insertar(libro);
                    }
                    else if (partes.Length == 6)
                    {
                        int copias = int.Parse(partes[4]);
                        int vecesPrestado = int.Parse(partes[5]);

                        Libro libro = new Libro(codigo, titulo, autor, categoria, copias, vecesPrestado);

                        arbol.Insertar(libro);
                        minH.Insertar(libro);
                        maxH.Insertar(libro);
                    }
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
        // se limpia y se vuelve a meter todo para que los heaps no pierdan el orden con los présta
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
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(" No hay libros registrados en el sistema.");
        Console.ResetColor();
        return;
    }

    Console.Write("\u001b[38;2;157;78;221m");
    Console.WriteLine($" Total de libros en el catálogo: {librosOrdenados.Count}\n");
    Console.Write("\u001b[0m");

    for (int i = 0; i < librosOrdenados.Count; i++)
    {
        var l = librosOrdenados[i];

        // Numeración con el color morado 
        Console.Write("\u001b[38;2;114;9;183m");
        Console.Write($" [{i + 1}] ");
        Console.Write("\u001b[0m");

        // Título del libro en blanco
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write($"'{l.Titulo}'");
        Console.ResetColor();

        // Autor
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine($" — {l.Autor}");
        Console.ResetColor();

        // Detalles con sangría
        Console.Write("      ");
        Console.Write($"Código: {l.Codigo}  |  Categoría: {l.Categoria}  |  ");

        // Color condicional para las copias (Verde si hay, Rojo si está agotado)
        if (l.CopiasDisponibles > 0)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"Disponibles: {l.CopiasDisponibles}/{l.TotalCopias}");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write($"Agotado (0/{l.TotalCopias})");
        }
        Console.ResetColor();

        Console.WriteLine($"  |  Préstamos: {l.VecesPrestado}");
        Console.WriteLine(); // Espacioentre elementos
    }
}

    static void RegistrarPrestamo(ArbolBPlus arbol, MinHeap minH, MaxHeap maxH)
    {
        var libros = arbol.ObtenerTodosLosLibros();
        if (libros.Count == 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("No hay libros registrados en el sistema.");
            Console.ResetColor();
            return;
        }

        Console.WriteLine("Catálogo actual:");
        MostrarListaCompacta(libros); 
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
                    Console.WriteLine($"\n¡Préstamo registrado con éxito para '{libro.Titulo}'!");
                    Console.ResetColor();
                    Console.WriteLine($"Copias disponibles restantes: {libro.CopiasDisponibles}");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n x Lo sentimos, no hay copias disponibles de este libro :(.");
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
            Console.WriteLine("Código inválido :(.");
        }
    }

    static void RegistrarDevolucion(ArbolBPlus arbol, MinHeap minH, MaxHeap maxH)
        {
            var libros = arbol.ObtenerTodosLosLibros();
            if (libros.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No hay libros registrados en el sistema.");
                Console.ResetColor();
                return;
            }

            Console.WriteLine("Catálogo actual:");
            MostrarListaCompacta(libros); 
            Console.WriteLine();

            Console.Write("Ingrese el código del libro a devolver: ");
        if (int.TryParse(Console.ReadLine(), out int codigo))
        {
            Libro libro = arbol.Buscar(codigo);
            if (libro != null)
            {
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
                    Console.WriteLine("\n x Error: Ya se encuentran todas las copias en la biblioteca. No se puede recibir una devolución °^°.");
                    Console.ResetColor();
                }
            }
            else
            {
                Console.WriteLine("\nEl libro con ese código no existe en el sistema °-°");
            }
        }
        else
        {
            Console.WriteLine("Código inválido.");
        }
    }

    static void RegistrarLibro(ArbolBPlus arbol, MinHeap minH, MaxHeap maxH)
    {
        try
        {
            Console.Write("Ingrese Código (número): ");
            int codigo = int.Parse(Console.ReadLine());

            // Valida que no se meta un código repetido
            if (arbol.Buscar(codigo) != null)
            {
                Console.WriteLine($"\nEl código {codigo} ya está registrado en el sistema °^°.");
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

                Libro nuevoLibro = new Libro(codigo, titulo, autor, categoriaSeleccionada, copias, copias, 0);

                arbol.Insertar(nuevoLibro);
                minH.Insertar(nuevoLibro);
                maxH.Insertar(nuevoLibro);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n¡Libro registrado con éxito! :D");
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
                Console.WriteLine("\n ¡Libro encontrado! :D");
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
                Console.WriteLine($"\nEl libro con código {codigo} no existe :(.");
                Console.ResetColor();
            }
        }
        else
        {
            Console.WriteLine("Código inválido.");
        }
    }

    static void EliminarLibroMenu(ArbolBPlus arbol, MinHeap minH, MaxHeap maxH)
    {
        var libros = arbol.ObtenerTodosLosLibros();
        if (libros.Count == 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("No hay libros registrados en el sistema.");
            Console.ResetColor();
            return;
        }

        Console.WriteLine("Catálogo actual:");
        MostrarListaCompacta(libros); 
        Console.WriteLine();

        Console.Write("Ingrese el código del libro que desea eliminar: ");
        if (int.TryParse(Console.ReadLine(), out int codigo))
        {
            Libro libro = arbol.Buscar(codigo);
            if (libro != null)
            {
                arbol.Eliminar(codigo);
                ReconstruirHeaps(arbol, minH, maxH);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\n¡Libro con código {codigo} eliminado con éxito!");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nEl libro con código {codigo} no existe en el sistema D:.");
                Console.ResetColor();
            }
        }
        else
        {
            Console.WriteLine("Código inválido.");
        }
    }

    // para listar libros 
    static void MostrarListaCompacta(List<Libro> libros)
    {
        for (int i = 0; i < libros.Count; i++)
        {
            var l = libros[i];

            // Borde superior de la tarjeta 
            Console.Write("\u001b[38;2;114;9;183m");
            Console.WriteLine("  ┌──────────────────────────────────────────────────────┐");

            // Línea 1: Número de opción y Título del libro
            Console.Write("  │ ");
            Console.Write("\u001b[38;2;157;78;221m");
            Console.Write($"[{i + 1}] ");
            Console.ForegroundColor = ConsoleColor.White;
            string tituloCortado = l.Titulo.Length > 44 ? l.Titulo.Substring(0, 41) + "..." : l.Titulo;
            Console.Write($"{tituloCortado,-44}");
            Console.Write("\u001b[38;2;114;9;183m");
            Console.WriteLine("│");

            // Línea 2: Autor y Categoría
            Console.Write("  │ ");
            Console.ForegroundColor = ConsoleColor.Gray;
            string infoAutor = $" {l.Autor} ({l.Categoria})";
            if (infoAutor.Length > 51) infoAutor = infoAutor.Substring(0, 48) + "...";
            Console.Write($"{infoAutor,-52}");
            Console.Write("\u001b[38;2;114;9;183m");
            Console.WriteLine("│");

            // Línea 3: Código, Estado de Copias y Préstamos
            Console.Write("  │ ");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write(" Codigo: ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write($"{l.Codigo,-5}");

            Console.Write(" │ ");
            if (l.CopiasDisponibles > 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write($"Disponible: {l.CopiasDisponibles}/{l.TotalCopias}   ");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Agotado (0)       ");
            }

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write($"│ Préstamos: ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"{l.VecesPrestado,-3}");
            
            Console.Write("\u001b[38;2;114;9;183m");
            Console.WriteLine(" │");

            // Borde inferior de la tarjeta
            Console.WriteLine("  └──────────────────────────────────────────────────────┘");
            Console.ResetColor();
        }
    }
}