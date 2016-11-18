using IO.Trakerr.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IO.Trakerr.Client
{
    public class EventTraceBuilder
    {
        public static Stacktrace GetEventTraces(Exception e)
        {
            if (e == null) return null;
            Stacktrace traces = new Stacktrace();
            AddStackTrace(traces, e);
            return traces;
        }

        private static void AddStackTrace(List<InnerStackTrace> traces, Exception exception)
        {
            InnerStackTrace newTrace = new InnerStackTrace();
            MethodBase catchingMethod;

            newTrace.TraceLines = GetEventTraceLines(exception, out catchingMethod);
            newTrace.Type = exception.GetType().FullName;
            newTrace.Message = exception.Message;
            traces.Add(newTrace);

            if (exception.InnerException != null)
            {
                AddStackTrace(traces, exception.InnerException);
            }
        }


        private static StackTraceLines GetEventTraceLines(Exception exception, out MethodBase catchingMethod)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            if (assembly.EntryPoint == null)
                assembly = Assembly.GetCallingAssembly();

            if (assembly.EntryPoint == null)
                assembly = Assembly.GetEntryAssembly();

            catchingMethod = assembly == null
                                 ? null
                                 : assembly.EntryPoint;

            StackTraceLines lines = new StackTraceLines();
            var stackTrace = new StackTrace(exception, true);
            StackFrame[] frames = stackTrace.GetFrames();

            if (frames == null || frames.Length == 0)
            {
                StackTraceLine line = new StackTraceLine();
                lines.Add(line);
                line.File = "unknown";
                line.Line = 0;
                line.Function = "unknown";
                return lines;
            }

            foreach (StackFrame frame in frames)
            {
                MethodBase method = frame.GetMethod();

                catchingMethod = method;

                int lineNumber = frame.GetFileLineNumber();

                if (lineNumber == 0)
                {
                    lineNumber = frame.GetILOffset();
                }

                string file = frame.GetFileName();

                if (String.IsNullOrEmpty(file))
                {
                    // disable ConditionIsAlwaysTrueOrFalse
                    file = method.ReflectedType != null
                               ? method.ReflectedType.FullName
                               : "(unknown)";
                    // restore ConditionIsAlwaysTrueOrFalse
                }

                StackTraceLine line = new StackTraceLine();
                line.File = file;
                line.Line = lineNumber;
                line.Function = method.Name;

                lines.Add(line);
            }

            return lines;
        }
    }
}
