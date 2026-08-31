using System.IO.Pipes;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text.Json;

namespace Company.Platform.IPC;

/// <summary>
/// Represents a strongly-typed named pipe server.
/// </summary>
/// <param name="pipeName">The name of the pipe.</param>
public sealed class NamedPipeServer(string pipeName)
{
    private readonly string _pipeName = pipeName;

    /// <summary>
    /// Starts the server and listens for a single request asynchronously.
    /// </summary>
    /// <param name="requestHandler">The handler for incoming requests.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task StartListeningAsync(
        Func<IpcRequest, CancellationToken, Task<IpcResponse>> requestHandler,
        CancellationToken cancellationToken)
    {
#pragma warning disable CA1416 // Validate platform compatibility
        PipeSecurity pipeSecurity = new();
        SecurityIdentifier everyone = new(WellKnownSidType.WorldSid, null);
        pipeSecurity.AddAccessRule(new PipeAccessRule(everyone, PipeAccessRights.ReadWrite, AccessControlType.Allow));

        using NamedPipeServerStream pipeServer = NamedPipeServerStreamAcl.Create(
            _pipeName,
            PipeDirection.InOut,
            NamedPipeServerStream.MaxAllowedServerInstances,
            PipeTransmissionMode.Byte,
            PipeOptions.Asynchronous,
            4096,
            4096,
            pipeSecurity);
#pragma warning restore CA1416

        await pipeServer.WaitForConnectionAsync(cancellationToken);

        using StreamReader reader = new(pipeServer, leaveOpen: true);
        using StreamWriter writer = new(pipeServer, leaveOpen: true) { AutoFlush = true };

        string? requestJson = await reader.ReadLineAsync(cancellationToken);
        if (requestJson == null)
        {
            return;
        }

        IpcRequest? request = JsonSerializer.Deserialize<IpcRequest>(requestJson);
        if (request == null)
        {
            return;
        }

        IpcResponse response = await requestHandler(request, cancellationToken);

        string responseJson = JsonSerializer.Serialize(response);
        await writer.WriteLineAsync(responseJson);
    }
}
