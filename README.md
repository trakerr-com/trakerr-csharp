# Trakerr - C# API client

Get your application events and errors to Trakerr via the *Trakerr API*.

You can send both **errors and non-errors** (plain log statements, for example) to Trakerr with this API.

## Overview

The **3-minute guide** is primarily oriented around sending **errors or warnings** and also do not allow sending additional
parameters. **Option-3 in the detailed integration guide** describes how you could send a non-error (or any log statement) along with additional parameters.

The SDK takes performance impact seriously and all communication between the SDK <=> Trakerr avoids blocking the calling function. The SDK also applies asynchronous patterns where applicable.

A Trakerr *Event* is what is created by this SDK and is then sent to Trakerr for Trakerr to capture. A Trakerr *Event* can consist of various parameters as described here in [Model.AppEvent](https://github.com/trakerr-io/trakerr-csharp/blob/master/generated/docs/AppEvent.md).
Some of these parameters are populated by default and others are optional and can be supplied by you.

Since some of these parameters are common across all event's, the API has the option of setting these on the
TrakerrClient instance (described towards the bottom) and offers a factory API for creating AppEvent's.

### Frameworks supported
- .NET 4.0 or later
- Windows Phone 7.1 (Mango) and later.

## Install SDK dependency using Nuget
- [IO.TrakerrClient](http://www.nuget.org/packages/IO.TrakerrClient/) - 1.0.0 or later

The DLLs included in the package may not be the latest version. We recommend using [NuGet] (https://docs.nuget.org/consume/installing-nuget) to obtain the latest version of the packages:
```
Install-Package IO.TrakerrClient
```

## 3-minute Integration Guide

### Using App.config for configuration
First setup a sample application and setup App.config to include your API key (see TrakerrSampleApp project for an example).

```xml
<configuration>
...
    <appSettings>
      <add key="trakerr.apiKey" value="<api-key>" />
      <add key="trakerr.url" value="https://trakerr.io/api/v1/" />
      <add key="trakerr.contextAppVersion" value="1.0" />
      <add key="trakerr.deploymentStage" value="development"/>
    </appSettings>
</configuration>
```

To send an exception to Trakerr, it's as simple as calling .SendToTrakerr() on the exception (see example below).

```csharp
    using IO.TrakerrClient;
    ...
    try
    {
        throw new Exception("This is a test exception.");
    }
    catch (Exception e)
    {
        // Send the event to Trakerr
        e.SendToTrakerr();
    }
```

### Without App.config for configuration

```csharp
    using IO.TrakerrClient;
    using IO.Trakerr.Model;
    ...

    // specify config (without App.config)
    var trakerrClient = TrakerrClient(
        "<api-key>",                // API KEY
        "1.0",                      // Your application version (any string)
        "production"                // Any custom deployment stage string ("production", "development", "staging" etc.)
    );

    try
    {
        throw new ArgumentException("Args are invalid.");
    }
    catch (Exception e)
    {
        // You can optionally specify the log level, along with the classifcation in the SendException parameters.
        // See C# documentation of SendException for more details.
        trakerrClient.SendException(e);
    }

```

## Trakerr Client Shutdown
When cleaning up your application, or implementing a shutdown, be sure to call

```csharp
TrakerrClient.Shutdown(false);
```

This will close the CPU monitoring thread gracefully. If an error has occurred; passing `true` will force thread abort. Do not pass true unless absolutely necessary.

## Detailed Integration Guide
### Option-1: Send an exception to Trakerr
Use the guide [3-minute integration guide above](#3-minute-Integration-Guide) to send an exception to Trakerr.


### Option-2: Send an exception with user, session, custom properties and more

Calling `CreateAppEvent` with an exception as shown below automatically populates the stacktrace information to send to Trakerr.

This call will create a new  [Model.AppEvent](https://github.com/trakerr-io/trakerr-csharp/blob/master/generated/docs/AppEvent.md).
You can then populate other member data to send to Trakerr using accessors (see [Model.AppEvent](https://github.com/trakerr-io/trakerr-csharp/blob/master/generated/docs/AppEvent.md)
 for more details on properties that you can set).


```csharp
    using IO.TrakerrClient;
    using IO.Trakerr.Model;
    ....

    // Instantiate the TrakerrClient globally somewhere and store. This is thread-safe.
    // Specify config without App.config (OR if using App.config you can use the parameterless constructor instead)

    var trakerrClient = TrakerrClient(
        "<api-key>",                // API KEY
        "1.0",                      // Your application version (any string)
        "production"                // Any custom deployment stage string ("production", "development", "staging" etc.)
    );

    ....

    try
    {
        throw new IndexOutOfRangeException("Buffer overflow.");
    }
    catch (Exception e)
    {

        var appevent = trakerrClient.CreateAppEvent(
            e,                                  // the exception to send to Trakerr
            AppEvent.LogLevelEnum.Fatal,        // the log level indicating FATAL
            "Issue"                             // the classification indicating an "Issue"
        );

        // You can set properties on AppEvent below like user, session, custom properties and more.
        appevent.EventUser = "john@trakerr.io";
        appevent.EventSession = "8";
        appevent.appevent.ContextOperationTimeMillis = 1000;

        // Set some custom data
        appevent.CustomProperties = new CustomData();
        appevent.CustomProperties.StringData = new CustomStringData("This is string data 1!");//Add up to 10 custom strings.
        appevent.CustomProperties.StringData.CustomData2 = "This is string data 2!";//You can also add strings later like this.

        // opulate any other data you want, customdata or overriding default values of the appevent.
        trakerrClient.SendEventAsync(appevent);
    }
```


### Option-3: Send an event (including non-exceptions) with user, session, custom properties and more
You can send non-errors to Trakerr. This will send an event without a stacktrace.

This creates a new  [Model.AppEvent](https://github.com/trakerr-io/trakerr-csharp/blob/master/generated/docs/AppEvent.md)
 using `CreateAppEvent`. You can then populate other member data using accessors (see [Model.AppEvent](https://github.com/trakerr-io/trakerr-csharp/blob/master/generated/docs/AppEvent.md)
 for more details on properties that you can set).

```csharp
    using IO.TrakerrClient;
    using IO.Trakerr.Model;

    ...

    // Instantiate the TrakerrClient globally somewhere and store. This is thread-safe.
    // Specify config without App.config (OR if using App.config you can use the parameterless constructor instead)

    var trakerrClient = TrakerrClient(
        "<api-key>",                // API KEY
        "1.0",                      // Your application version (any string)
        "production"                // Any custom deployment stage string ("production", "development", "staging" etc.)
    );

    ....

    // Create an event and then send it to Trakerr
    var infoevent = trakerrClient.CreateAppEvent(
        AppEvent.LogLevelEnum.Info,     // log level (logLevel in API)
        "Database",                     // classification -- user defined (classification in API)
        "MySQL.INSERT",                 // type -- user defined (eventType in API)
        "Success inserting data"        // some message -- user defined (eventMessage in API)
    );

    // You can set properties on AppEvent below like user, session, custom properties and more.
    infoevent.EventUser = "jill@trakerr.io";
    infoevent.EventSession = "2";

    // Set some custom data like database instance connected to and database region
    infoevent.CustomProperties = new CustomData();
    infoevent.CustomProperties.StringData = new CustomStringData("east.mysql.trakerr.com");//Add up to 10 custom strings.
    infoevent.CustomProperties.StringData.CustomData2 = "region-1"; //You can also add strings later like this.

    // Populate any other data you want, customdata or overriding default values of the appevent.
    trakerrClient.SendEventAsync(infoevent);
```

## About TrakerrClient's properties
The `TrakerrClient` class above can be constructed to take aditional data, rather than using the configured defaults. The constructor signature is:

```csharp
public TrakerrClient(string apiKey = null, string contextAppVersion = null, string contextDeploymentStage = null, string contextEnvLanguage = "C#")
```

The TrakerrClient class also has a lot of exposed properties. The benefit to setting these immediately after after you create TrakerrClient is that AppEvent will default it's values against the TrakerClient that created it. This way if there is a value that all your AppEvents uses, and the constructor default value currently doesn't suit you; it may be easier to change it in TrakerrClient as it will become the default value for all AppEvents created after. A lot of these are populated by default value by the constructor, but you can populate them with whatever string data you want. The following table provides an in depth look at each of those.

If you're populating an app event directly, you'll want to take a look at the [AppEvent properties](https://github.com/trakerr-io/trakerr-csharp/blob/master/generated/docs/AppEvent.md) as they contain properties unique to each AppEvent which do not have defaults you may set in the client.

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
**contextAppSKU** | **string** | Application SKU context. | Defaults to `null`
**contextTags** | **List<string>** | Any tags that describe the the module that this handler is for. | Defaults to `null`

## Documentation for Models

 - [Model.AppEvent](https://github.com/trakerr-io/trakerr-csharp/blob/master/generated/docs/AppEvent.md)

