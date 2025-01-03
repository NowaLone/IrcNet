# IrcNet

IrcNet is a comprehensive library for building IRC (Internet Relay Chat) clients. It includes abstractions, parsers, and extensions for both RFC 1459 and IRCv3 specifications.

## Projects

### IrcNet.Abstractions
This project contains abstractions for IRC functionality, providing interfaces and base classes for building IRC clients.

### IrcNet.Parser.Rfc1459
A library for parsing IRC messages according to the RFC 1459 specifications.

### IrcNet.Parser.V3
A library for parsing IRC messages according to the IRCv3 specifications.

### IrcNet.Client
A core library for building IRC clients.

### IrcNet.Client.Extensions.Core
This package contains core extensions for the IrcNet.Client library.

### IrcNet.Client.Rfc1459.Extensions
This package contains extensions for the IrcNet.Client library, implementing RFC 1459.

### IrcNet.Client.V3.Extensions
This package contains extensions for the IrcNet.Client library, implementing IRCv3 specifications.

## Tests

### IrcNet.Parser.Rfc1459.Tests
Unit tests for the IrcNet.Parser.Rfc1459 library.

### IrcNet.Parser.V3.Tests
Unit tests for the IrcNet.Parser.V3 library.

## Getting Started

### Prerequisites
- .NET Framework 4.6.1 or higher
- .NET Core 3.0 or higher
- .NET 6 or higher
- .NET Standard 2.0 or higher

### Installation
Clone the repository:

```shell
git clone https://github.com/NowaLone/IrcNet.git
```

### Building the Solution
Navigate to the solution directory and build the projects using the .NET CLI:

```shell
dotnet build
```

### Running Tests
To run the tests, use the .NET CLI:

```shell
dotnet test
```

## Usage Example

Here's a simple example of how to use the IrcNet library to connect to an IRC server and join a channel:

```csharp
using IrcNet.Client;
using System;
using System.Text;
using System.Threading.Tasks;
using static IrcNet.Client.IrcClientWebSocket;

internal class Program
{
	private static async Task Main(string[] args)
	{
		var options = new Options() { Uri = new Uri("irc.example.com:6667"), PingDelay = TimeSpan.FromSeconds(1) };
		var client = new IrcClientWebSocket(options);

		await client.OpenAsync();

		client.OnMessageReceived += (sender, message) =>
		{
			Console.WriteLine($"Received: {message}");
		};

		await client.SendAsync(Encoding.UTF8.GetBytes($"PRIVMSG test"));

		await Task.Delay(3000);
		await client.CloseAsync();
	}
}
```

## Acknowledgements
- [RFC 1459](https://tools.ietf.org/html/rfc1459)
- [RFC 2812](https://tools.ietf.org/html/rfc2812)
- [IRCv3 Working Group](https://ircv3.net/)
