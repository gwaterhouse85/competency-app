namespace CompetencyApp.Data;

public class CompetencyLevel
{
    public int Level { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<string> Characteristics { get; set; } = new();
}

public class SliderModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public int MinValue { get; set; }
    public int MaxValue { get; set; }
    public int DefaultValue { get; set; }
    public int Step { get; set; } = 1;
    public string Category { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<CompetencyLevel> Levels { get; set; } = new();
}

public class SliderConfiguration
{
    public List<SliderModel> Sliders { get; set; } = new();
}