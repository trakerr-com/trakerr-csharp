# IO.Trakerr - the C# library for the Trakerr API

Get your application events and errors to Trakerr via the *Trakerr API*.

- API version: 1.0.0
- SDK version: 1.0.0

## Frameworks supported
- .NET 4.0 or later
- Windows Phone 7.1 (Mango)

## Dependencies
- [IO.TrakerrClient](http://www.nuget.org/packages/IO.TrakerrClient/) - 1.0.0 or later

The DLLs included in the package may not be the latest version. We recommend using [NuGet] (https://docs.nuget.org/consume/installing-nuget) to obtain the latest version of the packages:
```
Install-Package IO.TrakerrClient
```

## Getting Started

First setup a sample application and setup App.config to include your API key (see TrakerrSampleApp project for an example).

```xml
<configuration>
...
    <appSettings>
      <add key="trakerr.apiKey" value="<your api key here>" />
      <add key="trakerr.url" value="https://trakerr.io/api/v1/" />
      <add key="trakerr.contextAppVersion" value="1.0" />
      <add key="trakerr.contextEnvName" value="development"/>
    </appSettings>
</configuration>
```

### Option-1: Send an exception to Trakerr

To send an exception to Trakerr, it's as simple as calling .SendToTrakerr() on the exception (see example below).

```csharp
using IO.TrakerrClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrakerrSampleApp
{
    /// <summary>
    /// Sample program to generate an event
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                throw new Exception("This is a test exception.");
            }
            catch (Exception e)
            {
                // Send the event to Trakerr
                e.SendToTrakerr();
            }
        }
    }
}
```

### Option-2: Send any event (including non-exceptions) programmatically
```csharp
using IO.TrakerrClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrakerrSampleApp
{
    /// <summary>
    /// Sample program to generate an event
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            var client = new TrakerrClient();

            var event = client.GetNewAppEvent("Info", "System.Exception", "Some message");

            client.SendEventAsync(event);
        }
    }
}
```


<a name="documentation-for-models"></a>
## Documentation for Models

 - [Model.AppEvent](https://github.com/trakerr-io/trakerr-csharp/blob/master/generated/docs/AppEvent.md)

