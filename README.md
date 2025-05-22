# .NET Minimal API with Model Context Protocol (MCP) Integration

This project demonstrates how to enable Model Context Protocol (MCP) support in a .NET 9 minimal API project.

## Key Features
- Minimal API endpoints (e.g., `/api/hello`, `/api/weatherforecast`)
- Swagger UI for API exploration
- MCP tools exposed for agent interoperability (see comments in `Program.cs`)

## How to Enable MCP in Your .NET Minimal API Project

1. **Install MCP NuGet Packages**
   - `ModelContextProtocol.AspNetCore`
   - `ModelContextProtocol.Server`

2. **Register MCP Services**
   In `Program.cs`, add:
   ```csharp
   builder.Services.AddMcpServer()
       .WithHttpTransport()
       .WithToolsFromAssembly();
   ```
   _See the MCP-related comments in `Program.cs` for details._

3. **Expose the MCP Endpoint**
   Add this before `app.Run();`:
   ```csharp
   app.MapMcp();
   ```
   By default, the negotiation endpoint is `/sse` for this SDK version.

4. **Define MCP Tools**
   Use the `[McpServerToolType]` and `[McpServerTool]` attributes to expose static methods as MCP tools. See the commented section in `Program.cs` for examples.

5. **Run and Test**
   - Start your API: `dotnet run --project DemoApi`
   - Use [MCP Inspector](https://www.npmjs.com/package/@modelcontextprotocol/inspector) to connect to `http://localhost:5068/sse` and test your MCP tools.

## Reference
- See the comments in `Program.cs` for the exact lines to enable and configure MCP support.
- For more, visit the [Model Context Protocol documentation](https://modelcontextprotocol.io/).
