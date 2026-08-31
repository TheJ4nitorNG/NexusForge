using System.IO.Pipes;
using System.Text.Json;

namespace Company.Platform.IPC;

/// <summary>
/// Represents a strongly-typed named pipe client.
/// </summary>
/// <param name="pipeName">The name of the pipe.</param>
public sealed class NamedPipeClient(string pipeName)
{
    private readonly string _pipeName = pipeName;

    /// <summary>
    /// Sends a request to the server and awaits a response.
    /// </summary>
    /// <param name="request">The request to send.</param>
    /// <param name="timeout">The connection timeout.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response from the server.</returns>
    public async Task<IpcResponse> SendRequestAsync(
        IpcRequest request,
        TimeSpan timeout,
        CancellationToken cancellationToken)
    {
        using NamedPipeClientStream pipeClient = new(
            ".",
            _pipeName,
            PipeDirection.InOut,
            PipeOptions.Asynchronous);

        await pipeClient.ConnectAsync((int)timeout.TotalMilliseconds, cancellationToken);

        using StreamReader reader = new(pipeClient, leaveOpen: true);
        using StreamWriter writer = new(pipeClient, leaveOpen: true) { AutoFlush = true };

        string requestJson = JsonSerializer.Serialize(request);
        await writer.WriteLineAsync(requestJson.AsMemory(), cancellationToken);

        string responseJson = await reader.ReadLineAsync(cancellationToken) ?? throw new InvalidOperationException("Failed to read response from server.");

        IpcResponse? response = JsonSerializer.Deserialize<IpcResponse>(responseJson);
        return response ?? throw new InvalidOperationException("Failed to deserialize response.");
    }
}
