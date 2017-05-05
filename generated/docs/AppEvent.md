# IO.Trakerr.Model.AppEvent
## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**ApiKey** | **string** | API key generated for the application | 
**LogLevel** | **string** | (optional) Logging level, one of &#39;debug&#39;,&#39;info&#39;,&#39;warning&#39;,&#39;error&#39;, &#39;fatal&#39;, defaults to &#39;error&#39; | [optional] 
**Classification** | **string** | (optional) one of &#39;issue&#39; or a custom string for non-issues, defaults to &#39;issue&#39; | 
**EventType** | **string** | type of the event or error (eg. NullPointerException) | 
**EventMessage** | **string** | message containing details of the event or error | 
**EventTime** | **long?** | (optional) event time in ms since epoch | [optional] 
**EventStacktrace** | [**Stacktrace**](Stacktrace.md) |  | [optional] 
**EventUser** | **string** | (optional) event user identifying a user | [optional] 
**EventSession** | **string** | (optional) session identification | [optional] 
**ContextAppVersion** | **string** | (optional) application version information | [optional] 
**DeploymentStage** | **string** | (optional) deployment stage, one of &#39;development&#39;,&#39;staging&#39;,&#39;production&#39; or a custom string | [optional] 
**ContextEnvName** | **string** | (optional) environment name (like &#39;cpython&#39; or &#39;ironpython&#39; etc.) | [optional] 
**ContextEnvLanguage** | **string** | (optional) language (like &#39;python&#39; or &#39;c#&#39; etc.) | [optional] 
**ContextEnvVersion** | **string** | (optional) version of environment | [optional] 
**ContextEnvHostname** | **string** | (optional) hostname or ID of environment | [optional] 
**ContextAppBrowser** | **string** | (optional) browser name if running in a browser (eg. Chrome) | [optional] 
**ContextAppBrowserVersion** | **string** | (optional) browser version if running in a browser | [optional] 
**ContextAppOS** | **string** | (optional) OS the application is running on | [optional] 
**ContextAppOSVersion** | **string** | (optional) OS version the application is running on | [optional] 
**ContextDataCenter** | **string** | (optional) Data center the application is running on or connected to | [optional] 
**ContextDataCenterRegion** | **string** | (optional) Data center region | [optional] 
**ContextTags** | **List&lt;string&gt;** |  | [optional] 
**ContextURL** | **string** | (optional) The full URL when running in a browser when the event was generated. | [optional] 
**ContextOperationTimeMillis** | **long?** | (optional) duration that this event took to occur in millis. Example - database call time in millis. | [optional] 
**ContextCpuPercentage** | **int?** | (optional) CPU utilization as a percent when event occured | [optional] 
**ContextMemoryPercentage** | **int?** | (optional) Memory utilization as a percent when event occured | [optional] 
**ContextCrossAppCorrelationId** | **string** | (optional) Cross application correlation ID | [optional] 
**ContextDevice** | **string** | (optional) Device information | [optional] 
**ContextAppSku** | **string** | (optional) Application SKU | [optional] 
**CustomProperties** | [**CustomData**](CustomData.md) |  | [optional] 
**CustomSegments** | [**CustomData**](CustomData.md) |  | [optional] 

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)

