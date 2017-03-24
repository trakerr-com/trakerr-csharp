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
            //Option 1: Send to Trakerr automatically.
            try
            {
                
                throw new Exception("This is a test exception.");
            }
            catch (Exception e)
            {
                // Send the event to Trakerr
                e.SendToTrakerr();
            }

            //Option 2: Send to Trakker using the client API.
            TrakerrClient tc = new TrakerrClient();

            try
            {
                throw new ArgumentException("Args are invalid.");
            }
            catch (Exception e)
            {
                tc.SendException(e);//Can also change the log level, along with the classifcation, unlike the above which only changes issue.
            }

            //Option 3: Send an exception to Trakerr with custom data.
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

            //Option 4: Send a non-exception to Trakerr.
            var infoevent = tc.CreateAppEvent(AppEvent.LogLevelEnum.Info, "User sends clicked this button", "Feature Analytics", "Some Feature");
            infoevent.EventUser = "jill@trakerr.io";
            infoevent.EventSession = "2";

            //Populate any other data you want, customdata or overriding default values of the appevent.

            tc.SendEventAsync(infoevent);

            Console.In.ReadLine();//Give time for the Async tasks to print to console for the sample app.
        }
    }
}
