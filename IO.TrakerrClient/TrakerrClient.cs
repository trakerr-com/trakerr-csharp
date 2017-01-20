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
        /// <param name="exception">The exception</param>
        /// <param name="classification">The classification ("Error", "Warning", "Info", "Debug")</param>
        public static void SendToTrakerr(this Exception exception, string classification = "Error")
        {
            var client = new TrakerrClient();

            var exceptionEvent = client.CreateAppEvent(classification, exception.GetType().ToString(), exception.Message);

            exceptionEvent.EventStacktrace = EventTraceBuilder.GetEventTraces(exception);

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
        private string contextEnvVersion;
        private string contextEnvHostname;
        private string contextAppOS;
        private string contextAppOSVersion;
        private string contextDataCenter;
        private string contextDataCenterRegion;

        /// <summary>
        /// Create a new Trakerr client to use in your application. This class is thread-safe and can be invoked from multiple threads. This class also acts as a factory to create new AppEvent's with the supplied apiKey and other data.
        /// </summary>
        /// <param name="apiKey">API Key for your application, defaults to reading "trakerr.apiKey" property under appSettings from the App.config.</param>
        /// <param name="url">URL to Trakerr, defaults to reading "trakerr.url" property under appSettings from the App.config.</param>
        /// <param name="contextAppVersion">Provide the application version, defaults to reading "trakerr.contextAppVersion" property under appSettings from the App.config.</param>
        /// <param name="contextEnvName">Provide the environemnt name (development/staging/production). You can also pass in a custom name. Defaults to reading "trakerr.contextEnvName" property under appSettings from the App.config</param>
        /// <param name="contextEnvVersion">Provide an optional context environment version.</param>
        /// <param name="contextEnvHostname">Provide the current hostname, defaults to the current DNS name if available or uses the Machine name as a fallback.</param>
        /// <param name="contextAppOS">Provide an operating system name, defaults to Environment.OSVersion.Platform along with the service pack (eg. Win32NT Service Pack 1).</param>
        /// <param name="contextAppOSVersion">Provide an operating system version, defaults to Environment.OSVersion.Version.ToString() (eg. 6.1.7601.65536)</param>
        /// <param name="contextDataCenter">Provide a datacenter name, defaults to null.</param>
        /// <param name="contextDataCenterRegion">Provide a datacenter region, defaults to null.</param>
        public TrakerrClient(string apiKey = null, string url = null, string contextAppVersion = null, string contextEnvName = "development", string contextEnvVersion = null, string contextEnvHostname = null, string contextAppOS = null, string contextAppOSVersion = null, string contextDataCenter = null, string contextDataCenterRegion = null)
        {
            if (apiKey == null) apiKey = ConfigurationManager.AppSettings["trakerr.apiKey"];
            if (url == null) url = ConfigurationManager.AppSettings["trakerr.url"];
            if (contextAppVersion == null) contextAppVersion = ConfigurationManager.AppSettings["trakerr.contextAppVersion"];
            if (contextEnvName == null) contextEnvName = ConfigurationManager.AppSettings["trakerr.contextEnvName"];

            this.apiKey = apiKey;
            this.contextAppVersion = contextAppVersion;

            this.contextEnvName = contextEnvName;
            this.contextEnvVersion = contextEnvVersion;
            if (contextEnvHostname == null)
            {
                try
                {
                    this.contextEnvHostname = Dns.GetHostName();
                }
                catch(SocketException)
                {
                    this.contextEnvHostname = Environment.MachineName;
                }
            } 
            else
            {
                this.contextEnvHostname = contextEnvHostname;
            }

            this.contextAppOS = contextAppOS == null ? Environment.OSVersion.Platform + " " + Environment.OSVersion.ServicePack : contextAppOS;
            this.contextAppOSVersion = contextAppOSVersion == null ? Environment.OSVersion.Version.ToString() : contextAppOSVersion;
            this.contextDataCenter = contextDataCenter;
            this.contextDataCenterRegion = contextDataCenterRegion;

            eventsApi = new EventsApi(url);
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
        /// Use this to bootstrap a new AppEvent object from an exception.
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
            if (appEvent.ContextEnvVersion == null) appEvent.ContextEnvVersion = this.contextEnvVersion;
            if (appEvent.ContextEnvHostname == null) appEvent.ContextEnvHostname = this.contextEnvHostname;

            if (appEvent.ContextAppOS == null)
            {
                appEvent.ContextAppOS = this.contextAppOS;
                appEvent.ContextAppOSVersion = this.contextAppOSVersion;
            }

            if (appEvent.ContextDataCenter == null) appEvent.ContextDataCenter = contextDataCenter;
            if (appEvent.ContextDataCenterRegion == null) appEvent.ContextDataCenterRegion = contextDataCenterRegion;

            if (!appEvent.EventTime.HasValue) appEvent.EventTime = (long)(DateTime.Now - DT_EPOCH).TotalMilliseconds;
        }

    }
}
