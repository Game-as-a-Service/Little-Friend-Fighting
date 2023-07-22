using Microsoft.AspNetCore.Mvc.Testing;

namespace Wsa.GaaS.LittleFriendFighting.UnitTest;

public class EndToEndTestServer : WebApplicationFactory<Program>
{
    public HttpClient Client { get; init; }

    public EndToEndTestServer()
    {
        Client = CreateClient();
    }
}