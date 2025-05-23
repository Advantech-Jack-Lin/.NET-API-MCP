using ModelContextProtocol.Server;
using System.ComponentModel;

namespace DemoApi.Services;

[McpServerToolType]
public class GreetingService : IGreetingService
{
    [McpServerTool, Description("Returns a greeting from a service implementation.")]
    public string GetServiceGreeting() => "Hello from Service";
}
