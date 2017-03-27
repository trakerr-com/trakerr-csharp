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

### Option-2: Send an exception using the TrakerrClient
The benifit of using the TrakerrClient to automatically create and send your error, as opposed to above, is that you can change the log level. You will, however, have to also add an import.

```csharp
using IO.TrakerrClient;
using IO.Trakerr.Model;
using System;

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

            try
            {
                throw new ArgumentException("Args are invalid.");
            }
            catch (Exception e)
            {
                tc.SendException(e);//Can also change the log level, along with the classifcation, unlike the above which only changes issue.
            }
        }
    }
}
```

### Option-3: Send an exception using the TrakerrClient
You can get an appevent to change and store custom data. You must then call to manually send it afterwards.

```csharp
using IO.TrakerrClient;
using IO.Trakerr.Model;
using System;

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

            try
            {
                throw new IndexOutOfRangeException("Buffer overflow.");
            }
            catch (Exception e)
            {
                var appevent = tc.CreateAppEvent(e, AppEvent.LogLevelEnum.Fatal);//Can also change the classification.
                //EventType and EventMessage are set automatically by create app event; you can set them manually from the appevent instance too.
                appevent.EventUser = "john@trakerr.io";
                appevent.EventSession = "8";

                appevent.CustomProperties = new CustomData();
                appevent.CustomProperties.StringData = new CustomStringData("This is string data 1!");//Add up to 10 custom strings.
                appevent.CustomProperties.StringData.CustomData2 = "This is string data 2!";//You can also add strings later like this.

                tc.SendEventAsync(appevent);
            }
        }
    }
}
```



### Option-3: Send any event (including non-exceptions) programmatically
You can send non-errors to Trakerr. This will send an event without a stacktrace. Be sure to construct the object properly as the default values in `CreateAppEvent` may not be useful for your non-error.

```csharp
using IO.TrakerrClient;
using IO.Trakerr.Model;
using System;

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

            var infoevent = tc.CreateAppEvent(AppEvent.LogLevelEnum.Info, "User sends clicked this button", "Feature Analytics", "Some Feature");
            infoevent.EventUser = "jill@trakerr.io";
            infoevent.EventSession = "2";

            //Populate any other data you want, customdata or overriding default values of the appevent.

            tc.SendEventAsync(infoevent);
        }
    }
}
```

## About the TrakerrClient Constructor

The `TrakerrClient` class above can be constructed to take aditional data, rather than using the configured defaults. The constructor signature is:

```csharp
public TrakerrClient(string apiKey = null, string contextAppVersion = null, string contextDeploymentStage = null, string contextEnvLanguage = "C#")
```

Nearly all of these have default values when passed in `null`. Below is a list of the arguments, and what Trakerr expects so you can pass in custom data.

Name | Type | Description | Notes
------------ | ------------- | -------------  | -------------
**apiKey** | **string**  | API Key for your application. | Defaults to reading "trakerr.apiKey" property under appSettings from the App.config.
**contextAppVersion** | **string** | Provide the application version. | Defaults to reading "trakerr.contextAppVersion" property under appSettings from the App.config.
**contextDevelopmentStage** | **string** | One of development, staging, production; or a custom string. | Default Value: trakerr.deploymentStage or "development" if not provided.
**contextEnvLanguage** | **string** | Constant string representing the language the application is in. | Default value: "C#", can be set by the constructor if your using VB or managed C++.
**contextEnvName** | **string** | Name of the CLR the program is running on | Defaults to returning "Microsoft CLR" if using .Net framework or "Mono" if mono. 
**contextEnvVersion** | **string** | Provide an environment version. | Defaults to reading to the CLR version of .net, or uses reflection to find the mono version.
**contextEnvHostname** | **string** | Provide the current hostname. | Defaults to the current DNS name if available or uses the Machine name as a fallback.
**contextAppOS** | **string** | Provide an operating system name. | Defaults to Environment.OSVersion.Platform along with the service pack (eg. Win32NT Service Pack 1).
**contextAppOSVersion** | **string** | Provide an operating system version. | Defaults to Environment.OSVersion.Version.ToString() (eg. 6.1.7601.65536).
**contextAppOSBrowser** | **string** | An optional string browser name the application is running on. | Defaults to `null`
**contextAppOSBrowserVersion** | **string** | An optional string browser version the application is running on. | Defaults to `null`
**contextDataCenter** | **string** | Data center the application is running on or connected to. | Defaults to `null`
**contextDataCenterRegion** | **string** | Data center region. | Defaults to `null`

If you want to use a default value in a custom call, simply pass in `null` to the argument, and it will be filled with the default value.

<a name="documentation-for-models"></a>
## Documentation for Models

 - [Model.AppEvent](https://github.com/trakerr-io/trakerr-csharp/blob/master/generated/docs/AppEvent.md)

