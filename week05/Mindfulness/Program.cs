using System;
using System.Collections.Generic;
using System.Threading;
class Activity
{
    private string _name;
    private string _description;
    protected int _durationSeconds;

    public Activity(string name, string description)
    {
        _name = name;
        _description = description;
        _durationSeconds = 0;
    }

    
    public void Start()
    {
        Console.Clear();
        Console.WriteLine($"--- {_name} ---\n");
        Console.WriteLine(_description + "\n");
        _durationSeconds = PromptForDuration();
        Console.WriteLine("\nGet ready...");
        DisplaySpinner(3);
    }

  
    public void End()
    {
        Console.WriteLine("\n");
        Console.WriteLine("Well done!");
        Console.WriteLine($"You have completed the {_durationSeconds} second {_name}.");
        DisplaySpinner(3);
    }

    
    public virtual void Run()
    {
        
    }

   
    private int PromptForDuration()
    {
        while (true)
        {
            Console.Write("Enter the duration of the activity in seconds (e.g., 30): ");
            string input = Console.ReadLine();
            if (int.TryParse(input, out int seconds) && seconds > 0)
            {
                return seconds;
            }
            Console.WriteLine("Please enter a positive integer number of seconds.");
        }
    }

    
    protected void DisplaySpinner(int seconds)
    {
        char[] spinner = new char[] { '|', '/', '-', '\\' };
        DateTime end = DateTime.Now.AddSeconds(seconds);
        int i = 0;
        while (DateTime.Now < end)
        {
            Console.Write(spinner[i % spinner.Length]);
            Thread.Sleep(250);
            Console.Write("\b \b"); // erase spinner char
            i++;
        }
    }

    
    protected void DisplayCountdown(int seconds)
    {
        for (int i = seconds; i >= 1; i--)
        {
            string s = i.ToString();
            Console.Write(s);
            Thread.Sleep(1000);
            
            for (int k = 0; k < s.Length; k++)
            {
                Console.Write("\b \b");
            }
        }
    }
}

// Breathing activity
class BreathingActivity : Activity
{
    public BreathingActivity() : base("Breathing Activity",
        "This activity will help you relax by walking you through breathing in and out slowly. Clear your mind and focus on your breathing.")
    {
    }

    public override void Run()
    {
        Start();

        DateTime endTime = DateTime.Now.AddSeconds(_durationSeconds);

      
        int inhale = 4;
        int exhale = 6;

        while (DateTime.Now < endTime)
        {
            // Breathe in
            Console.WriteLine("\nBreathe in...");
            int remaining = (int)(endTime - DateTime.Now).TotalSeconds;
            int wait = Math.Min(inhale, Math.Max(0, remaining));
            if (wait > 0)
            {
                DisplayCountdown(wait);
            }

            if (DateTime.Now >= endTime) break;

            // Breathe out
            Console.WriteLine("\nBreathe out...");
            remaining = (int)(endTime - DateTime.Now).TotalSeconds;
            wait = Math.Min(exhale, Math.Max(0, remaining));
            if (wait > 0)
            {
                DisplayCountdown(wait);
            }
        }

        End();
    }
}

// Reflection activity
class ReflectionActivity : Activity
{
    private List<string> _prompts = new List<string>
    {
        "Think of a time when you stood up for someone else.",
        "Think of a time when you did something really difficult.",
        "Think of a time when you helped someone in need.",
        "Think of a time when you did something truly selfless.",
        "Think of a time when you felt you really learned something important."
    };

    private List<string> _questions = new List<string>
    {
        "Why was this experience meaningful to you?",
        "Have you ever done anything like this before?",
        "How did you get started?",
        "How did you feel when it was complete?",
        "What made this time different than other times when you were not as successful?",
        "What is your favorite thing about this experience?",
        "What could you learn from this experience that applies to other situations?",
        "What did you learn about yourself through this experience?",
        "How can you keep this experience in mind in the future?"
    };

    private Random _rand = new Random();

    public ReflectionActivity() : base("Reflection Activity",
        "This activity will help you reflect on times in your life when you have shown strength and resilience. It will help you recognize the power you have and how you can use it.")
    {
    }

    public override void Run()
    {
        Start();

        // Show a random prompt
        string prompt = _prompts[_rand.Next(_prompts.Count)];
        Console.WriteLine("\nConsider the following prompt:\n");
        Console.WriteLine($"--- {prompt} ---\n");
        Console.WriteLine("When you have something in mind, press Enter to begin reflecting.");
        Console.ReadLine();

        DateTime endTime = DateTime.Now.AddSeconds(_durationSeconds);

       
        while (DateTime.Now < endTime)
        {
            string question = _questions[_rand.Next(_questions.Count)];
            Console.WriteLine(question);
           
            int remaining = (int)(endTime - DateTime.Now).TotalSeconds;
            int pause = Math.Min(8, Math.Max(1, remaining));
            DisplaySpinner(pause);
            Console.WriteLine(); // move to next line
        }

        End();
    }
}


class ListingActivity : Activity
{
    private List<string> _prompts = new List<string>
    {
        "Who are people that you appreciate?",
        "What are personal strengths of yours?",
        "Who are people that you have helped this week?",
        "When have you felt the Holy Ghost this month?",
        "Who are some of your personal heroes?"
    };

    private Random _rand = new Random();

    public ListingActivity() : base("Listing Activity",
        "This activity will help you reflect on the good things in your life by having you list as many things as you can in a certain area.")
    {
    }

    public override void Run()
    {
        Start();

        string prompt = _prompts[_rand.Next(_prompts.Count)];
        Console.WriteLine("\nList as many responses as you can to the prompt below:");
        Console.WriteLine($"--- {prompt} ---\n");

        Console.WriteLine("You will have a few seconds to prepare.");
        DisplayCountdown(5);
        Console.WriteLine("\nBegin listing. Press Enter after each item. Try to list as many as you can:");

        DateTime endTime = DateTime.Now.AddSeconds(_durationSeconds);
        List<string> items = new List<string>();

       
        while (DateTime.Now < endTime)
        {
            Console.Write("> ");
            string entry = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(entry))
            {
                items.Add(entry.Trim());
            }
           
        }

        Console.WriteLine($"\nYou listed {items.Count} items. Well done!");
        End();
    }
}

// Program with menu
class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello World! This is the Mindfulness Program.");
        Console.WriteLine();

        bool exit = false;

        while (!exit)
        {
            Console.WriteLine("\nSelect an activity:");
            Console.WriteLine("1. Breathing Activity");
            Console.WriteLine("2. Reflection Activity");
            Console.WriteLine("3. Listing Activity");
            Console.WriteLine("4. Quit");
            Console.Write("Enter choice (1-4): ");
            string choice = Console.ReadLine();

            Activity activity = null;

            switch (choice)
            {
                case "1":
                    activity = new BreathingActivity();
                    break;
                case "2":
                    activity = new ReflectionActivity();
                    break;
                case "3":
                    activity = new ListingActivity();
                    break;
                case "4":
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Invalid option. Please choose 1-4.");
                    break;
            }

            if (activity != null)
            {
                activity.Run();
            }
        }

        Console.WriteLine("Thank you for using the Mindfulness Program. Goodbye!");
    }
}