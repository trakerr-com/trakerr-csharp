# IO.Trakerr - the C# library for the Trakerr API

Get your application events and errors to Trakerr via the *Trakerr API*.

- API version: 1.0.0
- SDK version: 1.0.0

## Frameworks supported
- .NET 4.0 or later
- Windows Phone 7.1 (Mango) and later.

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

### Option-2: Send an exception with custom properties to Trakerr
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

            try {
                ...
            } catch(Exception e) {
                var exceptionEvent = client.CreateAppEventFromException("Error", e);

                exceptionEvent.CustomProperties = new IO.Trakerr.Model.CustomData();
                exceptionEvent.CustomProperties.StringData = new IO.Trakerr.Model.CustomStringData();
                exceptionEvent.CustomProperties.StringData.CustomData1 = "Some custom data";

                client.SendEventAsync(exceptionEvent);
            }

        }
    }
}
```



### Option-3: Send any event (including non-exceptions) programmatically
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

            var event = client.CreateAppEvent("Info", "System.Exception", "Some message");

            client.SendEventAsync(event);
        }
    }
}
```

##About the TrakerrClient Constructor

The `TrakerrClient` class above can be constructed to take aditional data, rather than using the configured defaults. The constructor signature is:

```csharp
public TrakerrClient(string apiKey = null, string url = null, string contextAppVersion = null,
string contextEnvName = "development", string contextEnvVersion = null,
string contextEnvHostname = null, string contextAppOS = null,
string contextAppOSVersion = null, string contextDataCenter = null,
string contextDataCenterRegion = null)
```

Nearly all of these have default values when passed in null. Below is a list of the arguments, and what Trakerr expects so you can pass in custom data.

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**api_key** | **str** | API Key for your application. | Defaults to reading "trakerr.apiKey" property under appSettings from the App.config.
**url_path** | **str** | URL to Trakerr. | Defaults to reading "trakerr.url" property under appSettings from the App.config.
**context_app_version** | **str** |  Provide the application version. | Defaults to reading "trakerr.contextAppVersion" property under appSettings from the App.config.
**context_env_name** | **str** | Provide the environemnt name (development/staging/production). You can also pass in a custom name. | Defaults to reading "trakerr.contextEnvName" property under appSettings from the App.config.
**context_env_version** | **str** | (Optional) Provide an optional context environment version. | Defaults to `null`. 
**context_env_hostname** | **str** | Provide the current hostname. | Defaults to the current DNS name if available or uses the Machine name as a fallback.
**context_app_os** | **str** | Provide an operating system name. | Defaults to Environment.OSVersion.Platform along with the service pack (eg. Win32NT Service Pack 1).
**context_app_os_version** | **str** | Provide an operating system version. | Defaults to Environment.OSVersion.Version.ToString() (eg. 6.1.7601.65536).
**context_data_center** | **str** | (optional) Provide a datacenter name. | Defaults to `null`.
**context_data_center_region** | **str** | (optional) Provide a datacenter region. | Defaults to `null`.

If you want to use a default value in a custom call, simply pass in `null` to the argument, and it will be filled with the default value.

<a name="documentation-for-models"></a>
## Documentation for Models

 - [Model.AppEvent](https://github.com/trakerr-io/trakerr-csharp/blob/master/generated/docs/AppEvent.md)

