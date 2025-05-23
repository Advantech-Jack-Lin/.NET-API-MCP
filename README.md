# .NET Minimal API with Model Context Protocol (MCP) Integration

This project demonstrates how to enable Model Context Protocol (MCP) support in a .NET 9 minimal API project using two different approaches:

1. **Attribute-based discovery** – Expose static methods as MCP tools using attributes
2. **Service-based integration** – Expose dependency-injected services as MCP tools using attributes on the implementation

---

## Project Structure
- `Program.cs` – Main application configuration, API endpoints, and MCP setup (shows both integration methods)
- `ApiLogic.cs` – Shared business logic for API endpoints and MCP tools
- `Services/`
  - `IGreetingService.cs` – Service interface
  - `GreetingService.cs` – Service implementation, marked as an MCP tool type
- `DemoApi.csproj` – Project file with MCP and Swagger dependencies

---

## Key Features
- Minimal API endpoints: `/api/hello`, `/api/weatherforecast`, `/api/greeting`
- Swagger UI for API exploration
- **MCP tools exposed via two approaches:**
  - Attribute-based static methods (see `McpApiTools` in `Program.cs`)
  - Service-based (see `IGreetingService` and `GreetingService`)
- Shared logic between API endpoints and MCP tools for code reuse

---

## How to Enable MCP in Your .NET Minimal API Project

### 1. Install MCP NuGet Packages
- `ModelContextProtocol.AspNetCore`
- `ModelContextProtocol.Server`

### 2. Register MCP Services
You can use either or both approaches below:

#### Approach 1: Attribute-based discovery
```csharp
builder.Services.AddMcpServer()
    .WithHttpTransport()
    .WithToolsFromAssembly();
```
Define your MCP tools as static methods with attributes:
```csharp
[McpServerToolType]
public static class McpApiTools
{
    [McpServerTool, Description("Returns a simple 'Hello World' string")]
    public static string Hello() => ApiLogic.GetHelloWorld();
}
```

#### Approach 2: Service-based integration
Register your service and mark the implementation as an MCP tool type:
```csharp
// Service interface
public interface IGreetingService
{
    string GetServiceGreeting();
}

// Service implementation
using ModelContextProtocol.Server;
using System.ComponentModel;

[McpServerToolType]
public class GreetingService : IGreetingService
{
    [McpServerTool, Description("Returns a greeting from a service implementation")]
    public string GetServiceGreeting() => "Hello from Service";
}

// Register in Program.cs
builder.Services.AddScoped<IGreetingService, GreetingService>();
builder.Services.AddMcpServer()
    .WithHttpTransport()
    .WithToolsFromAssembly(); // This will discover both static and service-based tools
```

### 3. Expose the MCP Endpoint
Add this before `app.Run();`:
```csharp
app.MapMcp();
```
By default, the negotiation endpoint is `/sse` for this SDK version.

### 4. Organize Shared Logic
Use separate classes (like `ApiLogic.cs`) to share business logic between your API endpoints and MCP tools, ensuring consistent behavior.

### 5. Run and Test
- Start your API: `dotnet run --project DemoApi`
- Use [MCP Inspector](https://www.npmjs.com/package/@modelcontextprotocol/inspector) to connect to `http://localhost:5068/sse` and test your MCP tools.
- You should see both types of tools available:
  - Attribute-based tools: `hello`, `weatherforecast`
  - Service-based tool: `greeting`

---

## Reference
- See `Program.cs` for both MCP integration approaches
- See `ApiLogic.cs` for shared business logic
- See `Services/IGreetingService.cs` and `Services/GreetingService.cs` for service-based MCP tool definition
- For more, visit the [Model Context Protocol documentation](https://modelcontextprotocol.io/).
