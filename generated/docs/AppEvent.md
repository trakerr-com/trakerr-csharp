# IO.Trakerr.Model.AppEvent
## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**ApiKey** | **string** | API key generated for the application | 
**Classification** | **string** | one of &#39;debug&#39;,&#39;info&#39;,&#39;warning&#39;,&#39;error&#39; or a custom string | 
**EventType** | **string** | type or event or error (eg. NullPointerException) | 
**EventMessage** | **string** | message containing details of the event or error | 
**EventTime** | **long?** | (optional) event time in ms since epoch | [optional] 
**EventStacktrace** | [**Stacktrace**](Stacktrace.md) |  | [optional] 
**EventUser** | **string** | (optional) event user identifying a user | [optional] 
**EventSession** | **string** | (optional) session identification | [optional] 
**ContextAppVersion** | **string** | (optional) application version information | [optional] 
**ContextEnvName** | **string** | (optional) one of &#39;development&#39;,&#39;staging&#39;,&#39;production&#39; or a custom string | [optional] 
**ContextEnvVersion** | **string** | (optional) version of environment | [optional] 
**ContextEnvHostname** | **string** | (optional) hostname or ID of environment | [optional] 
**ContextAppBrowser** | **string** | (optional) browser name if running in a browser (eg. Chrome) | [optional] 
**ContextAppBrowserVersion** | **string** | (optional) browser version if running in a browser | [optional] 
**ContextAppOS** | **string** | (optional) OS the application is running on | [optional] 
**ContextAppOSVersion** | **string** | (optional) OS version the application is running on | [optional] 
**ContextDataCenter** | **string** | (optional) Data center the application is running on or connected to | [optional] 
**ContextDataCenterRegion** | **string** | (optional) Data center region | [optional] 
**CustomProperties** | [**CustomData**](CustomData.md) |  | [optional] 
**CustomSegments** | [**CustomData**](CustomData.md) |  | [optional] 

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)

