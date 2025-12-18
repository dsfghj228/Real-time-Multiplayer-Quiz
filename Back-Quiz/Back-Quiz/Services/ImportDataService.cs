using System.Text.Json;
using Back_Quiz.Data;
using Back_Quiz.Dtos;
using Back_Quiz.Enums;
using Back_Quiz.Interfaces;
using Back_Quiz.Models;
using Microsoft.EntityFrameworkCore;

namespace Back_Quiz.Services;

public class ImportDataService : IImportDataService
{
    private readonly IConfiguration _configuration;
    private readonly ApplicationDbContext _context;
    private readonly HttpClient _httpClient;
    
    public ImportDataService(ApplicationDbContext context, IConfiguration configuration, HttpClient httpClient)
    {
        _context = context;
        _configuration = configuration;
        _httpClient = httpClient;
    }
    
    public async Task ImportDataAsync()
    {
        var apiUrl = _configuration.GetValue<string>("ExternalApi:Url");
        HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Не удалось загрузить данные с внешнего API");
        }
        
        await using var stream = await response.Content.ReadAsStreamAsync();
        var apiResult = await JsonSerializer.DeserializeAsync<ApiResponse>(
            stream,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        if (apiResult?.Questions == null || apiResult.Questions.Count == 0)
        {
            throw new Exception("API вернул пустой список вопросов");
        }

        foreach (var question in apiResult.Questions)
        {
            bool exists = await _context.Questions
                .AnyAsync(q => q.Text == question.Question);

            if (exists)
                continue;
            
            List<QuestionOption> options = new List<QuestionOption>();

            for (int i = 0; i < question.Options.Count; i++)
            {
                options.Add(new QuestionOption
                {
                    Id = Guid.NewGuid(),
                    Text = question.Options[i],
                    IsCorrect = i == question.CorrectIndex
                });
            }

            var questionEntity = new Question
            {
                Id = Guid.NewGuid(),
                Text = question.Question,
                Category = question.Category,
                Difficulty = Enum.Parse<Difficulty>(question.Difficulty, ignoreCase: true),
                Options = options
            };
            
            await _context.Questions.AddAsync(questionEntity);
        }
        await _context.SaveChangesAsync();
    }
}