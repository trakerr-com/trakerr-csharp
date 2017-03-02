using IO.Trakerr.Api;
using IO.Trakerr.Client;
using IO.Trakerr.Model;
using System;
using System.Configuration;
using System.Net;
using System.Net.Sockets;

/// <summary>
/// Trakerr.IO namespace
/// </summary>
namespace IO.TrakerrClient
{
    /// <summary>
    /// This extends Exception to make it easier to send Exceptions to Trakerr.
    /// 
    /// To use it just call e.SendToTrakerr("Warning"); where e is the Exception.
    /// </summary>
    public static class TrakerrException
    {
        /// <summary>
        /// Send an exception to Trakerr.
        /// </summary>
        /// <param name="e">The exception caught.</param>
        /// <param name="classification">The classification ("Error", "Warning", "Info", "Debug")</param>
        public static void SendToTrakerr(this Exception e, string classification = "Error")
        {
            var client = new TrakerrClient();

            var exceptionEvent = client.CreateAppEvent(classification, e.GetType().ToString(), e.Message);

            exceptionEvent.EventStacktrace = EventTraceBuilder.GetEventTraces(e);

            client.SendEventAsync(exceptionEvent);
        }
    }

    /// <summary>
    /// Client to create and send events to Trakerr.
    /// 
    /// This class uses the App.config to bootstrap certain parameters if those parameters are not passed in the constructor. Here is an example App.config setup with the relevant keys  under appSettings.
    /// 
    /// &lt;?xml version="1.0" encoding="utf-8" ?&gt;
    /// &lt;configuration&gt;
    ///     &lt;startup&gt; 
    ///         &lt;supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.5.2" /&gt;
    ///     &lt;/startup&gt;
    ///     &lt;appSettings&gt;
    ///       &lt;add key="trakerr.apiKey" value="a7a2807a2e8fd4602f70e9e8f819790a267213934083" /&gt;
    ///       &lt;add key="trakerr.url" value="https://trakerr.io/api/v1/" /&gt;
    ///       &lt;add key="trakerr.contextAppVersion" value="1.0" /&gt;
    ///       &lt;add key="trakerr.contextEnvName" value="development"/&gt;
    ///     &lt;/appSettings&gt;
    /// &lt;/configuration&gt;
    /// </summary>
    public class TrakerrClient
    {
        private static DateTime DT_EPOCH = new DateTime(1970, 1, 1);
        private EventsApi eventsApi;

        private string apiKey;
        private string contextAppVersion;
        private string contextEnvName;

        /// <summary>
        /// The version of the CLI the application is run on.
        /// </summary>
        public string ContextEnvVersion { get; set; }

        /// <summary>
        /// The hostname of the pc running the application.
        /// </summary>
        public string ContextEnvHostname { get; set; }

        /// <summary>
        /// The OS the application is running on.
        /// </summary>
        public string ContextAppOS { get; set; }

        /// <summary>
        /// The version of the OS the application is running on.
        /// </summary>
        public string ContextAppOSVersion { get; set; }

        /// <summary>
        /// Optional. Useful For MVC and ASP.net applications the browser name the application is running on.
        /// </summary>
        public string ContextAppOSBrowser { get; set; }

        /// <summary>
        /// Optional. Useful for MVC and ASP.net applications the browser version the application is running on.
        /// </summary>
        public string ContextAppOSBrowserVersion { get; set; }

        /// <summary>
        /// Optional. Datacenter the application may be running on.
        /// </summary>
        public string ContextDataCenter { get; set; }

        /// <summary>
        /// Optional. Datacenter region the application may be running on.
        /// </summary>
        public string ContextDataCenterRegion { get; set; }

        /// <summary>
        /// Create a new Trakerr client to use in your application. This class is thread-safe and can be invoked from multiple threads. This class also acts as a factory to create new AppEvent's with the supplied apiKey and other data.
        /// </summary>
        /// <param name="apiKey">API Key for your application, defaults to reading "trakerr.apiKey" property under appSettings from the App.config.</param>
        /// <param name="contextAppVersion">Provide the application version, defaults to reading "trakerr.contextAppVersion" property under appSettings from the App.config.</param>
        /// <param name="contextEnvName">Provide the environemnt name (development/staging/production). You can also pass in a custom name. Defaults to reading "trakerr.contextEnvName" property under appSettings from the App.config</param>
        public TrakerrClient(string apiKey = null, string contextAppVersion = null, string contextEnvName = "development")
        {
            if (apiKey == null) apiKey = ConfigurationManager.AppSettings["trakerr.apiKey"];
            if (contextAppVersion == null) contextAppVersion = ConfigurationManager.AppSettings["trakerr.contextAppVersion"];
            if (contextEnvName == null) contextEnvName = ConfigurationManager.AppSettings["trakerr.contextEnvName"];

            this.apiKey = apiKey;
            this.contextAppVersion = contextAppVersion;

            this.contextEnvName = contextEnvName;
            this.ContextEnvVersion = Type.GetType("Mono.Runtime") != null ? "Mono" : "Microsoft CLI";

            //Refactor 2 will push what is now contextEnvVersion to contextEnvName
            //To get the version then, follow: https://msdn.microsoft.com/en-us/library/hh925568(v=vs.110).aspx for
            //Microsoft CLI and (probably) Enviroment.Version for Mono 

            if (ContextEnvHostname == null)
            {
                try
                {
                    this.ContextEnvHostname = Dns.GetHostName();
                }
                catch(SocketException)
                {
                    this.ContextEnvHostname = Environment.MachineName;
                }
            } 
            else
            {
                this.ContextEnvHostname = ContextEnvHostname;
            }

            this.ContextAppOS = ContextAppOS == null ? Environment.OSVersion.Platform + " " + Environment.OSVersion.ServicePack : ContextAppOS;
            this.ContextAppOSVersion = ContextAppOSVersion == null ? Environment.OSVersion.Version.ToString() : ContextAppOSVersion;

            eventsApi = new EventsApi(ConfigurationManager.AppSettings["trakerr.url"]);
        }

        /// <summary>
        /// Use this to bootstrap a new AppEvent object with the supplied classification, event type and message.
        /// </summary>
        /// <param name="classification">Classification (Error/Warning/Info/Debug or custom string), defaults to "Error".</param>
        /// <param name="eventType">Type of event (eg. System.Exception), defaults to "unknonwn"</param>
        /// <param name="eventMessage">Message, defaults to "unknown"</param>
        /// <returns>Newly created AppEvent</returns>
        public AppEvent CreateAppEvent(string classification = "Error", string eventType = "unknown", string eventMessage = "unknown")
        {
            return new AppEvent(this.apiKey, classification, eventType, eventMessage);
        }

        /// <summary>
        /// Use this to bootstrap a new AppEvent object from an e.
        /// </summary>
        /// <param name="exception">The Exception to use to create the new AppEvent</param>
        /// <returns>Newly created AppEvent</returns>
        public AppEvent CreateAppEventFromException(string classification, Exception exception)
        {
            var exceptionEvent = CreateAppEvent(classification, exception.GetType().ToString(), exception.Message);

            exceptionEvent.EventStacktrace = EventTraceBuilder.GetEventTraces(exception);

            return exceptionEvent;
        }

        /// <summary>
        /// Send the AppEvent to Trakerr. If any of the parameters supplied in the constructor are not present, this will auto-populate those members on the supplied event before sending the event to Trakerr.
        /// </summary>
        /// <param name="appEvent">The event to send</param>
        public void SendEvent(AppEvent appEvent)
        {
            // fill defaults if not overridden in the AppEvent being passed
            FillDefaults(appEvent);

            eventsApi.EventsPost(appEvent);
        }

        /// <summary>
        /// Send the AppEvent to Trakerr asynchronously. If any of the parameters supplied in the constructor are not supplied in the AppEvent parameter, this will auto-populate those members before sending the event to Trakerr.
        /// </summary>
        /// <param name="appEvent">The event to send</param>
        public async void SendEventAsync(AppEvent appEvent)
        {
            // fill defaults if not overridden in the AppEvent being passed
            FillDefaults(appEvent);

            await eventsApi.EventsPostAsync(appEvent);
        }


        /// <summary>
        /// Fills the default values for the properties.
        /// </summary>
        /// <param name="appEvent">The event to populate data with.</param>
        private void FillDefaults(AppEvent appEvent)
        {
            if (appEvent.ApiKey == null) appEvent.ApiKey = apiKey;

            if (appEvent.ContextAppVersion == null) appEvent.ContextAppVersion = contextAppVersion;

            if (appEvent.ContextEnvName == null) appEvent.ContextEnvName = this.contextEnvName;
            if (appEvent.ContextEnvVersion == null) appEvent.ContextEnvVersion = this.ContextEnvVersion;
            if (appEvent.ContextEnvHostname == null) appEvent.ContextEnvHostname = this.ContextEnvHostname;

            if (appEvent.ContextAppOS == null)
            {
                appEvent.ContextAppOS = this.ContextAppOS;
                appEvent.ContextAppOSVersion = this.ContextAppOSVersion;
            }

            appEvent.ContextAppBrowser = appEvent.ContextAppBrowser == null ? this.ContextAppOSBrowser : appEvent.ContextAppBrowser;
            appEvent.ContextAppBrowserVersion = appEvent.ContextAppBrowserVersion == null ? this.ContextAppOSBrowserVersion : appEvent.ContextAppBrowserVersion;

            if (appEvent.ContextDataCenter == null) appEvent.ContextDataCenter = ContextDataCenter;
            if (appEvent.ContextDataCenterRegion == null) appEvent.ContextDataCenterRegion = ContextDataCenterRegion;

            if (!appEvent.EventTime.HasValue) appEvent.EventTime = (long)(DateTime.Now - DT_EPOCH).TotalMilliseconds;
        }

    }
}
