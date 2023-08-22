using FluentAssertions;

namespace Wsa.GaaS.LittleFriendFighting.UnitTest;

public class Tests
{
    private EndToEndTestServer _server = null!; 

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        _server = new EndToEndTestServer();
    }    
    
    // [Test]
    // public async Task Success()
    // {
    //     var response = await _server.Client.GetStringAsync("/");
    //     response.Should().Be("Hello World");
    // }
}