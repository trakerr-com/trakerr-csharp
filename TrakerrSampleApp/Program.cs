using IO.TrakerrClient;
using System;
using System.Reflection;
using Microsoft.Win32;

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
            Console.Out.WriteLine("Done!");
            Console.In.ReadLine();//Give time for the Async tasks to print to console.
        }
    }
}
