using System;

public class Libro : IComparable<Libro>
{
    public int Codigo { get; set; }
    public string Titulo { get; set; }
    public string Autor { get; set; }
    public CategoriaLibro Categoria { get; set; }
    public int CopiasDisponibles { get; set; }
    public int VecesPrestado { get; set; }

    public Libro(int codigo, string titulo, string autor, CategoriaLibro categoria, int copias, int vecesPrestado)
    {
        Codigo = codigo;
        Titulo = titulo;
        Autor = autor;
        Categoria = categoria;
        CopiasDisponibles = copias;
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
        return $"[{Codigo}] {Titulo} - {Autor} (Prestados: {VecesPrestado}, Copias: {CopiasDisponibles})";
    }

    public enum CategoriaLibro
{
    Tecnologia,
    Ciencia,
    Historia,
    Literatura,
    Ficcion
}
}