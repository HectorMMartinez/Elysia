using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Elysia.Core.Application.Interfaces;
using Elysia.Infraestructure.CenterIA.Configuration;
using Elysia.Infraestructure.CenterIA.Models;
using Microsoft.Extensions.Options;

namespace Elysia.Infrastructure.AI.Providers;

public class OpenAIProvider : IOpenAIProvider
{
    private readonly HttpClient _httpClient;
    private readonly OpenAISettings _settings;
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower // importante para Responses API
    };

    public OpenAIProvider(HttpClient httpClient, IOptions<OpenAISettings> options)
    {
        _httpClient = httpClient;
        _settings = options.Value;

        // Configuración base del HttpClient (solo una vez)
        if (_httpClient.BaseAddress is null)
        {
            _httpClient.BaseAddress = new Uri(_settings.BaseUrl);
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _settings.ApiKey);
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }

    public async Task<string> GenerarRespuestaAsync(string prompt, string? instructions = null, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(prompt))
            throw new ArgumentException("El prompt no puede estar vacío.", nameof(prompt));

        var request = new OpenAIRequest
        {
            Model = _settings.Model,
            Input = prompt,
            Instructions = instructions
            // Temperature se puede agregar después si lo necesitas
        };

        using var content = new StringContent(
            JsonSerializer.Serialize(request, JsonOptions),
            Encoding.UTF8,
            "application/json");

        using var response = await _httpClient.PostAsync("responses", content, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var errorBody = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new HttpRequestException(
                $"Error de OpenAI ({(int)response.StatusCode}): {errorBody}");
        }

        var openAiResponse = await response.Content.ReadFromJsonAsync<OpenAIResponse>(JsonOptions, cancellationToken);

        if (openAiResponse is null || string.IsNullOrWhiteSpace(openAiResponse.OutputText))
            throw new InvalidOperationException("La respuesta de OpenAI no contiene texto válido.");

        return openAiResponse.OutputText;
    }
}
