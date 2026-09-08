using System;

public class Libro : IComparable<Libro>
{
    public int Codigo { get; set; }
    public string Titulo { get; set; }
    public string Autor { get; set; }
    public CategoriaLibro Categoria { get; set; }
    public int TotalCopias { get; set; }
    public int CopiasDisponibles { get; set; }
    public int VecesPrestado { get; set; }

    public Libro(int codigo, string titulo, string autor, CategoriaLibro categoria, int totalCopias, int copiasDisponibles, int vecesPrestado = 0)
    {
        Codigo = codigo;
        Titulo = titulo;
        Autor = autor;
        Categoria = categoria;
        TotalCopias = totalCopias;
        CopiasDisponibles = copiasDisponibles;
        VecesPrestado = vecesPrestado;
    }

    public Libro(int codigo, string titulo, string autor, CategoriaLibro categoria, int copias, int vecesPrestado = 0)
    {
        Codigo = codigo;
        Titulo = titulo;
        Autor = autor;
        Categoria = categoria;
        TotalCopias = copias;
        CopiasDisponibles = copias;
        VecesPrestado = vecesPrestado;
    }

    public int CompareTo(Libro otro)
    {
        if (otro == null) return 1;
        return this.VecesPrestado.CompareTo(otro.VecesPrestado);
    }

    public override string ToString()
    {
        return $"[Código: {Codigo}] '{Titulo}' por {Autor} | Categoría: {Categoria} | Copias: {CopiasDisponibles}/{TotalCopias} | Préstamos: {VecesPrestado}";
    }

    // Nos permite numerar las opciones de categoria para el lubro a registror
    public enum CategoriaLibro
{
    Tecnologia = 1,
    Ciencia,
    Historia,
    Literatura,
    Ficcion,
    Fantasia
}
}