using FluentAssertions;

namespace Wsa.GaaS.LittleFriendFighting.UnitTest;

public class Tests
{
    private readonly EndToEndTestServer _server = new();

    [Test]
    public async Task Success()
    {
        var responseMessage = await _server.Client.GetStringAsync("/");
        responseMessage.Should().Be("Hello World");
    }
}