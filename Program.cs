using System;

class Program
{
    static void Main(string[] args)
    {
        // Creamos un Árbol B+ de orden 4
        ArbolBPlus arbol = new ArbolBPlus(4);

        // Códigos de prueba (simulando los códigos de los libros)
        int[] codigos = { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 };

        Console.WriteLine("--- INSERTANDO CÓDIGOS EN EL ÁRBOL B+ ---");
        foreach (var codigo in codigos)
        {
            arbol.Insertar(codigo);
        }

        // Mostramos la estructura resultante
        Console.WriteLine("\nESTRUCTURA DEL ÁRBOL B+:");
        arbol.Mostrar();

        // Probando búsqueda de un código existente
        int codigoBuscado = 70;
        Console.WriteLine($"\n¿El código {codigoBuscado} existe? {arbol.Buscar(codigoBuscado)}");

        // Probando búsqueda de un código inexistente
        int codigoInexistente = 55;
        Console.WriteLine($"¿El código {codigoInexistente} existe? {arbol.Buscar(codigoInexistente)}");
    }
}