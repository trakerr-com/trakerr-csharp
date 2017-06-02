using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;

namespace IO.TrakerrClient
{
    public sealed class CPUUsageTrackerFactory
    {
        /// <summary>
        /// 
        /// </summary>
        public static CPUUsageTracker CpuUsageTracker
        {
            get
            {
                return CPUUsageTracker.instance;
            }
        }

        public sealed class CPUUsageTracker
        {
            internal static readonly CPUUsageTracker instance = new CPUUsageTracker(1000);

            private volatile bool shutdown = false;
            private PerformanceCounter cpuCounter;
            private Thread pollingthread;
            private int interval;
            private volatile int cpupercentuse = 0;

            /// <summary>
            /// 
            /// </summary>
            /// <param name="pollinterval"></param>
            private CPUUsageTracker(int pollinterval)
            {
                try
                {
                    cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
                    cpuCounter.NextValue();
                }
                catch (Win32Exception)
                {
                    //Error with the WMI.
                }
                catch (PlatformNotSupportedException)
                {
                    //Windows this program is run on is too old.
                }
                catch (UnauthorizedAccessException)
                {
                    Console.Error.WriteLine("Logger does not have permission to get the CPU info");
                }
                interval = pollinterval;

                if (cpuCounter != null)
                {
                    pollingthread = new Thread(new ThreadStart(Poll));
                    pollingthread.Start();
                }
            }


            /// <summary>
            /// 
            /// </summary>
            public int CpuPercentUse
            {
                get
                {
                    return cpupercentuse;
                }
                private set
                {
                    cpupercentuse = value;
                }
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="forceShudown"></param>
            public void Shutdown(bool forceShudown)
            {
                shutdown = true;
                if (pollingthread != null && forceShudown)
                {
                    pollingthread.Abort();
                }
            }

            /// <summary>
            /// 
            /// </summary>
            private void Poll()
            {
                try
                {
                    while (true)
                    {
                        if (shutdown) return;
                        try
                        {
                            CpuPercentUse = (int)Math.Round(cpuCounter.NextValue(), MidpointRounding.AwayFromZero);
                        }
                        catch (Win32Exception)
                        {
                            //Error with the WMI.
                        }
                        catch (PlatformNotSupportedException)
                        {
                            //Windows this program is run on is too old.
                            return;
                        }
                        catch (UnauthorizedAccessException)
                        {
                            //Needs to be run from a more elavted positions.
                            return;
                        }
                        Thread.Sleep(interval);
                    }

                }
                finally
                {
                    Console.WriteLine("CPU THREAD EXITTING: " + shutdown);
                }

            }
        }

    }


}
