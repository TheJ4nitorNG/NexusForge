namespace Company.Platform.Abstractions;

/// <summary>
/// Contains parameters for executing an AI request.
/// </summary>
/// <param name="SystemPrompt">The system prompt.</param>
/// <param name="UserPrompt">The user prompt.</param>
/// <param name="Parameters">Additional provider-specific parameters.</param>
public sealed record AiRequest(
    string SystemPrompt,
    string UserPrompt,
    IReadOnlyDictionary<string, object?> Parameters);

/// <summary>
/// Contains the response from an AI provider.
/// </summary>
/// <param name="Content">The text content of the response.</param>
/// <param name="PromptTokens">Number of tokens in the prompt.</param>
/// <param name="CompletionTokens">Number of tokens in the completion.</param>
/// <param name="Duration">The duration of the AI request.</param>
public sealed record AiResponse(
    string Content,
    int PromptTokens,
    int CompletionTokens,
    TimeSpan Duration);

/// <summary>
/// Represents a provider capable of fulfilling AI requests.
/// </summary>
public interface IAiProvider
{
    /// <summary>
    /// Completes an AI request asynchronously.
    /// </summary>
    /// <param name="request">The request parameters.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The AI response.</returns>
    Task<AiResponse> CompleteAsync(
        AiRequest request,
        CancellationToken cancellationToken);
}
