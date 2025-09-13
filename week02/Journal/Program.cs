using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        Journal journal = new Journal();
        PromptGenerator promptGen = new PromptGenerator();

        bool exit = false;
        while (!exit)
        {
            Console.WriteLine("\n--- Menú ---");
            Console.WriteLine("1. Escribir nueva entrada");
            Console.WriteLine("2. Mostrar el diario");
            Console.WriteLine("3. Guardar diario en archivo");
            Console.WriteLine("4. Cargar diario desde archivo");
            Console.WriteLine("5. Salir");
            Console.Write("Elige una opción (1-5): ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    WriteNewEntry(journal, promptGen);
                    break;
                case "2":
                    journal.DisplayEntries();
                    break;
                case "3":
                    SaveJournal(journal);
                    break;
                case "4":
                    LoadJournal(journal);
                    break;
                case "5":
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Opción inválida.");
                    break;
            }
        }
    }

    static void WriteNewEntry(Journal journal, PromptGenerator prompts)
    {
        string prompt = prompts.GetRandomPrompt();
        Console.WriteLine($"\nPrompt: {prompt}");
        Console.WriteLine("Escribe tu respuesta (finaliza con END en una nueva línea):");

        string line;
        List<string> responseLines = new List<string>();
        while ((line = Console.ReadLine()) != null && line.Trim().ToUpper() != "END")
        {
            responseLines.Add(line);
        }

        string response = string.Join("\n", responseLines);
        string dateText = DateTime.Now.ToShortDateString();
        Entry entry = new Entry(dateText, prompt, response);
        journal.AddEntry(entry);

        Console.WriteLine("Entrada añadida.");
    }

    static void SaveJournal(Journal journal)
    {
        Console.Write("Nombre del archivo para guardar: ");
        string filename = Console.ReadLine();
        journal.SaveToFile(filename);
        Console.WriteLine("Diario guardado.");
    }

    static void LoadJournal(Journal journal)
    {
        Console.Write("Nombre del archivo para cargar: ");
        string filename = Console.ReadLine();
        if (File.Exists(filename))
        {
            journal.LoadFromFile(filename);
            Console.WriteLine("Diario cargado.");
        }
        else
        {
            Console.WriteLine("Archivo no encontrado.");
        }
    }
}

class Entry
{
    public string Date { get; set; }
    public string Prompt { get; set; }
    public string Response { get; set; }

    private const string Separator = "~|~";

    public Entry(string date, string prompt, string response)
    {
        Date = date;
        Prompt = prompt;
        Response = response;
    }

    public string ToFileString()
    {
        string safeResponse = Response.Replace("\n", "\\n");
        return $"{Date}{Separator}{Prompt}{Separator}{safeResponse}";
    }

    public static Entry FromFileString(string line)
    {
        string[] parts = line.Split(new string[] { Separator }, StringSplitOptions.None);
        string date = parts[0];
        string prompt = parts[1];
        string response = parts[2].Replace("\\n", "\n");
        return new Entry(date, prompt, response);
    }

    public override string ToString()
    {
        return $"Fecha: {Date}\nPregunta: {Prompt}\nRespuesta:\n{Response}\n";
    }
}

class Journal
{
    private List<Entry> entries = new List<Entry>();

    public void AddEntry(Entry e) => entries.Add(e);

    public void DisplayEntries()
    {
        if (entries.Count == 0)
        {
            Console.WriteLine("El diario está vacío.\n");
            return;
        }

        Console.WriteLine("\n--- Entradas del Diario ---\n");
        foreach (Entry e in entries)
        {
            Console.WriteLine(e);
        }
    }

    public void SaveToFile(string filename)
    {
        using (StreamWriter outputFile = new StreamWriter(filename))
        {
            foreach (Entry e in entries)
            {
                outputFile.WriteLine(e.ToFileString());
            }
        }
    }

    public void LoadFromFile(string filename)
    {
        string[] lines = File.ReadAllLines(filename);
        entries.Clear();
        foreach (string line in lines)
        {
            entries.Add(Entry.FromFileString(line));
        }
    }
}

class PromptGenerator
{
    private List<string> prompts = new List<string>
    {
        "Who was the most interesting person I interacted with today?",
        "What was the best part of my day?",
        "How did I see the hand of the Lord in my life today?",
        "What was the strongest emotion I felt today?",
        "If I had one thing I could do over today, what would it be?"
    };

    private Random rnd = new Random();

    public string GetRandomPrompt()
    {
        int index = rnd.Next(prompts.Count);
        return prompts[index];
    }
}