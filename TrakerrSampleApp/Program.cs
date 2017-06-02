using IO.TrakerrClient;
using IO.Trakerr.Model;
using System;
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
            var t = execute();
            t.Wait();
            return;
        }
        public static async Task execute()
        {
            System.Threading.Thread.Sleep(2000);
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
                for(var i = 0; i < 100; i++)
                {
                    try
                    {
                        var appevent = tc.CreateAppEvent(e, AppEvent.LogLevelEnum.Fatal);//Can also change the classification.
                                                                                         //EventType and EventMessage are set automatically by create app event; you can set them manually from the appevent instance too.
                        appevent.EventUser = "john@trakerr.io";
                        appevent.EventSession = "8";

                        appevent.CustomProperties = new CustomData();
                        appevent.CustomProperties.StringData = new CustomStringData("This is string data 1!");//Add up to 10 custom strings.
                        appevent.CustomProperties.StringData.CustomData2 = "This is string data 2!";//You can also add strings later like this.
                        appevent.ContextOperationTimeMillis = 1000;

                        var response = await tc.SendEventAsync(appevent);
                        Console.WriteLine("Status[" + i + "]: " + response.StatusCode);

                    }
                    catch (Exception ie)
                    {
                        Console.WriteLine("Error["  + i + "]: " + ie.Message);
                    }
                }
            }

            //Option 4: Send a non-exception to Trakerr.
            var infoevent = tc.CreateAppEvent(AppEvent.LogLevelEnum.Info, "CLICK_EVENT", "Feature Analytics", "Some Feature");
            infoevent.EventUser = "jill@trakerr.io";
            infoevent.EventSession = "2";

            //Populate any other data you want, customdata or overriding default values of the appevent.

            await tc.SendEventAsync(infoevent);
            // Console.In.ReadLine();//Give time for the Async tasks to print to console for the sample app.
            tc.Shutdown(false); //IMPORTANT: Uncomment this line if you are using the VS debugger.
        }
    }
}


