using System;

class Program
{
    static void Main(string[] args)
    {
      List<int> numbers = new List<int>();

        Console.WriteLine("Enter a list of numbers, type 0 when finished.");

        int userNumber = -1;

        // Recolectar números hasta que el usuario escriba 0
        while (userNumber != 0)
        {
            Console.Write("Enter number: ");
            userNumber = int.Parse(Console.ReadLine());

            if (userNumber != 0)
            {
                numbers.Add(userNumber);
            }
        }

        // Calcular la suma
        int sum = 0;
        foreach (int num in numbers)
        {
            sum += num;
        }

        // Calcular el promedio
        double average = (double)sum / numbers.Count;

        // Encontrar el número máximo
        int max = numbers[0];
        foreach (int num in numbers)
        {
            if (num > max)
            {
                max = num;
            }
        }

        // Mostrar resultados
        Console.WriteLine($"The sum is: {sum}");
        Console.WriteLine($"The average is: {average}");
        Console.WriteLine($"The largest number is: {max}");
    }
}