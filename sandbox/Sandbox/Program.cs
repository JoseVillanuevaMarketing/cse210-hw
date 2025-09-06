using System;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello World! This is the Sandbox Project.");
    }
}
using System;

class Program
{
    static void Main(string[] args)
    {
        // Pedir primer nombre
        Console.Write("What is your first name? ");
        string firstName = Console.ReadLine();

        // Pedir apellido
        Console.Write("What is your last name? ");
        string lastName = Console.ReadLine();

        // Mostrar resultado
        Console.WriteLine($"Your name is {lastName}, {firstName} {lastName}.");
    }
}
