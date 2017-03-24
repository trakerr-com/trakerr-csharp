﻿using IO.Trakerr.Api;
using IO.Trakerr.Client;
using IO.Trakerr.Model;
using Microsoft.Win32;
using System;
using System.Configuration;
using System.Net;
using System.Net.Sockets;
using System.Reflection;

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
        /// <param name="classification">Optional extra string descriptor. Defaults to issue.</param>
        public static void SendToTrakerr(this Exception e, string classification = "issue")
        {
            var client = new TrakerrClient();

            client.SendEventAsync(client.CreateAppEvent(e, AppEvent.LogLevelEnum.Error, classification));
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

        public string apiKey { get; set; }
        public string contextAppVersion { get; set; }
        public string contextDeploymentStage { get; set; }

        /// <summary>
        /// A constant property with the name of the language this is compiled from. Is set in a constructor argument (so you can pass in managed c++ or VB), but defaults to c#
        /// </summary>
        public string contextEnvLanguage { get; }

        /// <summary>
        /// Name of the CLI the program is running on.
        /// </summary>
        public string contextEnvName { get; set; }

        /// <summary>
        /// The version of the CLI the application is run on.
        /// </summary>
        public string contextEnvVersion { get; set; }

        /// <summary>
        /// The hostname of the pc running the application.
        /// </summary>
        public string contextEnvHostname { get; set; }

        /// <summary>
        /// The OS the application is running on.
        /// </summary>
        public string contextAppOS { get; set; }

        /// <summary>
        /// The version of the OS the application is running on.
        /// </summary>
        public string contextAppOSVersion { get; set; }

        /// <summary>
        /// Optional. Useful For MVC and ASP.net applications the browser name the application is running on.
        /// </summary>
        public string contextAppOSBrowser { get; set; }

        /// <summary>
        /// Optional. Useful for MVC and ASP.net applications the browser version the application is running on.
        /// </summary>
        public string contextAppOSBrowserVersion { get; set; }

        /// <summary>
        /// Optional. Datacenter the application may be running on.
        /// </summary>
        public string contextDataCenter { get; set; }

        /// <summary>
        /// Optional. Datacenter region the application may be running on.
        /// </summary>
        public string contextDataCenterRegion { get; set; }

        /// <summary>
        /// Create a new Trakerr client to use in your application. This class is thread-safe and can be invoked from multiple threads. This class also acts as a factory to create new AppEvent's with the supplied apiKey and other data.
        /// </summary>
        /// <param name="apiKey">API Key for your application, defaults to reading "trakerr.apiKey" property under appSettings from the App.config.</param>
        /// <param name="contextAppVersion">Provide the application version, defaults to reading "trakerr.contextAppVersion" property under appSettings from the App.config.</param>
        /// <param name="contextDeploymentStage">Provide the string representation of the deployment stage (development/staging/production). You can also pass in a custom name. Defaults to reading "trakerr.contextDeploymentStage" property under appSettings from the App.config</param>
        /// <param name="contextEnvLanguage">String representation of the language being used. If not provided defaults to C#, but can be passed other items in a string like "VB" or "Managed C++"</param>
        public TrakerrClient(string apiKey = null, string contextAppVersion = null, string contextDeploymentStage = null, string contextEnvLanguage = "C#")
        {
            if (apiKey == null) apiKey = ConfigurationManager.AppSettings["trakerr.apiKey"];
            if (contextAppVersion == null) contextAppVersion = ConfigurationManager.AppSettings["trakerr.contextAppVersion"];
            if (contextDeploymentStage == null) contextDeploymentStage = ConfigurationManager.AppSettings["trakerr.deploymentStage"];

            this.apiKey = apiKey;
            this.contextAppVersion = contextAppVersion;
            this.contextDeploymentStage = contextDeploymentStage;

            this.contextEnvLanguage = contextEnvLanguage;
            this.contextEnvName = Type.GetType("Mono.Runtime") != null ? "Mono" : "Microsoft CLR";

            this.contextEnvVersion = null;
            Type type = Type.GetType("Mono.Runtime");
            if (type != null)
            {
                MethodInfo displayName = type.GetMethod("GetDisplayName", BindingFlags.NonPublic | BindingFlags.Static);
                if (displayName != null)
                    this.contextEnvVersion = displayName.Invoke(null, null).ToString();
            }
            else
            {
                try//Code might not work on non-windows.
                {
                    this.contextEnvVersion = TrakerrClient.Get45PlusFromRegistry();
                }
                catch
                {
                    this.contextEnvVersion = null;
                }   
            }
            

            //Refactor 2 will push what is now contextEnvVersion to contextEnvName
            //To get the version then, follow: https://msdn.microsoft.com/en-us/library/hh925568(v=vs.110).aspx for
            //Microsoft CLI and (probably) Enviroment.Version for Mono 

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

            eventsApi = new EventsApi(ConfigurationManager.AppSettings["trakerr.url"]);
        }

        /// <summary>
        /// Use this to bootstrap a new AppEvent object with the supplied logLevel, classification, event type and message.
        /// </summary>
        /// <param name="logLevel">Level (Error/Warning/Info/Debug) of the error, defaults to "Error" if null or passed in something else.</param>
        /// <param name="classification">Optional extra string descriptor. Defaults to issue.</param>
        /// <param name="eventType">Type of event (eg. System.Exception), defaults to "unknonwn"</param>
        /// <param name="eventMessage">Message, defaults to "unknown"</param>
        /// <returns>Newly created AppEvent</returns>
        public AppEvent CreateAppEvent(AppEvent.LogLevelEnum logLevel = AppEvent.LogLevelEnum.Error, string classification = "issue", string eventType = "unknown", string eventMessage = "unknown")
        {            
            return new AppEvent(this.apiKey, logLevel, classification, eventType, eventMessage);
        }

        /// <summary>
        /// Use this to bootstrap a new AppEvent object from an e.
        /// </summary>
        /// <param name="exception">The Exception to use to create the new AppEvent</param>
        /// <param name="classification">Optional extra string descriptor. Defaults to issue.</param>
        /// <returns>Newly created AppEvent</returns>
        public AppEvent CreateAppEvent(Exception exception, AppEvent.LogLevelEnum logLevel = AppEvent.LogLevelEnum.Error, string classification = "issue")
        {
            var exceptionEvent = CreateAppEvent(logLevel, classification, exception.GetType().ToString(), exception.Message);

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

            var response = eventsApi.EventsPostWithHttpInfo(appEvent);
            /*Console.Error.WriteLine("Status Code:" + response.StatusCode);
            Console.Error.WriteLine(response.Data);Debug statements*/
        }

        /// <summary>
        /// Send the AppEvent to Trakerr asynchronously. If any of the parameters supplied in the constructor are not supplied in the AppEvent parameter, this will auto-populate those members before sending the event to Trakerr.
        /// </summary>
        /// <param name="appEvent">The event to send</param>
        public async void SendEventAsync(AppEvent appEvent)
        {
            // fill defaults if not overridden in the AppEvent being passed
            FillDefaults(appEvent);

            var response = await eventsApi.EventsPostAsyncWithHttpInfo(appEvent);
            /*await Console.Error.WriteLineAsync("Status Code:" + response.StatusCode);
            await Console.Error.WriteLineAsync(response.Data.ToString());Debug statements*/
        }

        /// <summary>
        /// Creates and sends an AppEvent to Trakerr with the provided Exception and other parameters
        /// </summary>
        /// <param name="e">Exception to send to Trakerr</param>
        /// <param name="logLevel">Level of the exception that you want to be registered to trakerr (Error/Warning/Info/Debug).</param>
        /// <param name="classification">Optional extra string descriptor. Defaults to issue.</param>
        public void SendException(Exception e, AppEvent.LogLevelEnum logLevel = AppEvent.LogLevelEnum.Error, string classification = "issue")
        {
            SendEventAsync(CreateAppEvent(e, logLevel, classification));
        }

        /// <summary>
        /// Fills the default values for the properties.
        /// </summary>
        /// <param name="appEvent">The event to populate data with.</param>
        private void FillDefaults(AppEvent appEvent)
        {
            if (appEvent.ApiKey == null) appEvent.ApiKey = apiKey;
            if (appEvent.ContextAppVersion == null) appEvent.ContextAppVersion = contextAppVersion;
            appEvent.DeploymentStage = appEvent.DeploymentStage == null ? this.contextDeploymentStage : appEvent.DeploymentStage;

            appEvent.ContextEnvLanguage = appEvent.ContextEnvLanguage == null ? this.contextEnvLanguage : appEvent.ContextEnvLanguage;
            if (appEvent.ContextEnvName == null) appEvent.ContextEnvName = this.contextEnvName;
            if (appEvent.ContextEnvVersion == null) appEvent.ContextEnvVersion = this.contextEnvVersion;
            if (appEvent.ContextEnvHostname == null) appEvent.ContextEnvHostname = this.contextEnvHostname;

            if (appEvent.ContextAppOS == null)
            {
                appEvent.ContextAppOS = this.contextAppOS;
                appEvent.ContextAppOSVersion = this.contextAppOSVersion;
            }

            appEvent.ContextAppBrowser = appEvent.ContextAppBrowser == null ? this.contextAppOSBrowser : appEvent.ContextAppBrowser;
            appEvent.ContextAppBrowserVersion = appEvent.ContextAppBrowserVersion == null ? this.contextAppOSBrowserVersion : appEvent.ContextAppBrowserVersion;

            if (appEvent.ContextDataCenter == null) appEvent.ContextDataCenter = contextDataCenter;
            if (appEvent.ContextDataCenterRegion == null) appEvent.ContextDataCenterRegion = contextDataCenterRegion;

            if (!appEvent.EventTime.HasValue) appEvent.EventTime = (long)(DateTime.Now - DT_EPOCH).TotalMilliseconds;
        }

        private static string Get45PlusFromRegistry()
        {
            const string subkey = @"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\";
            using (RegistryKey ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(subkey))
            {
                if (ndpKey != null && ndpKey.GetValue("Release") != null)
                {
                    return CheckFor45PlusVersion((int)ndpKey.GetValue("Release"));
                }
                else
                {
                    return System.Environment.Version.ToString();
                }
            }
        }

        // Checking the version using >= will enable forward compatibility.
        private static string CheckFor45PlusVersion(int releaseKey)
        {
            if (releaseKey >= 394802)
                return "4.6.2 or later";
            if (releaseKey >= 394254)
            {
                return "4.6.1";
            }
            if (releaseKey >= 393295)
            {
                return "4.6";
            }
            if ((releaseKey >= 379893))
            {
                return "4.5.2";
            }
            if ((releaseKey >= 378675))
            {
                return "4.5.1";
            }
            if ((releaseKey >= 378389))
            {
                return "4.5";
            }
            // This code should never execute. A non-null release key should mean
            // that 4.5 or later is installed.
            return "No 4.5 or later version detected";
        }

    }
}
