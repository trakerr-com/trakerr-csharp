using IO.Trakerr.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;


namespace IO.Trakerr.Client
{
    /// <summary>
    /// EventTraceBuilder uses the swagger codegenned classes to serialize the system stacktrace and returns it.
    /// See the swagger code for more information about the stacktrace object.
    /// </summary>
    public class EventTraceBuilder
    {
        /// <summary>
        /// Returns a Stactrace object contain the e stactrace, line by line.
        /// </summary>
        /// <param name="e">The e that's stacktrace should be parsed.</param>
        /// <returns>A Stacktrace object which holds a list of InnerStackTraces with the data.</returns>
        public static Stacktrace GetEventTraces(Exception e)
        {
            if (e == null) return null;
            Stacktrace traces = new Stacktrace();
            AddStackTrace(traces, e);
            return traces;
        }

        /// <summary>
        /// Adds an InnerStackTrace to the trace object and parses it.
        /// </summary>
        /// <param name="traces">A lost of InnerStackTraces which gets one gets added too.
        /// Should normally be a Stacktrace object.</param>
        /// <param name="e">The exception object to parse from.</param>
        private static void AddStackTrace(List<InnerStackTrace> traces, Exception e)
        {
            InnerStackTrace newTrace = new InnerStackTrace();
            MethodBase catchingMethod;

            newTrace.TraceLines = GetEventTraceLines(e, out catchingMethod);
            newTrace.Type = e.GetType().FullName;
            newTrace.Message = e.Message;
            traces.Add(newTrace);

            if (e.InnerException != null)
            {
                AddStackTrace(traces, e.InnerException);
            }
        }

        /// <summary>
        /// Parses each specific line in the trace and adds it to the InnerStackTraces in a serializable list format.
        /// </summary>
        /// <param name="e">The exception to parse.</param>
        /// <param name="catchingMethod"> A reference to the method that threw the error.</param>
        /// <returns>A StackTraceLines object which holds the parsed Stacktrace.</returns>
        private static StackTraceLines GetEventTraceLines(Exception e, out MethodBase catchingMethod)
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
            var stackTrace = new StackTrace(e, true);
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
