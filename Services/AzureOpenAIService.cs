using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using CompetencyApp.Data;

namespace CompetencyApp.Services;

public class AzureOpenAISettings
{
    public bool Enabled { get; set; } = false;
    public string Endpoint { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
    public string DeploymentName { get; set; } = string.Empty;
}

public class CompetencySuggestion
{
    public int SliderId { get; set; }
    public string CompetencyName { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int CurrentScore { get; set; }
    public string LevelName { get; set; } = string.Empty;
    public string UserNotes { get; set; } = string.Empty;
    public string AISuggestion { get; set; } = string.Empty;
}

public class AzureOpenAIService
{
    private readonly HttpClient _httpClient;
    private readonly AzureOpenAISettings _settings;
    private readonly ILogger<AzureOpenAIService> _logger;

    public AzureOpenAIService(HttpClient httpClient, AzureOpenAISettings settings, ILogger<AzureOpenAIService> logger)
    {
        _httpClient = httpClient;
        _settings = settings;
        _logger = logger;
    }

    public async Task<List<CompetencySuggestion>> GetCompetencySuggestionsAsync(
        List<SliderModel> sliders,
        Dictionary<int, int> sliderValues,
        Dictionary<int, string> sliderNotes)
    {
        var suggestions = new List<CompetencySuggestion>();

        foreach (var slider in sliders)
        {
            var currentScore = sliderValues.GetValueOrDefault(slider.Id, slider.DefaultValue);
            var currentLevel = slider.Levels.FirstOrDefault(l => l.Level == currentScore);
            var userNote = sliderNotes.GetValueOrDefault(slider.Id, string.Empty);

            var suggestion = new CompetencySuggestion
            {
                SliderId = slider.Id,
                CompetencyName = slider.Label,
                Category = slider.Category,
                CurrentScore = currentScore,
                LevelName = currentLevel?.Name ?? "Unknown",
                UserNotes = userNote
            };

            try
            {
                suggestion.AISuggestion = await GenerateSuggestionForCompetencyAsync(slider, currentScore, currentLevel, userNote);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating suggestion for {CompetencyName}", slider.Label);
                suggestion.AISuggestion = "Unable to generate suggestion at this time.";
            }

            suggestions.Add(suggestion);
        }

        return suggestions;
    }

    private async Task<string> GenerateSuggestionForCompetencyAsync(
        SliderModel slider,
        int currentScore,
        CompetencyLevel? currentLevel,
        string userNote)
    {
        var nextLevel = slider.Levels.FirstOrDefault(l => l.Level == currentScore + 1);
        
        var prompt = BuildPrompt(slider, currentScore, currentLevel, nextLevel, userNote);
        
        var requestBody = new
        {
            messages = new[]
            {
                new
                {
                    role = "system",
                    content = "You are a helpful career development coach specializing in software engineering and QA competencies. Provide concise, actionable suggestions to help professionals improve their skills. Keep responses brief (2-3 sentences max) and practical."
                },
                new
                {
                    role = "user",
                    content = prompt
                }
            },
            max_tokens = 150,
            temperature = 0.7
        };

        var apiUrl = $"{_settings.Endpoint.TrimEnd('/')}/openai/deployments/{_settings.DeploymentName}/chat/completions?api-version=2024-02-15-preview";

        var request = new HttpRequestMessage(HttpMethod.Post, apiUrl);
        request.Headers.Add("api-key", _settings.ApiKey);
        request.Content = new StringContent(
            JsonSerializer.Serialize(requestBody),
            Encoding.UTF8,
            "application/json");

        var response = await _httpClient.SendAsync(request);
        
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            _logger.LogError("Azure OpenAI API error: {StatusCode} - {Content}", response.StatusCode, errorContent);
            throw new HttpRequestException($"API request failed: {response.StatusCode}");
        }

        var responseContent = await response.Content.ReadAsStringAsync();
        var jsonResponse = JsonDocument.Parse(responseContent);
        
        var suggestion = jsonResponse.RootElement
            .GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString();

        return suggestion?.Trim() ?? "No suggestion available.";
    }

    private string BuildPrompt(
        SliderModel slider,
        int currentScore,
        CompetencyLevel? currentLevel,
        CompetencyLevel? nextLevel,
        string userNote)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"Competency: {slider.Label}");
        sb.AppendLine($"Description: {slider.Description}");
        sb.AppendLine($"Current Level: {currentScore}/5 ({currentLevel?.Name ?? "Unknown"})");
        
        if (currentLevel != null)
        {
            sb.AppendLine($"Current Level Description: {currentLevel.Description}");
            if (currentLevel.Characteristics.Any())
            {
                sb.AppendLine($"Current Characteristics: {string.Join(", ", currentLevel.Characteristics)}");
            }
        }

        if (nextLevel != null)
        {
            sb.AppendLine($"Next Level: {nextLevel.Name}");
            sb.AppendLine($"Next Level Description: {nextLevel.Description}");
            if (nextLevel.Characteristics.Any())
            {
                sb.AppendLine($"Target Characteristics: {string.Join(", ", nextLevel.Characteristics)}");
            }
        }
        else if (currentScore == 5)
        {
            sb.AppendLine("The user is already at Expert level.");
        }

        if (!string.IsNullOrWhiteSpace(userNote))
        {
            sb.AppendLine($"User's Notes: {userNote}");
        }

        if (currentScore == 5)
        {
            sb.AppendLine("\nProvide suggestions on how to maintain expert-level skills and contribute to the community (mentoring, thought leadership, etc.).");
        }
        else
        {
            sb.AppendLine($"\nProvide specific, actionable suggestions to help improve from level {currentScore} to level {currentScore + 1}.");
        }

        return sb.ToString();
    }
}
