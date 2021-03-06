/* 
 * Trakerr API
 *
 * Get your application events and errors to Trakerr via the *Trakerr API*.
 *
 * OpenAPI spec version: 1.0.0
 * 
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace IO.Trakerr.Model
{
    /// <summary>
    /// AppEvent
    /// </summary>
    [DataContract]
    public partial class AppEvent :  IEquatable<AppEvent>
    {
        /// <summary>
        /// (optional) Logging level, one of 'debug','info','warning','error', 'fatal', defaults to 'error'
        /// </summary>
        /// <value>(optional) Logging level, one of 'debug','info','warning','error', 'fatal', defaults to 'error'</value>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum LogLevelEnum
        {
            
            /// <summary>
            /// Enum Debug for "debug"
            /// </summary>
            [EnumMember(Value = "debug")]
            Debug,
            
            /// <summary>
            /// Enum Info for "info"
            /// </summary>
            [EnumMember(Value = "info")]
            Info,
            
            /// <summary>
            /// Enum Warning for "warning"
            /// </summary>
            [EnumMember(Value = "warning")]
            Warning,
            
            /// <summary>
            /// Enum Error for "error"
            /// </summary>
            [EnumMember(Value = "error")]
            Error,
            
            /// <summary>
            /// Enum Fatal for "fatal"
            /// </summary>
            [EnumMember(Value = "fatal")]
            Fatal
        }

        /// <summary>
        /// (optional) Logging level, one of 'debug','info','warning','error', 'fatal', defaults to 'error'
        /// </summary>
        /// <value>(optional) Logging level, one of 'debug','info','warning','error', 'fatal', defaults to 'error'</value>
        [DataMember(Name="logLevel", EmitDefaultValue=false)]
        public LogLevelEnum? LogLevel { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="AppEvent" /> class.
        /// </summary>
        [JsonConstructorAttribute]
        protected AppEvent() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="AppEvent" /> class.
        /// </summary>
        /// <param name="ApiKey">API key generated for the application (required).</param>
        /// <param name="LogLevel">(optional) Logging level, one of &#39;debug&#39;,&#39;info&#39;,&#39;warning&#39;,&#39;error&#39;, &#39;fatal&#39;, defaults to &#39;error&#39;.</param>
        /// <param name="Classification">(optional) one of &#39;issue&#39; or a custom string for non-issues, defaults to &#39;issue&#39; (required).</param>
        /// <param name="EventType">type of the event or error (eg. NullPointerException) (required).</param>
        /// <param name="EventMessage">message containing details of the event or error (required).</param>
        /// <param name="EventTime">(optional) event time in ms since epoch.</param>
        /// <param name="EventStacktrace">EventStacktrace.</param>
        /// <param name="EventUser">(optional) event user identifying a user.</param>
        /// <param name="EventSession">(optional) session identification.</param>
        /// <param name="ContextAppVersion">(optional) application version information.</param>
        /// <param name="DeploymentStage">(optional) deployment stage, one of &#39;development&#39;,&#39;staging&#39;,&#39;production&#39; or a custom string.</param>
        /// <param name="ContextEnvName">(optional) environment name (like &#39;cpython&#39; or &#39;ironpython&#39; etc.).</param>
        /// <param name="ContextEnvLanguage">(optional) language (like &#39;python&#39; or &#39;c#&#39; etc.).</param>
        /// <param name="ContextEnvVersion">(optional) version of environment.</param>
        /// <param name="ContextEnvHostname">(optional) hostname or ID of environment.</param>
        /// <param name="ContextAppBrowser">(optional) browser name if running in a browser (eg. Chrome).</param>
        /// <param name="ContextAppBrowserVersion">(optional) browser version if running in a browser.</param>
        /// <param name="ContextAppOS">(optional) OS the application is running on.</param>
        /// <param name="ContextAppOSVersion">(optional) OS version the application is running on.</param>
        /// <param name="ContextDataCenter">(optional) Data center the application is running on or connected to.</param>
        /// <param name="ContextDataCenterRegion">(optional) Data center region.</param>
        /// <param name="ContextTags">ContextTags.</param>
        /// <param name="ContextURL">(optional) The full URL when running in a browser when the event was generated..</param>
        /// <param name="ContextOperationTimeMillis">(optional) duration that this event took to occur in millis. Example - database call time in millis..</param>
        /// <param name="ContextCpuPercentage">(optional) CPU utilization as a percent when event occured.</param>
        /// <param name="ContextMemoryPercentage">(optional) Memory utilization as a percent when event occured.</param>
        /// <param name="ContextCrossAppCorrelationId">(optional) Cross application correlation ID.</param>
        /// <param name="ContextDevice">(optional) Device information.</param>
        /// <param name="ContextAppSku">(optional) Application SKU.</param>
        /// <param name="CustomProperties">CustomProperties.</param>
        /// <param name="CustomSegments">CustomSegments.</param>
        public AppEvent(string ApiKey = null, LogLevelEnum? LogLevel = null, string Classification = null, string EventType = null, string EventMessage = null, long? EventTime = null, Stacktrace EventStacktrace = null, string EventUser = null, string EventSession = null, string ContextAppVersion = null, string DeploymentStage = null, string ContextEnvName = null, string ContextEnvLanguage = null, string ContextEnvVersion = null, string ContextEnvHostname = null, string ContextAppBrowser = null, string ContextAppBrowserVersion = null, string ContextAppOS = null, string ContextAppOSVersion = null, string ContextDataCenter = null, string ContextDataCenterRegion = null, List<string> ContextTags = null, string ContextURL = null, long? ContextOperationTimeMillis = null, int? ContextCpuPercentage = null, int? ContextMemoryPercentage = null, string ContextCrossAppCorrelationId = null, string ContextDevice = null, string ContextAppSku = null, CustomData CustomProperties = null, CustomData CustomSegments = null)
        {
            // to ensure "ApiKey" is required (not null)
            if (ApiKey == null)
            {
                throw new InvalidDataException("ApiKey is a required property for AppEvent and cannot be null");
            }
            else
            {
                this.ApiKey = ApiKey;
            }
            // to ensure "Classification" is required (not null)
            if (Classification == null)
            {
                throw new InvalidDataException("Classification is a required property for AppEvent and cannot be null");
            }
            else
            {
                this.Classification = Classification;
            }
            // to ensure "EventType" is required (not null)
            if (EventType == null)
            {
                throw new InvalidDataException("EventType is a required property for AppEvent and cannot be null");
            }
            else
            {
                this.EventType = EventType;
            }
            // to ensure "EventMessage" is required (not null)
            if (EventMessage == null)
            {
                throw new InvalidDataException("EventMessage is a required property for AppEvent and cannot be null");
            }
            else
            {
                this.EventMessage = EventMessage;
            }
            this.LogLevel = LogLevel;
            this.EventTime = EventTime;
            this.EventStacktrace = EventStacktrace;
            this.EventUser = EventUser;
            this.EventSession = EventSession;
            this.ContextAppVersion = ContextAppVersion;
            this.DeploymentStage = DeploymentStage;
            this.ContextEnvName = ContextEnvName;
            this.ContextEnvLanguage = ContextEnvLanguage;
            this.ContextEnvVersion = ContextEnvVersion;
            this.ContextEnvHostname = ContextEnvHostname;
            this.ContextAppBrowser = ContextAppBrowser;
            this.ContextAppBrowserVersion = ContextAppBrowserVersion;
            this.ContextAppOS = ContextAppOS;
            this.ContextAppOSVersion = ContextAppOSVersion;
            this.ContextDataCenter = ContextDataCenter;
            this.ContextDataCenterRegion = ContextDataCenterRegion;
            this.ContextTags = ContextTags;
            this.ContextURL = ContextURL;
            this.ContextOperationTimeMillis = ContextOperationTimeMillis;
            this.ContextCpuPercentage = ContextCpuPercentage;
            this.ContextMemoryPercentage = ContextMemoryPercentage;
            this.ContextCrossAppCorrelationId = ContextCrossAppCorrelationId;
            this.ContextDevice = ContextDevice;
            this.ContextAppSku = ContextAppSku;
            this.CustomProperties = CustomProperties;
            this.CustomSegments = CustomSegments;
        }
        
        /// <summary>
        /// API key generated for the application
        /// </summary>
        /// <value>API key generated for the application</value>
        [DataMember(Name="apiKey", EmitDefaultValue=false)]
        public string ApiKey { get; set; }
        /// <summary>
        /// (optional) one of &#39;issue&#39; or a custom string for non-issues, defaults to &#39;issue&#39;
        /// </summary>
        /// <value>(optional) one of &#39;issue&#39; or a custom string for non-issues, defaults to &#39;issue&#39;</value>
        [DataMember(Name="classification", EmitDefaultValue=false)]
        public string Classification { get; set; }
        /// <summary>
        /// type of the event or error (eg. NullPointerException)
        /// </summary>
        /// <value>type of the event or error (eg. NullPointerException)</value>
        [DataMember(Name="eventType", EmitDefaultValue=false)]
        public string EventType { get; set; }
        /// <summary>
        /// message containing details of the event or error
        /// </summary>
        /// <value>message containing details of the event or error</value>
        [DataMember(Name="eventMessage", EmitDefaultValue=false)]
        public string EventMessage { get; set; }
        /// <summary>
        /// (optional) event time in ms since epoch
        /// </summary>
        /// <value>(optional) event time in ms since epoch</value>
        [DataMember(Name="eventTime", EmitDefaultValue=false)]
        public long? EventTime { get; set; }
        /// <summary>
        /// Gets or Sets EventStacktrace
        /// </summary>
        [DataMember(Name="eventStacktrace", EmitDefaultValue=false)]
        public Stacktrace EventStacktrace { get; set; }
        /// <summary>
        /// (optional) event user identifying a user
        /// </summary>
        /// <value>(optional) event user identifying a user</value>
        [DataMember(Name="eventUser", EmitDefaultValue=false)]
        public string EventUser { get; set; }
        /// <summary>
        /// (optional) session identification
        /// </summary>
        /// <value>(optional) session identification</value>
        [DataMember(Name="eventSession", EmitDefaultValue=false)]
        public string EventSession { get; set; }
        /// <summary>
        /// (optional) application version information
        /// </summary>
        /// <value>(optional) application version information</value>
        [DataMember(Name="contextAppVersion", EmitDefaultValue=false)]
        public string ContextAppVersion { get; set; }
        /// <summary>
        /// (optional) deployment stage, one of &#39;development&#39;,&#39;staging&#39;,&#39;production&#39; or a custom string
        /// </summary>
        /// <value>(optional) deployment stage, one of &#39;development&#39;,&#39;staging&#39;,&#39;production&#39; or a custom string</value>
        [DataMember(Name="deploymentStage", EmitDefaultValue=false)]
        public string DeploymentStage { get; set; }
        /// <summary>
        /// (optional) environment name (like &#39;cpython&#39; or &#39;ironpython&#39; etc.)
        /// </summary>
        /// <value>(optional) environment name (like &#39;cpython&#39; or &#39;ironpython&#39; etc.)</value>
        [DataMember(Name="contextEnvName", EmitDefaultValue=false)]
        public string ContextEnvName { get; set; }
        /// <summary>
        /// (optional) language (like &#39;python&#39; or &#39;c#&#39; etc.)
        /// </summary>
        /// <value>(optional) language (like &#39;python&#39; or &#39;c#&#39; etc.)</value>
        [DataMember(Name="contextEnvLanguage", EmitDefaultValue=false)]
        public string ContextEnvLanguage { get; set; }
        /// <summary>
        /// (optional) version of environment
        /// </summary>
        /// <value>(optional) version of environment</value>
        [DataMember(Name="contextEnvVersion", EmitDefaultValue=false)]
        public string ContextEnvVersion { get; set; }
        /// <summary>
        /// (optional) hostname or ID of environment
        /// </summary>
        /// <value>(optional) hostname or ID of environment</value>
        [DataMember(Name="contextEnvHostname", EmitDefaultValue=false)]
        public string ContextEnvHostname { get; set; }
        /// <summary>
        /// (optional) browser name if running in a browser (eg. Chrome)
        /// </summary>
        /// <value>(optional) browser name if running in a browser (eg. Chrome)</value>
        [DataMember(Name="contextAppBrowser", EmitDefaultValue=false)]
        public string ContextAppBrowser { get; set; }
        /// <summary>
        /// (optional) browser version if running in a browser
        /// </summary>
        /// <value>(optional) browser version if running in a browser</value>
        [DataMember(Name="contextAppBrowserVersion", EmitDefaultValue=false)]
        public string ContextAppBrowserVersion { get; set; }
        /// <summary>
        /// (optional) OS the application is running on
        /// </summary>
        /// <value>(optional) OS the application is running on</value>
        [DataMember(Name="contextAppOS", EmitDefaultValue=false)]
        public string ContextAppOS { get; set; }
        /// <summary>
        /// (optional) OS version the application is running on
        /// </summary>
        /// <value>(optional) OS version the application is running on</value>
        [DataMember(Name="contextAppOSVersion", EmitDefaultValue=false)]
        public string ContextAppOSVersion { get; set; }
        /// <summary>
        /// (optional) Data center the application is running on or connected to
        /// </summary>
        /// <value>(optional) Data center the application is running on or connected to</value>
        [DataMember(Name="contextDataCenter", EmitDefaultValue=false)]
        public string ContextDataCenter { get; set; }
        /// <summary>
        /// (optional) Data center region
        /// </summary>
        /// <value>(optional) Data center region</value>
        [DataMember(Name="contextDataCenterRegion", EmitDefaultValue=false)]
        public string ContextDataCenterRegion { get; set; }
        /// <summary>
        /// Gets or Sets ContextTags
        /// </summary>
        [DataMember(Name="contextTags", EmitDefaultValue=false)]
        public List<string> ContextTags { get; set; }
        /// <summary>
        /// (optional) The full URL when running in a browser when the event was generated.
        /// </summary>
        /// <value>(optional) The full URL when running in a browser when the event was generated.</value>
        [DataMember(Name="contextURL", EmitDefaultValue=false)]
        public string ContextURL { get; set; }
        /// <summary>
        /// (optional) duration that this event took to occur in millis. Example - database call time in millis.
        /// </summary>
        /// <value>(optional) duration that this event took to occur in millis. Example - database call time in millis.</value>
        [DataMember(Name="contextOperationTimeMillis", EmitDefaultValue=false)]
        public long? ContextOperationTimeMillis { get; set; }
        /// <summary>
        /// (optional) CPU utilization as a percent when event occured
        /// </summary>
        /// <value>(optional) CPU utilization as a percent when event occured</value>
        [DataMember(Name="contextCpuPercentage", EmitDefaultValue=false)]
        public int? ContextCpuPercentage { get; set; }
        /// <summary>
        /// (optional) Memory utilization as a percent when event occured
        /// </summary>
        /// <value>(optional) Memory utilization as a percent when event occured</value>
        [DataMember(Name="contextMemoryPercentage", EmitDefaultValue=false)]
        public int? ContextMemoryPercentage { get; set; }
        /// <summary>
        /// (optional) Cross application correlation ID
        /// </summary>
        /// <value>(optional) Cross application correlation ID</value>
        [DataMember(Name="contextCrossAppCorrelationId", EmitDefaultValue=false)]
        public string ContextCrossAppCorrelationId { get; set; }
        /// <summary>
        /// (optional) Device information
        /// </summary>
        /// <value>(optional) Device information</value>
        [DataMember(Name="contextDevice", EmitDefaultValue=false)]
        public string ContextDevice { get; set; }
        /// <summary>
        /// (optional) Application SKU
        /// </summary>
        /// <value>(optional) Application SKU</value>
        [DataMember(Name="contextAppSku", EmitDefaultValue=false)]
        public string ContextAppSku { get; set; }
        /// <summary>
        /// Gets or Sets CustomProperties
        /// </summary>
        [DataMember(Name="customProperties", EmitDefaultValue=false)]
        public CustomData CustomProperties { get; set; }
        /// <summary>
        /// Gets or Sets CustomSegments
        /// </summary>
        [DataMember(Name="customSegments", EmitDefaultValue=false)]
        public CustomData CustomSegments { get; set; }
        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class AppEvent {\n");
            sb.Append("  ApiKey: ").Append(ApiKey).Append("\n");
            sb.Append("  LogLevel: ").Append(LogLevel).Append("\n");
            sb.Append("  Classification: ").Append(Classification).Append("\n");
            sb.Append("  EventType: ").Append(EventType).Append("\n");
            sb.Append("  EventMessage: ").Append(EventMessage).Append("\n");
            sb.Append("  EventTime: ").Append(EventTime).Append("\n");
            sb.Append("  EventStacktrace: ").Append(EventStacktrace).Append("\n");
            sb.Append("  EventUser: ").Append(EventUser).Append("\n");
            sb.Append("  EventSession: ").Append(EventSession).Append("\n");
            sb.Append("  ContextAppVersion: ").Append(ContextAppVersion).Append("\n");
            sb.Append("  DeploymentStage: ").Append(DeploymentStage).Append("\n");
            sb.Append("  ContextEnvName: ").Append(ContextEnvName).Append("\n");
            sb.Append("  ContextEnvLanguage: ").Append(ContextEnvLanguage).Append("\n");
            sb.Append("  ContextEnvVersion: ").Append(ContextEnvVersion).Append("\n");
            sb.Append("  ContextEnvHostname: ").Append(ContextEnvHostname).Append("\n");
            sb.Append("  ContextAppBrowser: ").Append(ContextAppBrowser).Append("\n");
            sb.Append("  ContextAppBrowserVersion: ").Append(ContextAppBrowserVersion).Append("\n");
            sb.Append("  ContextAppOS: ").Append(ContextAppOS).Append("\n");
            sb.Append("  ContextAppOSVersion: ").Append(ContextAppOSVersion).Append("\n");
            sb.Append("  ContextDataCenter: ").Append(ContextDataCenter).Append("\n");
            sb.Append("  ContextDataCenterRegion: ").Append(ContextDataCenterRegion).Append("\n");
            sb.Append("  ContextTags: ").Append(ContextTags).Append("\n");
            sb.Append("  ContextURL: ").Append(ContextURL).Append("\n");
            sb.Append("  ContextOperationTimeMillis: ").Append(ContextOperationTimeMillis).Append("\n");
            sb.Append("  ContextCpuPercentage: ").Append(ContextCpuPercentage).Append("\n");
            sb.Append("  ContextMemoryPercentage: ").Append(ContextMemoryPercentage).Append("\n");
            sb.Append("  ContextCrossAppCorrelationId: ").Append(ContextCrossAppCorrelationId).Append("\n");
            sb.Append("  ContextDevice: ").Append(ContextDevice).Append("\n");
            sb.Append("  ContextAppSku: ").Append(ContextAppSku).Append("\n");
            sb.Append("  CustomProperties: ").Append(CustomProperties).Append("\n");
            sb.Append("  CustomSegments: ").Append(CustomSegments).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
  
        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="obj">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object obj)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            return this.Equals(obj as AppEvent);
        }

        /// <summary>
        /// Returns true if AppEvent instances are equal
        /// </summary>
        /// <param name="other">Instance of AppEvent to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(AppEvent other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.ApiKey == other.ApiKey ||
                    this.ApiKey != null &&
                    this.ApiKey.Equals(other.ApiKey)
                ) && 
                (
                    this.LogLevel == other.LogLevel ||
                    this.LogLevel != null &&
                    this.LogLevel.Equals(other.LogLevel)
                ) && 
                (
                    this.Classification == other.Classification ||
                    this.Classification != null &&
                    this.Classification.Equals(other.Classification)
                ) && 
                (
                    this.EventType == other.EventType ||
                    this.EventType != null &&
                    this.EventType.Equals(other.EventType)
                ) && 
                (
                    this.EventMessage == other.EventMessage ||
                    this.EventMessage != null &&
                    this.EventMessage.Equals(other.EventMessage)
                ) && 
                (
                    this.EventTime == other.EventTime ||
                    this.EventTime != null &&
                    this.EventTime.Equals(other.EventTime)
                ) && 
                (
                    this.EventStacktrace == other.EventStacktrace ||
                    this.EventStacktrace != null &&
                    this.EventStacktrace.Equals(other.EventStacktrace)
                ) && 
                (
                    this.EventUser == other.EventUser ||
                    this.EventUser != null &&
                    this.EventUser.Equals(other.EventUser)
                ) && 
                (
                    this.EventSession == other.EventSession ||
                    this.EventSession != null &&
                    this.EventSession.Equals(other.EventSession)
                ) && 
                (
                    this.ContextAppVersion == other.ContextAppVersion ||
                    this.ContextAppVersion != null &&
                    this.ContextAppVersion.Equals(other.ContextAppVersion)
                ) && 
                (
                    this.DeploymentStage == other.DeploymentStage ||
                    this.DeploymentStage != null &&
                    this.DeploymentStage.Equals(other.DeploymentStage)
                ) && 
                (
                    this.ContextEnvName == other.ContextEnvName ||
                    this.ContextEnvName != null &&
                    this.ContextEnvName.Equals(other.ContextEnvName)
                ) && 
                (
                    this.ContextEnvLanguage == other.ContextEnvLanguage ||
                    this.ContextEnvLanguage != null &&
                    this.ContextEnvLanguage.Equals(other.ContextEnvLanguage)
                ) && 
                (
                    this.ContextEnvVersion == other.ContextEnvVersion ||
                    this.ContextEnvVersion != null &&
                    this.ContextEnvVersion.Equals(other.ContextEnvVersion)
                ) && 
                (
                    this.ContextEnvHostname == other.ContextEnvHostname ||
                    this.ContextEnvHostname != null &&
                    this.ContextEnvHostname.Equals(other.ContextEnvHostname)
                ) && 
                (
                    this.ContextAppBrowser == other.ContextAppBrowser ||
                    this.ContextAppBrowser != null &&
                    this.ContextAppBrowser.Equals(other.ContextAppBrowser)
                ) && 
                (
                    this.ContextAppBrowserVersion == other.ContextAppBrowserVersion ||
                    this.ContextAppBrowserVersion != null &&
                    this.ContextAppBrowserVersion.Equals(other.ContextAppBrowserVersion)
                ) && 
                (
                    this.ContextAppOS == other.ContextAppOS ||
                    this.ContextAppOS != null &&
                    this.ContextAppOS.Equals(other.ContextAppOS)
                ) && 
                (
                    this.ContextAppOSVersion == other.ContextAppOSVersion ||
                    this.ContextAppOSVersion != null &&
                    this.ContextAppOSVersion.Equals(other.ContextAppOSVersion)
                ) && 
                (
                    this.ContextDataCenter == other.ContextDataCenter ||
                    this.ContextDataCenter != null &&
                    this.ContextDataCenter.Equals(other.ContextDataCenter)
                ) && 
                (
                    this.ContextDataCenterRegion == other.ContextDataCenterRegion ||
                    this.ContextDataCenterRegion != null &&
                    this.ContextDataCenterRegion.Equals(other.ContextDataCenterRegion)
                ) && 
                (
                    this.ContextTags == other.ContextTags ||
                    this.ContextTags != null &&
                    this.ContextTags.SequenceEqual(other.ContextTags)
                ) && 
                (
                    this.ContextURL == other.ContextURL ||
                    this.ContextURL != null &&
                    this.ContextURL.Equals(other.ContextURL)
                ) && 
                (
                    this.ContextOperationTimeMillis == other.ContextOperationTimeMillis ||
                    this.ContextOperationTimeMillis != null &&
                    this.ContextOperationTimeMillis.Equals(other.ContextOperationTimeMillis)
                ) && 
                (
                    this.ContextCpuPercentage == other.ContextCpuPercentage ||
                    this.ContextCpuPercentage != null &&
                    this.ContextCpuPercentage.Equals(other.ContextCpuPercentage)
                ) && 
                (
                    this.ContextMemoryPercentage == other.ContextMemoryPercentage ||
                    this.ContextMemoryPercentage != null &&
                    this.ContextMemoryPercentage.Equals(other.ContextMemoryPercentage)
                ) && 
                (
                    this.ContextCrossAppCorrelationId == other.ContextCrossAppCorrelationId ||
                    this.ContextCrossAppCorrelationId != null &&
                    this.ContextCrossAppCorrelationId.Equals(other.ContextCrossAppCorrelationId)
                ) && 
                (
                    this.ContextDevice == other.ContextDevice ||
                    this.ContextDevice != null &&
                    this.ContextDevice.Equals(other.ContextDevice)
                ) && 
                (
                    this.ContextAppSku == other.ContextAppSku ||
                    this.ContextAppSku != null &&
                    this.ContextAppSku.Equals(other.ContextAppSku)
                ) && 
                (
                    this.CustomProperties == other.CustomProperties ||
                    this.CustomProperties != null &&
                    this.CustomProperties.Equals(other.CustomProperties)
                ) && 
                (
                    this.CustomSegments == other.CustomSegments ||
                    this.CustomSegments != null &&
                    this.CustomSegments.Equals(other.CustomSegments)
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            // credit: http://stackoverflow.com/a/263416/677735
            unchecked // Overflow is fine, just wrap
            {
                int hash = 41;
                // Suitable nullity checks etc, of course :)
                if (this.ApiKey != null)
                    hash = hash * 59 + this.ApiKey.GetHashCode();
                if (this.LogLevel != null)
                    hash = hash * 59 + this.LogLevel.GetHashCode();
                if (this.Classification != null)
                    hash = hash * 59 + this.Classification.GetHashCode();
                if (this.EventType != null)
                    hash = hash * 59 + this.EventType.GetHashCode();
                if (this.EventMessage != null)
                    hash = hash * 59 + this.EventMessage.GetHashCode();
                if (this.EventTime != null)
                    hash = hash * 59 + this.EventTime.GetHashCode();
                if (this.EventStacktrace != null)
                    hash = hash * 59 + this.EventStacktrace.GetHashCode();
                if (this.EventUser != null)
                    hash = hash * 59 + this.EventUser.GetHashCode();
                if (this.EventSession != null)
                    hash = hash * 59 + this.EventSession.GetHashCode();
                if (this.ContextAppVersion != null)
                    hash = hash * 59 + this.ContextAppVersion.GetHashCode();
                if (this.DeploymentStage != null)
                    hash = hash * 59 + this.DeploymentStage.GetHashCode();
                if (this.ContextEnvName != null)
                    hash = hash * 59 + this.ContextEnvName.GetHashCode();
                if (this.ContextEnvLanguage != null)
                    hash = hash * 59 + this.ContextEnvLanguage.GetHashCode();
                if (this.ContextEnvVersion != null)
                    hash = hash * 59 + this.ContextEnvVersion.GetHashCode();
                if (this.ContextEnvHostname != null)
                    hash = hash * 59 + this.ContextEnvHostname.GetHashCode();
                if (this.ContextAppBrowser != null)
                    hash = hash * 59 + this.ContextAppBrowser.GetHashCode();
                if (this.ContextAppBrowserVersion != null)
                    hash = hash * 59 + this.ContextAppBrowserVersion.GetHashCode();
                if (this.ContextAppOS != null)
                    hash = hash * 59 + this.ContextAppOS.GetHashCode();
                if (this.ContextAppOSVersion != null)
                    hash = hash * 59 + this.ContextAppOSVersion.GetHashCode();
                if (this.ContextDataCenter != null)
                    hash = hash * 59 + this.ContextDataCenter.GetHashCode();
                if (this.ContextDataCenterRegion != null)
                    hash = hash * 59 + this.ContextDataCenterRegion.GetHashCode();
                if (this.ContextTags != null)
                    hash = hash * 59 + this.ContextTags.GetHashCode();
                if (this.ContextURL != null)
                    hash = hash * 59 + this.ContextURL.GetHashCode();
                if (this.ContextOperationTimeMillis != null)
                    hash = hash * 59 + this.ContextOperationTimeMillis.GetHashCode();
                if (this.ContextCpuPercentage != null)
                    hash = hash * 59 + this.ContextCpuPercentage.GetHashCode();
                if (this.ContextMemoryPercentage != null)
                    hash = hash * 59 + this.ContextMemoryPercentage.GetHashCode();
                if (this.ContextCrossAppCorrelationId != null)
                    hash = hash * 59 + this.ContextCrossAppCorrelationId.GetHashCode();
                if (this.ContextDevice != null)
                    hash = hash * 59 + this.ContextDevice.GetHashCode();
                if (this.ContextAppSku != null)
                    hash = hash * 59 + this.ContextAppSku.GetHashCode();
                if (this.CustomProperties != null)
                    hash = hash * 59 + this.CustomProperties.GetHashCode();
                if (this.CustomSegments != null)
                    hash = hash * 59 + this.CustomSegments.GetHashCode();
                return hash;
            }
        }
    }

}
