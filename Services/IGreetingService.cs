using ModelContextProtocol.Server;
using System.ComponentModel;

namespace DemoApi.Services;

public interface IGreetingService
{
    string GetServiceGreeting();
}
