using System;

class Program
{
    static void Main(string[] args)
    {
    public string Date { get; set; }
    public string Prompt { get; set; }
    public string Response { get; set; }

    public Entry(string date, string prompt, string response)
    {
        Date = date;
        Prompt = prompt;
        Response = response;
    }

    public override string ToString()
    {
        return $"{Date} | {Prompt} | {Response}";
    }
}

class Journal
{
    private List<Entry> _entries = new List<Entry>();

    public void AddEntry(string prompt, string response)
    {
        string date = DateTime.Now.ToShortDateString();
        Entry entry = new Entry(date, prompt, response);
        _entries.Add(entry);
    }

    public void DisplayEntries()
    {
        if (_entries.Count == 0)
        {
            Console.WriteLine("No hay entradas en el diario aún.");
            return;
        }

        foreach (Entry entry in _entries)
        {
            Console.WriteLine(entry.ToString());
        }
    }

    public void SaveToFile(string filename)
    {
        using (StreamWriter outputFile = new StreamWriter(filename))
        {
            foreach (Entry entry in _entries)
            {
                outputFile.WriteLine($"{entry.Date}|{entry.Prompt}|{entry.Response}");
            }
        }
        Console.WriteLine($"Diario guardado en: {filename}");
    }

    public void LoadFromFile(string filename)
    {
        if (!File.Exists(filename))
        {
            Console.WriteLine("El archivo no existe.");
            return;
        }

        _entries.Clear();
        string[] lines = File.ReadAllLines(filename);

        foreach (string line in lines)
        {
            string[] parts = line.Split("|");
            if (parts.Length == 3)
            {
                Entry entry = new Entry(parts[0], parts[1], parts[2]);
                _entries.Add(entry);
            }
        }
        Console.WriteLine($"Diario cargado desde: {filename}");
    }
}

class Program
{
    static void Main(string[] args)
    {
        Journal myJournal = new Journal();
        Random random = new Random();

        List<string> prompts = new List<string>
        {
            "¿Quién fue la persona más interesante que conocí hoy?",
            "¿Cuál fue la mejor parte de mi día?",
            "¿Cómo vi la mano de Dios en mi vida hoy?",
            "¿Qué emoción fue la más fuerte que sentí hoy?",
            "Si pudiera repetir una cosa del día de hoy, ¿cuál sería?"
        };

        bool running = true;

        while (running)
        {
            Console.WriteLine("\n--- Journal Program ---");
            Console.WriteLine("1. Escribir una nueva entrada");
            Console.WriteLine("2. Mostrar el diario");
            Console.WriteLine("3. Guardar el diario en un archivo");
            Console.WriteLine("4. Cargar el diario desde un archivo");
            Console.WriteLine("5. Salir");
            Console.Write("Selecciona una opción: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    int index = random.Next(prompts.Count);
                    string prompt = prompts[index];
                    Console.WriteLine($"\n{prompt}");
                    Console.Write("Tu respuesta: ");
                    string response = Console.ReadLine();
                    myJournal.AddEntry(prompt, response);
                    break;

                case "2":
                    myJournal.DisplayEntries();
                    break;

                case "3":
                    Console.Write("Ingresa el nombre del archivo para guardar: ");
                    string saveFile = Console.ReadLine();
                    myJournal.SaveToFile(saveFile);
                    break;

                case "4":
                    Console.Write("Ingresa el nombre del archivo para cargar: ");
                    string loadFile = Console.ReadLine();
                    myJournal.LoadFromFile(loadFile);
                    break;

                case "5":
                    running = false;
                    Console.WriteLine("Saliendo del programa...");
                    break;

                default:
                    Console.WriteLine("Opción inválida. Intenta de nuevo.");
                    break;
            }
        }
    }
}
}