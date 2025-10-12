using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;



abstract class Goal
{
   
    private string _title;
    private string _description;
    private int _pointsForCompletion; 

    protected Goal(string title, string description, int pointsForCompletion)
    {
        _title = title;
        _description = description;
        _pointsForCompletion = pointsForCompletion;
    }

    public string Title => _title;
    public string Description => _description;
    public int PointsForCompletion => _pointsForCompletion;

    public abstract string GetStatusString();

    public abstract int RecordEvent();

    public abstract bool IsComplete();

    public abstract string ToDataString();

    public static Goal CreateFromDataString(string dataLine)
    {
        
        if (string.IsNullOrWhiteSpace(dataLine)) return null;
        var parts = dataLine.Split(':', 2);
        if (parts.Length < 2) return null;
        string type = parts[0];
        string payload = parts[1];

        string[] fields = payload.Split('|');

        try
        {
            switch (type)
            {
                case "Simple":
                    
                    if (fields.Length >= 4)
                    {
                        string t = fields[0];
                        string d = fields[1];
                        int pts = int.Parse(fields[2]);
                        bool done = bool.Parse(fields[3]);
                        var g = new SimpleGoal(t, d, pts);
                        if (done) g.MarkComplete(); 
                        return g;
                    }
                    break;

                case "Eternal":
                  
                    if (fields.Length >= 3)
                    {
                        string t = fields[0];
                        string d = fields[1];
                        int pts = int.Parse(fields[2]);
                        return new EternalGoal(t, d, pts);
                    }
                    break;

                case "Checklist":
                    
                    if (fields.Length >= 6)
                    {
                        string t = fields[0];
                        string d = fields[1];
                        int ptsPer = int.Parse(fields[2]);
                        int target = int.Parse(fields[3]);
                        int bonus = int.Parse(fields[4]);
                        int current = int.Parse(fields[5]);
                        var g = new ChecklistGoal(t, d, ptsPer, target, bonus);
                        g.SetCurrentCount(current);
                        return g;
                    }
                    break;
            }
        }
        catch
        {
           
            return null;
        }

        return null;
    }
}


class SimpleGoal : Goal
{
    private bool _isComplete;

    public SimpleGoal(string title, string description, int points) : base(title, description, points)
    {
        _isComplete = false;
    }

    public override string GetStatusString()
    {
        return _isComplete ? "[X]" : "[ ]";
    }

    public override int RecordEvent()
    {
        if (_isComplete)
        {
            Console.WriteLine("This goal is already completed; no points awarded.");
            return 0;
        }
        else
        {
            _isComplete = true;
            Console.WriteLine($"Goal completed: {Title} (+{PointsForCompletion} points)");
            return PointsForCompletion;
        }
    }

    public void MarkComplete()
    {
        _isComplete = true;
    }

    public override bool IsComplete()
    {
        return _isComplete;
    }

    public override string ToDataString()
    {
        
        return $"Simple:{Title}|{Description}|{PointsForCompletion}|{_isComplete}";
    }
}


class EternalGoal : Goal
{
    public EternalGoal(string title, string description, int pointsPerRecording) : base(title, description, pointsPerRecording)
    {
    }

    public override string GetStatusString()
    {
        return "[~]"; 
    }

    public override int RecordEvent()
    {
        Console.WriteLine($"Recorded eternal goal: {Title} (+{PointsForCompletion} points)");
        return PointsForCompletion;
    }

    public override bool IsComplete()
    {
        return false;
    }

    public override string ToDataString()
    {
        
        return $"Eternal:{Title}|{Description}|{PointsForCompletion}";
    }
}

class ChecklistGoal : Goal
{
    private int _currentCount;
    private int _targetCount;
    private int _bonusPoints;

    public ChecklistGoal(string title, string description, int pointsPerRecording, int targetCount, int bonusPoints)
        : base(title, description, pointsPerRecording)
    {
        _currentCount = 0;
        _targetCount = targetCount;
        _bonusPoints = bonusPoints;
    }

    public override string GetStatusString()
    {
        string doneFlag = (_currentCount >= _targetCount) ? "[X]" : "[ ]";
        return $"{doneFlag} ({_currentCount}/{_targetCount})";
    }

    public override int RecordEvent()
    {
        if (_currentCount >= _targetCount)
        {
            Console.WriteLine("This checklist goal is already fully completed; no points awarded.");
            return 0;
        }

        _currentCount++;
        int awarded = PointsForCompletion;

        Console.WriteLine($"Recorded checklist progress for: {Title} (+{PointsForCompletion} points). Progress {_currentCount}/{_targetCount}");

        if (_currentCount >= _targetCount)
        {
            awarded += _bonusPoints;
            Console.WriteLine($"Congratulations! You completed the checklist goal and earned a bonus of {_bonusPoints} points!");
        }

        return awarded;
    }

    public override bool IsComplete()
    {
        return _currentCount >= _targetCount;
    }

    public void SetCurrentCount(int count)
    {
        _currentCount = count;
    }

    public override string ToDataString()
    {
        
        return $"Checklist:{Title}|{Description}|{PointsForCompletion}|{_targetCount}|{_bonusPoints}|{_currentCount}";
    }
}


class GoalManager
{
    private List<Goal> _goals;
    private int _score;

    public GoalManager()
    {
        _goals = new List<Goal>();
        _score = 0;
    }

    public int Score => _score;
    public IReadOnlyList<Goal> Goals => _goals.AsReadOnly();

    public void AddGoal(Goal g)
    {
        if (g != null) _goals.Add(g);
    }

    public void RemoveGoalAt(int index)
    {
        if (index >= 0 && index < _goals.Count) _goals.RemoveAt(index);
    }

    public void ListGoals()
    {
        Console.WriteLine("\nYour Goals:");
        if (_goals.Count == 0)
        {
            Console.WriteLine("  (No goals yet)");
            return;
        }

        for (int i = 0; i < _goals.Count; i++)
        {
            var g = _goals[i];
            Console.WriteLine($"{i + 1}. {g.GetStatusString()} {g.Title} - {g.Description}");
        }
    }

  
    public void RecordEvent(int index)
    {
        if (index < 1 || index > _goals.Count)
        {
            Console.WriteLine("Invalid goal selection.");
            return;
        }

        Goal g = _goals[index - 1];
        int earned = g.RecordEvent();
        if (earned > 0)
        {
            _score += earned;
            Console.WriteLine($"Total score is now: {_score}");
        }
    }

   
    public bool SaveToFile(string filename)
    {
        try
        {
            using (StreamWriter sw = new StreamWriter(filename))
            {
                sw.WriteLine(_score); 
                foreach (var g in _goals)
                {
                    sw.WriteLine(g.ToDataString());
                }
            }
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving file: {ex.Message}");
            return false;
        }
    }

    
    public bool LoadFromFile(string filename)
    {
        try
        {
            if (!File.Exists(filename))
            {
                Console.WriteLine("File not found.");
                return false;
            }

            string[] lines = File.ReadAllLines(filename);
            var newGoals = new List<Goal>();
            int newScore = 0;

            if (lines.Length > 0)
            {
                
                if (!int.TryParse(lines[0], out newScore))
                {
                    newScore = 0;
                }

                for (int i = 1; i < lines.Length; i++)
                {
                    string line = lines[i].Trim();
                    if (string.IsNullOrEmpty(line)) continue;
                    Goal g = Goal.CreateFromDataString(line);
                    if (g != null) newGoals.Add(g);
                }
            }

            
            _goals = newGoals;
            _score = newScore;
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading file: {ex.Message}");
            return false;
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello World! This is the Eternal Quest Program.");
        Console.WriteLine("Welcome to Eternal Quest — track goals, earn points, and level up!");
        var manager = new GoalManager();

        bool quit = false;
        while (!quit)
        {
            ShowMenu();
            Console.Write("Choose an option (1-8): ");
            string choice = Console.ReadLine()?.Trim();

            switch (choice)
            {
                case "1":
                    CreateGoalMenu(manager);
                    break;
                case "2":
                    manager.ListGoals();
                    break;
                case "3":
                    RecordEventMenu(manager);
                    break;
                case "4":
                    Console.WriteLine($"\nYour current score: {manager.Score} points\n");
                    break;
                case "5":
                    SaveMenu(manager);
                    break;
                case "6":
                    LoadMenu(manager);
                    break;
                case "7":
                    DeleteGoalMenu(manager);
                    break;
                case "8":
                    quit = true;
                    break;
                default:
                    Console.WriteLine("Invalid choice — please select from the menu.");
                    break;
            }
        }

        Console.WriteLine("Thanks for using Eternal Quest. Goodbye!");
    }

    static void ShowMenu()
    {
        Console.WriteLine("\nMenu:");
        Console.WriteLine("1. Create a new goal");
        Console.WriteLine("2. List goals");
        Console.WriteLine("3. Record event (mark progress / complete goal)");
        Console.WriteLine("4. Show score");
        Console.WriteLine("5. Save goals to file");
        Console.WriteLine("6. Load goals from file");
        Console.WriteLine("7. Delete a goal");
        Console.WriteLine("8. Quit");
    }

    static void CreateGoalMenu(GoalManager manager)
    {
        Console.WriteLine("\nCreate Goal:");
        Console.WriteLine("a. Simple Goal (one-time)");
        Console.WriteLine("b. Eternal Goal (repeatable)");
        Console.WriteLine("c. Checklist Goal (N times + bonus)");
        Console.Write("Choose type (a/b/c): ");
        string type = Console.ReadLine()?.Trim().ToLower();

        Console.Write("Title: ");
        string title = Console.ReadLine()?.Trim() ?? "";
        Console.Write("Description: ");
        string desc = Console.ReadLine()?.Trim() ?? "";

        switch (type)
        {
            case "a":
                int pts = PromptInt("Points awarded when completed (e.g., 100): ");
                var sg = new SimpleGoal(title, desc, pts);
                manager.AddGoal(sg);
                Console.WriteLine("Simple goal created.");
                break;

            case "b":
                int ptsPer = PromptInt("Points awarded each time it's recorded (e.g., 50): ");
                var eg = new EternalGoal(title, desc, ptsPer);
                manager.AddGoal(eg);
                Console.WriteLine("Eternal goal created.");
                break;

            case "c":
                int ptsEach = PromptInt("Points awarded per recording (e.g., 20): ");
                int target = PromptInt("Target count to complete goal (e.g., 10): ");
                int bonus = PromptInt("Bonus points when target reached (e.g., 200): ");
                var cg = new ChecklistGoal(title, desc, ptsEach, target, bonus);
                manager.AddGoal(cg);
                Console.WriteLine("Checklist goal created.");
                break;

            default:
                Console.WriteLine("Unknown type. Aborting creation.");
                break;
        }
    }

    static void RecordEventMenu(GoalManager manager)
    {
        if (manager.Goals.Count == 0)
        {
            Console.WriteLine("No goals to record. Create one first.");
            return;
        }

        manager.ListGoals();
        int choice = PromptInt($"Select a goal number to record (1-{manager.Goals.Count}): ");
        manager.RecordEvent(choice);
    }

    static void SaveMenu(GoalManager manager)
    {
        Console.Write("Enter filename to save (e.g., mygoals.txt): ");
        string fname = Console.ReadLine()?.Trim();
        if (string.IsNullOrEmpty(fname)) { Console.WriteLine("Invalid filename."); return; }

        if (manager.SaveToFile(fname))
            Console.WriteLine("Saved successfully.");
        else
            Console.WriteLine("Save failed.");
    }

    static void LoadMenu(GoalManager manager)
    {
        Console.Write("Enter filename to load (e.g., mygoals.txt): ");
        string fname = Console.ReadLine()?.Trim();
        if (string.IsNullOrEmpty(fname)) { Console.WriteLine("Invalid filename."); return; }

        if (manager.LoadFromFile(fname))
            Console.WriteLine("Loaded successfully.");
        else
            Console.WriteLine("Load failed.");
    }

    static void DeleteGoalMenu(GoalManager manager)
    {
        if (manager.Goals.Count == 0)
        {
            Console.WriteLine("No goals to delete.");
            return;
        }

        manager.ListGoals();
        int choice = PromptInt($"Select goal number to delete (1-{manager.Goals.Count}): ");
        if (choice < 1 || choice > manager.Goals.Count)
        {
            Console.WriteLine("Invalid selection.");
            return;
        }
        Console.Write($"Are you sure you want to delete '{manager.Goals[choice - 1].Title}'? (y/n): ");
        string ans = Console.ReadLine()?.Trim().ToLower();
        if (ans == "y" || ans == "yes")
        {
            manager.RemoveGoalAt(choice - 1);
            Console.WriteLine("Deleted.");
        }
        else
        {
            Console.WriteLine("Cancelled.");
        }
    }

    // Utility prompt for integers
    static int PromptInt(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string s = Console.ReadLine();
            if (int.TryParse(s, out int v) && v >= 0) return v;
            Console.WriteLine("Please enter a non-negative integer.");
        }
    }
}