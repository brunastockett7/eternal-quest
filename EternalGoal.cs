public class EternalGoal : Goal
{
    public EternalGoal(string name, string description, int points)
        : base(name, description, points)
    {
    }

    public override void RecordEvent()
    {
        Console.WriteLine($"Nice! You earned {_points} points.");
    }

    public override bool IsComplete()
    {
        return false; // Eternal goals never complete
    }

    public override string GetDetailsString()
    {
        return $"[∞] {_name} ({_description})";
    }

    public override string GetStringRepresentation()
    {
        return $"EternalGoal:{_name}|{_description}|{_points}";
    }
}
