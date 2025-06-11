using System;

public class SimpleGoal : Goal
{
    private bool _isComplete;

    public SimpleGoal(string name, string description, int points)
        : base(name, description, points)
    {
        _isComplete = false;
    }

    public override void RecordEvent()
    {
        if (!_isComplete)
        {
            _isComplete = true;
            Console.WriteLine($"You earned {_points} points!");
        }
        else
        {
            Console.WriteLine("This goal is already completed.");
        }
    }

    public override bool IsComplete() => _isComplete;

    public override string GetDetailsString()
    {
        string checkbox = _isComplete ? "[X]" : "[ ]";
        return $"{checkbox} {_name} ({_description})";
    }

    public override string GetStringRepresentation()
    {
        return $"SimpleGoal:{_name}|{_description}|{_points}|{_isComplete}";
    }
}
