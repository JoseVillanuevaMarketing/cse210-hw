using System;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello World! This is the Journal Project.");
        Console.WriteLine("---------------------------------------------------");
        Console.WriteLine("Financial Journal - Budget Reflection");
        Console.WriteLine("---------------------------------------------------\n");

        // 1. Challenges
        Console.WriteLine("1. What challenges did you have tracking your income/expenses this week?");
        string challenges = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(challenges))
        {
            challenges = "No challenges this week. Tracking was manageable.";
        }

        // 2. SMART Goal
        Console.WriteLine("\n2. Please write one or more SMART financial goals (with measurable value):");
        string smartGoal = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(smartGoal))
        {
            smartGoal = "Save $100 this month for an emergency fund.";
        }

        // 3. Budget Cell Representation
        Console.WriteLine("\n3. Where is/are your SMART financial goal(s) represented in your budget? (Example: B40)");
        string budgetCell = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(budgetCell))
        {
            budgetCell = "B40";
        }

        // 4. Envelope Method
        Console.WriteLine("\n4. With which of your variable expenses (Groceries, etc.) would you like to practice using the envelope method?");
        string envelopeExpense = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(envelopeExpense))
        {
            envelopeExpense = "Groceries";
        }

        // Final summary
        Console.WriteLine("\n---------------------------------------------------");
        Console.WriteLine("Your Financial Journal Entry:");
        Console.WriteLine($"Challenges: {challenges}");
        Console.WriteLine($"SMART Goal: {smartGoal}");
        Console.WriteLine($"Budget Cell: {budgetCell}");
        Console.WriteLine($"Envelope Method Expense: {envelopeExpense}");
        Console.WriteLine("---------------------------------------------------");
        Console.WriteLine("Journal entry completed. Thank you!");
    }
}