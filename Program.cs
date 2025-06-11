// See https://aka.ms/new-console-template for more information

using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static List<Goal> goals = new List<Goal>();
    static int score = 0;

    static void Main(string[] args)
    {
        string input = "";
        while (input != "6")
        {
            Console.WriteLine($"\nYou have {score} points.\n");
            Console.WriteLine("Menu Options:");
            Console.WriteLine("  1. Create New Goal");
            Console.WriteLine("  2. List Goals");
            Console.WriteLine("  3. Save Goals");
            Console.WriteLine("  4. Load Goals");
            Console.WriteLine("  5. Record Event");
            Console.WriteLine("  6. Quit");
            Console.Write("Select a choice from the menu: ");
            input = Console.ReadLine()!;

            switch (input)
            {
                case "1":
                    CreateGoal();
                    break;
                case "2":
                    ListGoals();
                    break;
                case "3":
                    SaveGoals();
                    break;
                case "4":
                    LoadGoals();
                    break;
                case "5":
                    RecordEvent();
                    break;
                case "6":
                    Console.WriteLine("Goodbye!");
                    break;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }

    static void CreateGoal()
    {
        Console.WriteLine("\nThe types of Goals are:");
        Console.WriteLine("  1. Simple Goal");
        Console.WriteLine("  2. Eternal Goal");
        Console.WriteLine("  3. Checklist Goal");
        Console.Write("Which type of goal would you like to create? ");
        string choice = Console.ReadLine()!;

        Console.Write("Enter goal name: ");
        string name = Console.ReadLine()!;
        Console.Write("Enter description: ");
        string description = Console.ReadLine()!;
        Console.Write("Enter points: ");
        if (!int.TryParse(Console.ReadLine(), out int points))
        {
            Console.WriteLine("Invalid input for points.");
            return;
        }

        if (choice == "1")
        {
            goals.Add(new SimpleGoal(name, description, points));
        }
        else if (choice == "2")
        {
            goals.Add(new EternalGoal(name, description, points));
        }
        else if (choice == "3")
        {
            Console.Write("Enter number of times to complete: ");
            if (!int.TryParse(Console.ReadLine(), out int target))
            {
                Console.WriteLine("Invalid input for target count.");
                return;
            }
            Console.Write("Enter bonus points: ");
            if (!int.TryParse(Console.ReadLine(), out int bonus))
            {
                Console.WriteLine("Invalid input for bonus.");
                return;
            }

            goals.Add(new ChecklistGoal(name, description, points, target, bonus));
        }
        else
        {
            Console.WriteLine("Invalid goal type.");
        }
    }

    static void ListGoals()
    {
        Console.WriteLine("\nYour Goals:");
        if (goals.Count == 0)
        {
            Console.WriteLine("  No goals created yet.");
            return;
        }

        for (int i = 0; i < goals.Count; i++)
        {
            Console.WriteLine($"  {i + 1}. {goals[i].GetDetailsString()}");
        }
    }

    static void RecordEvent()
    {
        if (goals.Count == 0)
        {
            Console.WriteLine("No goals available to record.");
            return;
        }

        ListGoals();
        Console.Write("Which goal did you accomplish? ");
        if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <= goals.Count)
        {
            Goal selectedGoal = goals[index - 1];
            selectedGoal.RecordEvent();

            int earned = selectedGoal.GetPoints();

            if (selectedGoal is ChecklistGoal checklist && checklist.IsComplete())
            {
                earned += checklist.GetBonus();
            }

            score += earned;
            Console.WriteLine($"Total points earned: {earned}");
        }
        else
        {
            Console.WriteLine("Invalid selection.");
        }
    }

    static void SaveGoals()
    {
        try
        {
            using (StreamWriter output = new StreamWriter("goals.txt"))
            {
                output.WriteLine(score);
                foreach (Goal goal in goals)
                {
                    output.WriteLine(goal.GetStringRepresentation());
                }
            }
            Console.WriteLine("Goals saved to 'goals.txt'.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving goals: {ex.Message}");
        }
    }

    static void LoadGoals()
    {
        if (!File.Exists("goals.txt"))
        {
            Console.WriteLine("No saved file found.");
            return;
        }

        try
        {
            string[] lines = File.ReadAllLines("goals.txt");
            if (lines.Length == 0) return;

            if (!int.TryParse(lines[0], out score)) score = 0;
            goals.Clear();

            for (int i = 1; i < lines.Length; i++)
            {
                string[] parts = lines[i].Split(':');
                if (parts.Length != 2) continue;

                string type = parts[0];
                string[] data = parts[1].Split('|');

                switch (type)
                {
                    case "SimpleGoal":
                        var sg = new SimpleGoal(data[0], data[1], int.Parse(data[2]));
                        if (bool.TryParse(data[3], out bool done) && done) sg.RecordEvent();
                        goals.Add(sg);
                        break;

                    case "EternalGoal":
                        goals.Add(new EternalGoal(data[0], data[1], int.Parse(data[2])));
                        break;

                    case "ChecklistGoal":
                        var cg = new ChecklistGoal(data[0], data[1], int.Parse(data[2]),
                                                   int.Parse(data[4]), int.Parse(data[3]));
                        int completedCount = int.Parse(data[5]);
                        for (int j = 0; j < completedCount; j++) cg.RecordEvent();
                        goals.Add(cg);
                        break;
                }
            }

            Console.WriteLine("Goals loaded successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading goals: {ex.Message}");
        }
    }
}


