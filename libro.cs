using System;

public class Libro : IComparable<Libro>
{
    public int Codigo { get; set; }
    public string Titulo { get; set; }
    public string Autor { get; set; }
    public CategoriaLibro Categoria { get; set; }
    public int TotalCopias { get; set; } //Maximo de copias para que no s epermita devolver mas de los que existen
    public int CopiasDisponibles { get; set; }
    public int VecesPrestado { get; set; }

// Constructor para cuando registras un libro nuevo desde la consola
    public Libro(int codigo, string titulo, string autor, CategoriaLibro categoria, int copiasDisponibles, int vecesPrestado)
    {
        Codigo = codigo;
        Titulo = titulo;
        Autor = autor;
        Categoria = categoria;
        TotalCopias = copiasDisponibles;       // Las que ingresan son el total inicial
        CopiasDisponibles = copiasDisponibles; // Y también las disponibles en el momento
        VecesPrestado = vecesPrestado;
    }

    // Constructor para cuando se carga desde el archivo CSV (permite recuperar el total real y las disponibles 
    // por separado)
    public Libro(int codigo, string titulo, string autor, CategoriaLibro categoria, int totalCopias, int copiasDisponibles, int vecesPrestado)
    {
        Codigo = codigo;
        Titulo = titulo;
        Autor = autor;
        Categoria = categoria;
        TotalCopias = totalCopias;
        CopiasDisponibles = copiasDisponibles;
        VecesPrestado = vecesPrestado;
    }

    //IComparable sirve para que C# sepa cómo comparar, en este caso los lubros :D 
    public int CompareTo(Libro otro) 
    {
        if (otro == null) return 1;     //un libro es mayor que otro si se prestó más veces.
        return this.VecesPrestado.CompareTo(otro.VecesPrestado);
    }

    public override string ToString()
    {
        return $"[{Codigo}] {Titulo} - {Autor} (Prestados: {VecesPrestado}, Copias: {CopiasDisponibles}/{TotalCopias})";
    }

    // Nos permite numerar las opciones de categoria para el lubro a registror
    public enum CategoriaLibro
{
    Tecnologia,
    Ciencia,
    Historia,
    Literatura,
    Ficcion
}
}