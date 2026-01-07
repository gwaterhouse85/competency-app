using CompetencyApp.Data;
using System.Text.Json;

namespace CompetencyApp.Services;

public class SliderService
{
    private readonly IWebHostEnvironment _environment;
    private readonly ILogger<SliderService> _logger;

    public SliderService(IWebHostEnvironment environment, ILogger<SliderService> logger)
    {
        _environment = environment;
        _logger = logger;
    }

    public async Task<List<SliderModel>> GetSlidersAsync(string competencyType = "SE")
    {
        try
        {
            var fileName = GetCompetencyFileName(competencyType);
            var jsonPath = Path.Combine(_environment.WebRootPath, "data", fileName);
            
            if (!File.Exists(jsonPath))
            {
                _logger.LogWarning("Competency configuration file not found at {Path}", jsonPath);
                return GetDefaultSliders(competencyType);
            }

            var jsonContent = await File.ReadAllTextAsync(jsonPath);
            var configuration = JsonSerializer.Deserialize<SliderConfiguration>(jsonContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            var sliders = configuration?.Sliders ?? GetDefaultSliders(competencyType);
            
            // Ensure all sliders have Dreyfus model levels
            foreach (var slider in sliders)
            {
                if (!slider.Levels.Any())
                {
                    slider.Levels = GetDefaultLevelsForSlider(slider);
                    // Convert scale from 1-10 to 1-5 for Dreyfus model
                    if (slider.MaxValue == 10)
                    {
                        slider.MaxValue = 5;
                        slider.DefaultValue = Math.Min(3, slider.DefaultValue / 2 + 1);
                    }
                }
            }

            return sliders;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading slider configuration for {CompetencyType}", competencyType);
            return GetDefaultSliders(competencyType);
        }
    }

    public List<string> GetAvailableCompetencyTypes()
    {
        var competencyTypes = new List<string>();
        var dataPath = Path.Combine(_environment.WebRootPath, "data");
        
        if (Directory.Exists(dataPath))
        {
            var files = Directory.GetFiles(dataPath, "*Competency.json");
            foreach (var file in files)
            {
                var fileName = Path.GetFileNameWithoutExtension(file);
                if (fileName.EndsWith("Competency"))
                {
                    var competencyType = fileName.Substring(0, fileName.Length - "Competency".Length);
                    competencyTypes.Add(competencyType);
                }
            }
        }
        
        return competencyTypes.Any() ? competencyTypes : new List<string> { "SE" };
    }

    public string GetCompetencyDisplayName(string competencyType)
    {
        return competencyType switch
        {
            "SE" => "Software Engineer",
            "QA" => "QA Engineer",
            _ => competencyType
        };
    }

    private static string GetCompetencyFileName(string competencyType)
    {
        return $"{competencyType}Competency.json";
    }

    private static List<CompetencyLevel> GetDefaultLevelsForSlider(SliderModel slider)
    {
        return new List<CompetencyLevel>
        {
            new CompetencyLevel
            {
                Level = 1,
                Name = "Novice",
                Description = $"Basic understanding of {slider.Label.ToLower()}",
                Characteristics = new List<string> { "Learning fundamentals", "Requires guidance", "Limited experience" }
            },
            new CompetencyLevel
            {
                Level = 2,
                Name = "Advanced Beginner",
                Description = $"Can work with guidance on {slider.Label.ToLower()}",
                Characteristics = new List<string> { "Understands basics", "Can complete simple tasks", "Needs some support" }
            },
            new CompetencyLevel
            {
                Level = 3,
                Name = "Competent",
                Description = $"Can work independently with {slider.Label.ToLower()}",
                Characteristics = new List<string> { "Works independently", "Solves problems systematically", "Plans and executes tasks" }
            },
            new CompetencyLevel
            {
                Level = 4,
                Name = "Proficient",
                Description = $"Deep understanding and intuitive use of {slider.Label.ToLower()}",
                Characteristics = new List<string> { "Intuitive problem solving", "Sees bigger picture", "Mentors others" }
            },
            new CompetencyLevel
            {
                Level = 5,
                Name = "Expert",
                Description = $"Expert-level mastery of {slider.Label.ToLower()}",
                Characteristics = new List<string> { "Innovates and improves", "Sets standards", "Leads and teaches" }
            }
        };
    }

    private static List<SliderModel> GetDefaultSliders(string competencyType)
    {
        return new List<SliderModel>
        {
            new SliderModel
            {
                Id = 1,
                Name = "frontend_frameworks",
                Label = "Frontend Frameworks",
                MinValue = 1,
                MaxValue = 5,
                DefaultValue = 3,
                Category = "Frontend Development",
                Description = "React, Vue, Angular, Svelte proficiency",
                Levels = GetDefaultLevelsForSlider(new SliderModel { Label = "Frontend Frameworks" })
            }
        };
    }
}