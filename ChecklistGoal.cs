using System;

public class ChecklistGoal : Goal
{
    private int _targetCount;
    private int _completedCount;
    private int _bonus;

    public ChecklistGoal(string name, string description, int points, int targetCount, int bonus)
        : base(name, description, points)
    {
        _targetCount = targetCount;
        _completedCount = 0;
        _bonus = bonus;
    }

    public override void RecordEvent()
    {
        _completedCount++;

        if (_completedCount == _targetCount)
        {
            Console.WriteLine($"You earned {_points} + {_bonus} bonus points!");
        }
        else
        {
            Console.WriteLine($"You earned {_points} points.");
        }
    }

    public override bool IsComplete()
    {
        return _completedCount >= _targetCount;
    }

    public override string GetDetailsString()
    {
        string checkbox = IsComplete() ? "[X]" : "[ ]";
        return $"{checkbox} {_name} ({_description}) -- Completed {_completedCount}/{_targetCount}";
    }

    public int GetBonus()
    {
        return _bonus;
    }

    public override string GetStringRepresentation()
    {
        return $"ChecklistGoal:{_name}|{_description}|{_points}|{_targetCount}|{_bonus}|{_completedCount}";
    }
}

