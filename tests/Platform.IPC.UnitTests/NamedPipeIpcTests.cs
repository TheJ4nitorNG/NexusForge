namespace Company.Platform.IPC.UnitTests;

using Company.Platform.IPC;
using FluentAssertions;

public class NamedPipeIpcTests
{
    [Fact]
    public async Task ServerAndClient_ShouldCommunicateSuccessfully()
    {
        // Arrange
        string pipeName = $"test_pipe_{Guid.NewGuid():N}";
        var server = new NamedPipeServer(pipeName);
        var client = new NamedPipeClient(pipeName);
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));

        var request = new IpcRequest("req-123", "TestOp", "TestPayload");

        var serverTask = server.StartListeningAsync((req, token) =>
        {
            req.RequestId.Should().Be("req-123");
            req.Operation.Should().Be("TestOp");
            req.Payload.Should().Be("TestPayload");

            return Task.FromResult(new IpcResponse(req.RequestId, true, "SuccessPayload"));
        }, cts.Token);

        // Act
        var clientTask = client.SendRequestAsync(request, TimeSpan.FromSeconds(5), cts.Token);

        await Task.WhenAll(serverTask, clientTask);

        var response = await clientTask;

        // Assert
        response.Should().NotBeNull();
        response.RequestId.Should().Be("req-123");
        response.IsSuccess.Should().BeTrue();
        response.Payload.Should().Be("SuccessPayload");
    }
}
