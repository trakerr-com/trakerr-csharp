using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;

namespace IO.TrakerrClient
{
    class CPUUsageTracker
    {
        private static CPUUsageTracker cpuusagetracker;
        private static uint numref = 0;

        private PerformanceCounter cpuCounter;
        private volatile int cpupercentuse = 0;
        private Thread pollingthread;
        private int interval;


        /// <summary>
        /// 
        /// </summary>
        public bool IsShutdown { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public static CPUUsageTracker CpuUsageTracker
        {
            get
            {
                if (cpuusagetracker == null)
                {
                    cpuusagetracker = new CPUUsageTracker(1000);
                }
                numref++;
                return cpuusagetracker;
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
        /// <param name="forceShudown"></param>
        public void Shutdown(bool forceShudown)
        {
            if (numref > 0) numref--;
            if (numref == 0) IsShutdown = true;

            if (pollingthread != null && forceShudown)
            {
                pollingthread.Abort();
                IsShutdown = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void Poll()
        {
            while (true)
            {
                if (IsShutdown) return;
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
                }
                catch (UnauthorizedAccessException)
                {
                    //Needs to be run from a more elavted positions.
                }
                Thread.Sleep(interval);
            }

        }
    }


}
